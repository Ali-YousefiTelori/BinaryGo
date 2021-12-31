using BinaryGo.Binary.Deserialize;
using BinaryGo.Binary.StructureModels;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryGo.Runtime.Variables
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

        internal BasePropertyGoInfo<TObject>[] Properties;
        int PropertiesLength;
        TypeGoInfo<TObject> TypeGoInfo;
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TObject> typeGoInfo, ITypeOptions options)
        {
            TypeGoInfo = typeGoInfo;
            typeGoInfo.IsNoQuotesValueType = false;
            Type baseType = Nullable.GetUnderlyingType(typeGoInfo.Type);
            if (baseType == null)
                baseType = typeGoInfo.Type;
            baseType = ReflectionHelper.GenerateTypeFromInterface(baseType, options);
            
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
            GenerateProperties();
        }

        internal void GenerateProperties()
        {
            Type baseType = Nullable.GetUnderlyingType(TypeGoInfo.Type);
            if (baseType == null)
                baseType = TypeGoInfo.Type;
            baseType = ReflectionHelper.GenerateTypeFromInterface(baseType, Options);

            List<System.Reflection.PropertyInfo> properties = ReflectionHelper.GetListOfProperties(baseType).ToList();
            Properties = new BasePropertyGoInfo<TObject>[properties.Count];
            PropertiesLength = properties.Count;
            for (int i = 0; i < properties.Count; i++)
            {
                System.Reflection.PropertyInfo property = properties[i];
                BasePropertyGoInfo<TObject> propertyInfo = (BasePropertyGoInfo<TObject>)Activator.CreateInstance(typeof(PropertyGoInfo<,>)
                   .MakeGenericType(typeof(TObject), property.PropertyType), property, Options);
                TypeGoInfo.Properties[property.Name] = propertyInfo;
                propertyInfo.Index = i;
                propertyInfo.Type = property.PropertyType;
                propertyInfo.Name = property.Name;
                propertyInfo.NameBytes = Options.Encoding.GetBytes(property.Name);
                Properties[i] = propertyInfo;
            }
            for (int i = 0; i < Properties.Length; i++)
            {
                BasePropertyGoInfo<TObject> property = Properties[i];
                property.NameSerialized = JsonConstantsString.Quotes.ToString();
                property.NameSerialized += property.Name;
                property.NameSerialized += JsonConstantsString.QuotesColon;
            }
        }

        internal void RebuildProperties(List<MemberBinaryModelInfo> properties)
        {
            Properties = new BasePropertyGoInfo<TObject>[TypeGoInfo.Properties.Count];
            PropertiesLength = TypeGoInfo.Properties.Count;
            int i = 0;
            foreach (KeyValuePair<string, BasePropertyGoInfo<TObject>> propertyKeyValue in TypeGoInfo.Properties)
            {
                BasePropertyGoInfo<TObject> property = propertyKeyValue.Value;
                property.Name = propertyKeyValue.Key;
                property.NameBytes = Options.Encoding.GetBytes(property.Name);
                Properties[i] = property;
                MemberBinaryModelInfo findProperty = properties.FirstOrDefault(x => x.Name == property.Name);
                property.Index = findProperty.Index;
                i++;
            }

            //order properties
            //re indexing properties because generation will set index of properties
            Properties = Properties.OrderBy(x => x.Index).ToArray();
            for (i = 0; i < Properties.Length; i++)
            {
                BasePropertyGoInfo<TObject> property = Properties[i];
                property.NameSerialized = JsonConstantsString.Quotes.ToString();
                property.NameSerialized += property.Name;
                property.NameSerialized += JsonConstantsString.QuotesColon;
            }
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
                BasePropertyGoInfo<TObject> property = Properties[i];
                property.TypedJsonSerialize(ref handler, ref value);
                //object propertyValue = property.InternalGetValue(ref value);
                //if (propertyValue == null || propertyValue.Equals(property.DefaultValue))
                //    continue;
                //handler.TextWriter.Write(property.NameSerialized);
                //property.JsonSerialize(ref handler, ref propertyValue);
                //handler.TextWriter.Write(JsonConstantsString.Comma);
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
        public void BinarySerialize(ref BufferBuilder stream, ref TObject data)
        {
            if (data == null)
            {
                //flag this object is null
                stream.Write(0);
            }
            else
            {
                //flag this object is not null
                stream.Write(1);
                for (int i = 0; i < PropertiesLength; i++)
                {
                    //var value = property.InternalGetValue(ref data);
                    Properties[i].BinarySerialize(ref stream, ref data);
                }
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
            TObject instance = TypeGoInfo.CreateInstance();
            int len = Properties.Length;
            for (int i = 0; i < len; i++)
            {
                BasePropertyGoInfo<TObject> property = Properties[i];
                //var value = property.BinaryDeserialize(ref reader);
                property.BinaryDeserialize(ref reader, ref instance);
                //property.InternalSetValue(ref instance, ref value);
            }
            return instance;
        }
    }
}