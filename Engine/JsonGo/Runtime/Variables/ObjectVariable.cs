using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// any custom objects serialization
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class ObjectVariable<TObject> : BaseVariable, ISerializationVariable<TObject>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ObjectVariable() : base(typeof(TObject))
        {

        }

        BasePropertyGoInfo<TObject>[] Properties { get; set; }
        TypeGoInfo<TObject> TypeGoInfo;
        /// <summary>
        /// static serialized value one time calculated
        /// </summary>
        public string StaticSerializedValue;
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TObject> typeGoInfo, ITypeGo options)
        {
            TypeGoInfo = typeGoInfo;
            typeGoInfo.IsNoQuotesValueType = false;
            var baseType = Nullable.GetUnderlyingType(typeGoInfo.Type);
            if (baseType == null)
                baseType = typeGoInfo.Type;
            baseType = ReflectionHelper.GenerateTypeFromInterface(baseType, options);
            var properties = ReflectionHelper.GetListOfProperties(baseType).ToList();
            Properties = new BasePropertyGoInfo<TObject>[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var propertyInfo = (BasePropertyGoInfo<TObject>)Activator.CreateInstance(typeof(PropertyGoInfo<,>)
                   .MakeGenericType(typeof(TObject), property.PropertyType), property, options);
                typeGoInfo.Properties[property.Name] = propertyInfo;
                propertyInfo.Type = property.PropertyType;
                propertyInfo.Name = property.Name;
                propertyInfo.NameBytes = options.Encoding.GetBytes(property.Name);
                Properties[i] = propertyInfo;
            }
            typeGoInfo.DefaultBinaryValue = new byte[] { 0 };

            //set delegates to access faster and make it pointer directly usgae
            typeGoInfo.JsonSerialize = JsonSerialize;

            //set delegates to access faster and make it pointer directly usage for json deserializer
            typeGoInfo.JsonDeserialize = JsonDeserialize;

            //set delegates to access faster and make it pointer directly usage for binary serializer
            typeGoInfo.BinarySerialize = BinarySerialize;

            //set delegates to access faster and make it pointer directly usage for binary deserializer
            typeGoInfo.BinaryDeserialize = BinaryDeserialize;

            //create instance of object
            typeGoInfo.CreateInstance = ReflectionHelper.GetActivator<TObject>(baseType);

            typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.ToArray();
            typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.ToArray();

            //cache serialization data to memory to use it always without calculation
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(JsonConstantsString.OpenBraket);
            for (int i = 0; i < Properties.Length; i++)
            {
                var property = Properties[i];
                property.NameSerialized += JsonConstantsString.Quotes;
                property.NameSerialized += property.Name;
                property.NameSerialized += JsonConstantsString.QuotesColon;
            }
            //if (stringBuilder[stringBuilder.Length - 1] == JsonConstantsString.Comma)
            //    stringBuilder.Length--;
            //stringBuilder.Append(JsonConstantsString.CloseBracket);
            //StaticSerializedValue = JsonConstantsString.OpenBraket;
        }

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref TObject value)
        {
            handler.TextWriter.Write(JsonConstantsString.OpenBraket);
            for (int i = 0; i < Properties.Length; i++)
            {
                var property = Properties[i];
                object propertyValue = property.InternalGetValue(ref value);
                if (propertyValue == null || propertyValue.Equals(property.DefaultValue))
                    continue;
                handler.TextWriter.Write(property.NameSerialized);
                property.JsonSerialize(ref handler, ref propertyValue);
                handler.TextWriter.Write(JsonConstantsString.Comma);
            }
            handler.TextWriter.RemoveLast(JsonConstantsString.Comma);
            handler.TextWriter.Write(JsonConstantsString.CloseBracket);
        }
        //public void JsonSerialize(ref JsonStringSerializeHandler handler, ref TObject value)
        //{
        //    handler.AppendChar(JsonConstantsString.OpenBraket);
        //    for (int i = 0; i < Properties.Length; i++)
        //    {
        //        var property = Properties[i];
        //        object propertyValue = property.InternalGetValue(ref value);
        //        if (propertyValue == null || propertyValue.Equals(property.DefaultValue))
        //            continue;
        //        handler.AppendChar(JsonConstantsString.Quotes);
        //        handler.Append(property.Name);
        //        handler.Append(JsonConstantsString.QuotesColon);
        //        property.JsonSerialize(ref handler, ref propertyValue);
        //        handler.AppendChar(JsonConstantsString.Comma);

        //    }

        //    //Remove Last Comma
        //    handler.RemoveLastCommaCharacter();

        //    handler.AppendChar(JsonConstantsString.CloseBracket);
        //    //handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
        //}

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public TObject JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="data">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder<byte> stream, ref TObject data)
        {
            if (data == null)
            {
                //flag this object is null
                stream.Write(0);
                return;
            }
            //flag this object is not null
            stream.Write(1);
            var len = Properties.Length;
            for (int i = 0; i < len; i++)
            {
                var property = Properties[i];
                var value = property.InternalGetValue(ref data);
                property.BinarySerialize(ref stream, ref value);
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public TObject BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read(1)[0] == 0)
                return default;
            var instance = TypeGoInfo.CreateInstance();
            var properties = TypeGoInfo.DeserializeProperties;
            var len = properties.Length;
            for (int i = 0; i < len; i++)
            {
                var property = properties[i];
                var value = property.BinaryDeserialize(ref reader);
                property.InternalSetValue(ref instance, ref value);
            }
            return instance;
        }
    }
}

