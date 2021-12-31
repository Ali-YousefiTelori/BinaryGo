using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Array serializer and deserializer
    /// </summary>
    public class ArrayVariable<T> : BaseVariable, ISerializationVariable<T[]>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ArrayVariable() : base(typeof(T[]))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="arrayTypeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<T[]> arrayTypeGoInfo, ITypeOptions options)
        {
            arrayTypeGoInfo.IsNoQuotesValueType = false;
            //set the default value of variable
            arrayTypeGoInfo.DefaultValue = default;

            if (TryGetValueOfTypeGo(typeof(T), out object result))
                typeGoInfo = (TypeGoInfo<T>)result;
            else
                typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);

            arrayTypeGoInfo.JsonSerialize = JsonSerialize;

            //set delegates to access faster and make it pointer directly usage for json deserializer
            arrayTypeGoInfo.JsonDeserialize = JsonDeserialize;

            //set delegates to access faster and make it pointer directly usage for binary serializer
            arrayTypeGoInfo.BinarySerialize = BinarySerialize;

            //set delegates to access faster and make it pointer directly usage for binary deserializer
            arrayTypeGoInfo.BinaryDeserialize = BinaryDeserialize;

            //create instance of object
            //arrayTypeGoInfo.CreateInstance = ReflectionHelper.GetActivator<TObject>(baseType);

            CastToArray = arrayTypeGoInfo.Cast;
            arrayTypeGoInfo.CreateInstance = () => new T[0]; 
        }

        Func<T[], object> CastToArray;
        //type of one of element
        TypeGoInfo<T> typeGoInfo = null;

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref T[] value)
        {
            if (value != null)
            {
                handler.TextWriter.Write(JsonConstantsString.OpenSquareBrackets);

                for (int i = 0; i < value.Length; i++)
                {
                    T obj = value[i];
                    typeGoInfo.JsonSerialize(ref handler, ref obj);
                    handler.TextWriter.Write(JsonConstantsString.Comma);
                }

                handler.TextWriter.RemoveLast(JsonConstantsString.Comma);
                handler.TextWriter.Write(JsonConstantsString.CloseSquareBrackets);
            }
            else
            {
                handler.TextWriter.Write(JsonConstantsString.Null);
            }
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public T[] JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            throw new NotSupportedException();
            //List<T> array = new List<T>();
            //while (true)
            //{
            //    var character = text.Read();
            //    if (character == JsonConstantsString.OpenBraket)
            //    {
            //        var obj = ExtractOject(deserializer, generic, ref json);
            //        typeGo.AddArrayValue(arrayInstance, obj);
            //    }
            //    else if (character == JsonConstantsString.OpenSquareBrackets)
            //    {
            //        var obj = ExtractArray(deserializer, generic, ref json);
            //        typeGo.AddArrayValue(arrayInstance, obj);
            //    }
            //    else if (character == JsonConstantsString.Comma)
            //    {
            //        continue;
            //    }
            //    else if (character == JsonConstantsString.CloseSquareBrackets)
            //    {
            //        break;
            //    }
            //    else if (character == JsonConstantsString.Quotes)
            //    {
            //        var value = json.ExtractString();
            //        typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
            //    }
            //    else
            //    {
            //        bool isClosed = false;
            //        var value = json.ExtractValue();
            //        if (value[value.Length - 1] == JsonConstantsString.Comma)
            //            value = value.Slice(0, value.Length - 1);
            //        if (value[value.Length - 1] == JsonConstantsString.CloseSquareBrackets)
            //        {
            //            value = value.Slice(0, value.Length - 1);
            //            isClosed = true;
            //        }
            //        if (generic.JsonDeserialize != null)
            //            typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
            //        if (isClosed)
            //            break;
            //    }
            //}
            //return typeGo.Cast == null ? arrayInstance : typeGo.Cast(arrayInstance);
            //return array.ToArray();
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref T[] value)
        {
            if (value == null)
            {
                stream.Write(BitConverter.GetBytes(-1));
            }
            else
            {
                if (value.Length > 0)
                {
                    stream.Write(BitConverter.GetBytes(value.Length));
                    for (int i = 0; i < value.Length; i++)
                    {
                        T obj = value[i];
                        typeGoInfo.BinarySerialize(ref stream, ref obj);
                    }
                }
                else
                {
                    stream.Write(BitConverter.GetBytes(0));
                }
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public T[] BinaryDeserialize(ref BinarySpanReader reader)
        {
            int length = BitConverter.ToInt32(reader.Read(sizeof(int)));
            if (length == -1)
                return null; 
            else if (length == 0)
                return new T[0];
            T[] instance = new T[length];
            for (int i = 0; i < length; i++)
            {
                instance[i] = typeGoInfo.BinaryDeserialize(ref reader);
            }
            return instance;
        }
    }
}






















//public class alaki
//{
//    /// <summary>
//    /// Initalizes TypeGo variable
//    /// </summary>
//    /// <param name="typeGoInfo">TypeGo variable to initialize</param>
//    /// <param name="options">Serializer or deserializer options</param>
//    public void Initialize(TypeGoInfo typeGoInfo, IBaseTypeGo options)
//    {
//        var baseType = Nullable.GetUnderlyingType(typeGoInfo.Type);
//        if (baseType == null)
//            baseType = typeGoInfo.Type;
//        baseType = TypeGoInfo.GenerateTypeFromInterface(baseType);
//        var currentCulture = TypeGoInfo.CurrentCulture;
//        typeGoInfo.IsNoQuotesValueType = false;
//        if (options.HasGenerateRefrencedTypes)
//        {
//            //add $Id dproperties
//            typeGoInfo.Properties[JsonConstantsString.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
//            {
//                TypeGoInfo = TypeGoInfo.Generate(typeof(int), options),
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

//            typeGoInfo.Properties[JsonConstantsString.ValuesRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
//            {
//                TypeGoInfo = TypeGoInfo.Generate(typeGoInfo.Type, options),
//                Type = typeGoInfo.Type,
//                Name = JsonConstantsString.ValuesRefrencedTypeNameNoQuotes,
//                JsonSetValue = (serializer, instance, value) =>
//                {
//                    if (TypeGoInfo.Generate(instance.GetType(), options) is TypeGoInfo typeGo)
//                    {
//                        foreach (var item in (IEnumerable)value)
//                        {
//                            typeGo.AddArrayValue(instance, item);
//                        }
//                    }
//                },
//                JsonGetValue = (handler, data) =>
//                {
//                    if (data == null)
//                        return null;
//                    handler.AppendChar(JsonConstantsString.Quotes);
//                    handler.Append(JsonConstantsString.ValuesRefrencedTypeNameNoQuotes);
//                    handler.Append(JsonConstantsString.QuotesColon);
//                    handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
//                    var generic = typeGoInfo.Generics[0];
//                    foreach (var item in (IEnumerable)data)
//                    {
//                        var obj = item;
//                        generic.JsonSerialize(handler, ref obj);
//                        handler.AppendChar(JsonConstantsString.Comma);
//                    }
//                    handler.Serializer.RemoveLastComma();
//                    handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
//                    return null;
//                }
//            };
//        }
//        foreach (var item in baseType.GetGenericArguments())
//        {
//            if (!options.TryGetValueOfTypeGo(item, out TypeGoInfo typeGoInfoProperty))
//            {
//                typeGoInfoProperty = TypeGoInfo.Generate(item, options);
//            }
//            typeGoInfo.Generics.Add(typeGoInfoProperty);
//        }

//        if (baseType.IsArray)
//        {
//            var elementType = baseType.GetElementType();
//            var newType = typeof(List<>).MakeGenericType(elementType);
//            if (!options.TryGetValueOfTypeGo(elementType, out TypeGoInfo typeGoInfoProperty))
//            {
//                typeGoInfoProperty = TypeGoInfo.Generate(elementType, options);
//            }
//            typeGoInfo.Generics.Add(typeGoInfoProperty);
//            typeGoInfo.CreateInstance = TypeGoInfo.GetActivator(newType);
//            var castMethod = typeof(TypeGoInfo).GetMethod("GetArray", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
//            typeGoInfo.Cast = (obj) => castMethod.Invoke(null, new object[] { obj });
//            var method = newType.GetMethod("Add");
//            typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
//        }
//        else
//        {
//            typeGoInfo.CreateInstance = TypeGoInfo.GetActivator(baseType);
//            //use add method for deserialization
//            var method = baseType.GetMethod("Add");
//            typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
//        }

//        if (options.HasGenerateRefrencedTypes)
//        {
//            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
//            {
//                //handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
//            };
//        }
//        else
//        {
//            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
//            {
//                if (data != null)
//                {
//                    handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
//                    var generic = typeGoInfo.Generics[0];
//                    foreach (var item in (IEnumerable)data)
//                    {
//                        var obj = item;
//                        generic.JsonSerialize(handler, ref obj);
//                        handler.AppendChar(JsonConstantsString.Comma);
//                    }
//                    handler.Serializer.RemoveLastComma();
//                    handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
//                }
//                else
//                {
//                    handler.Append("null");
//                }
//            };
//            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
//            {
//                var generic = typeGoInfo.Generics[0];
//                if (data != null)
//                {
//                    if (data is ICollection collection)
//                    {
//                        stream.Write(BitConverter.GetBytes(collection.Count));
//                        foreach (var item in collection)
//                        {
//                            var obj = item;
//                            generic.BinarySerialize(stream, ref obj);
//                        }
//                    }
//                    else if (data is IEnumerable enumerable)
//                    {
//                        int count = 0;
//                        IEnumerator enumerator = enumerable.GetEnumerator();
//                        while (enumerator.MoveNext())
//                            count++;
//                        stream.Write(BitConverter.GetBytes(count));
//                        foreach (var item in enumerable)
//                        {
//                            var obj = item;
//                            generic.BinarySerialize(stream, ref obj);
//                        }
//                    }

//                }
//                else
//                {
//                    stream.Write(BitConverter.GetBytes(0));
//                }
//            };

//            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
//            {
//                var length = BitConverter.ToInt32(reader.Read(sizeof(int)));
//                if (length == 0)
//                    return null;
//                var instance = typeGoInfo.CreateInstance();
//                var generic = typeGoInfo.Generics[0];
//                for (int i = 0; i < length; i++)
//                {
//                    typeGoInfo.AddArrayValue(instance, generic.BinaryDeserialize(ref reader));
//                }
//                return typeGoInfo.Cast == null ? instance : typeGoInfo.Cast(instance);
//            };
//        }
//        typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonGetValue != null).ToArray();
//        typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonSetValue != null).ToArray();
//        typeGoInfo.DefaultValue = null;
//    }
//}