//public class Alaki
//{
//    /// <summary>
//    /// Initalizes TypeGo variable
//    /// </summary>
//    /// <param name="typeGoInfo">TypeGo variable to initialize</param>
//    /// <param name="options">Serializer or deserializer options</param>
//    public override void Initialize<T>(TypeGoInfo<T> typeGoInfo, IBaseTypeGo options)
//    {
//        typeGoInfo.IsNoQuotesValueType = false;
//        var baseType = Nullable.GetUnderlyingType(typeGoInfo.Type);
//        if (baseType == null)
//            baseType = typeGoInfo.Type;
//        baseType = ReflectionHelper.GenerateTypeFromInterface(baseType);
//        foreach (var property in ReflectionHelper.GetListOfProperties(baseType))
//        {
//            IPropertyCallerInfo del = null;
//            try
//            {
//                del = ReflectionHelper.GetDelegateInstance(baseType, property);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Cannot create delegate for property {property.Name} in type {typeGoInfo.Type.FullName}", ex);
//            }
//            if (!options.TryGetValueOfTypeGo(property.PropertyType, out object typeGoInfoProperty))
//            {
//                var method = typeof(TypeGoInfo<>).MakeGenericType(property.PropertyType).GetMethod("Generate", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
//                typeGoInfoProperty = method.Invoke(null, new object[] { options });
//            }

//            object propertyInfo = Activator.CreateInstance(typeof(PropertyGoInfo<,>).MakeGenericType(typeof(TObject), property.PropertyType));
//            typeGoInfo.Properties[property.Name] = propertyInfo;


//            propertyInfo.TypeGoInfo = typeGoInfoProperty;
//            propertyInfo.Type = property.PropertyType;
//            propertyInfo.Name = property.Name;
//            //propertyInfo.JsonGetValue = (handler, x) => del.GetPropertyValue(x);
//            //propertyInfo.JsonSetValue = del.SetPropertyValue;
//            propertyInfo.GetValue = del.GetPropertyValue;
//            propertyInfo.SetValue = del.SetValue;
//        }

//        foreach (var item in baseType.GetFields())
//        {
//            if (item.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
//                continue;

//            if (!options.TryGetValueOfTypeGo(item.FieldType, out TypeGoInfo typeGoInfoProperty))
//            {
//                typeGoInfoProperty = Generate(item.FieldType, options);
//            }
//            typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
//            {
//                TypeGoInfo = typeGoInfoProperty,
//                Type = item.FieldType,
//                Name = item.Name,
//                //GetValue = item.GetValue,
//                //SetValue = item.SetValue
//            };
//        }

//        //set the default value of variable
//        typeGoInfo.DefaultValue = default;
//    }

//    /// <summary>
//    /// json serialize
//    /// </summary>
//    /// <param name="handler"></param>
//    /// <param name="value"></param>
//    public void JsonSerialize(ref JsonSerializeHandler handler, ref T value)
//    {
//        if (options.HasGenerateRefrencedTypes)
//        {
//            //add $Id dproperties
//            typeGoInfo.Properties[JsonConstantsString.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
//            {
//                TypeGoInfo = Generate(typeof(int), options),
//                Type = typeof(int),
//                Name = JsonConstantsString.IdRefrencedTypeNameNoQuotes,
//                JsonSetValue = (serializer, instance, value) =>
//                {
//                    serializer.DeSerializedObjects.Add((int)value, instance);
//                },
//                JsonGetValue = (handler, data) =>
//                {
//                    if (!handler.TryGetValueOfSerializedObjects(data, out int refrencedId))
//                    {
//                        var serializer = handler.Serializer;
//                        serializer.ReferencedIndex++;
//                        handler.AddSerializedObjects(data, serializer.ReferencedIndex);
//                        return serializer.ReferencedIndex;
//                    }
//                    else
//                    {
//                        return refrencedId;
//                    }
//                }
//            };
//        }


//        typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonGetValue != null).ToArray();
//        typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonSetValue != null).ToArray();
//        if (options.HasGenerateRefrencedTypes)
//        {
//            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
//            {
//                if (handler.TryGetValueOfSerializedObjects(data, out int refrencedId))
//                {
//                    handler.AppendChar(JsonConstantsString.OpenBraket);
//                    handler.Append(JsonConstantsString.RefRefrencedTypeName);
//                    handler.AppendChar(JsonConstantsString.Colon);
//                    handler.Append(refrencedId.ToString(CurrentCulture));
//                    handler.AppendChar(JsonConstantsString.CloseBracket);
//                }
//                //else
//                //    handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
//            };
//        }
//        else
//        {
//            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
//            {
//                Func<char, StringBuilder> appendChar = handler.AppendChar;
//                RefFunc<StringBuilder> append = handler.Append;

//                appendChar(JsonConstantsString.OpenBraket);

//                var properties = typeGoInfo.SerializeProperties;
//                var length = properties.Length;
//                for (int i = 0; i < length; i++)
//                {
//                    var property = properties[i];
//                    var propertyType = property.TypeGoInfo;
//                    object propertyValue = property.JsonGetValue(handler, data);
//                    if (propertyValue == null || propertyValue.Equals(propertyType.DefaultValue))
//                        continue;
//                    appendChar(JsonConstantsString.Quotes);
//                    append(property.Name);
//                    append(JsonConstantsString.QuotesColon);
//                    propertyType.JsonSerialize(handler, ref propertyValue);
//                    appendChar(JsonConstantsString.Comma);
//                }

//                //Remove Last Comma
//                handler.RemoveLastCommaCharacter();

//                appendChar(JsonConstantsString.CloseBracket);
//                //handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
//            };

//            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
//            {
//                var properties = typeGoInfo.SerializeProperties;
//                var len = properties.Length;
//                for (int i = 0; i < len; i++)
//                {
//                    var property = properties[i];
//                    var value = property.GetValue(data);
//                    if (value == null || value == property.TypeGoInfo.DefaultValue)
//                    {

//                    }
//                    else
//                        property.TypeGoInfo.BinarySerialize(stream, ref value);
//                }
//            };

//            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
//            {

//            };
//        }

//        typeGoInfo.CreateInstance = GetActivator(baseType);
//        typeGoInfo.DefaultValue = null;
//    }
//}