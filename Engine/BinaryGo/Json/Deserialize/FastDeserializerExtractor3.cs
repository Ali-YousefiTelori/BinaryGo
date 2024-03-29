﻿using BinaryGo.Json;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryGo.Json.Deserialize
{
    internal static class FastDeserializerExtractor<T>
    {
        /// <summary>
        /// Deserialize json
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="typeGo"></param>
        /// <param name="json">json object to deserialize</param>
        /// <returns>The deserialized value</returns>
        internal static T Extract(ref JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader json)
        {
            char character = json.Read();
            if (character == JsonConstantsString.Quotes)
            {
                ReadOnlySpan<char> extract = json.ExtractString();
                return typeGo.JsonDeserialize(ref extract);
            }
            else if (character == JsonConstantsString.OpenBraket)
            {
                return ExtractOject(ref deserializer, ref typeGo, ref json);
            }
            else if (character == JsonConstantsString.OpenSquareBrackets)
            {
                return ExtractArray(deserializer, typeGo, ref json);
            }
            else
            {
                ReadOnlySpan<char> value = json.ExtractValue();
                return typeGo.JsonDeserialize(ref value);
            }
        }

        internal static void ExtractProperty(ref T instance, ref JsonDeserializer deserializer, BasePropertyGoInfo<T> basePropertyGo, ref JsonSpanReader json)
        {
            char character = json.Read();
            if (character == JsonConstantsString.Quotes)
            {
                basePropertyGo.JsonDeserializeString(ref instance, ref json);
                //return typeGo.JsonDeserialize(ref extract);
            }
            else if (character == JsonConstantsString.OpenBraket)
            {
                basePropertyGo.JsonDeserializeObject(ref instance, ref deserializer, ref json);
            }
            else if (character == JsonConstantsString.OpenSquareBrackets)
            {
                basePropertyGo.JsonDeserializeArray(ref instance, ref deserializer, ref json);
            }
            else
            {
                basePropertyGo.JsonDeserializeValue(ref instance, ref json);
                //var value = json.ExtractValue();
                //if (value[value.Length - 1] == JsonConstantsString.Comma)
                //    value = value.Slice(0, value.Length - 1);
                //if (typeGo == null || typeGo.JsonDeserialize == null)
                //{
                //    return new string(value);
                //}
                //return typeGo.JsonDeserialize(ref value);
            }
        }
        internal static T ExtractArray(JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader json)
        {
            //typeGo.JsonDeserialize(ref json);
            //var generic = typeGo.Generics.First();
            while (true)
            {
                char character = json.Read();
                if (character == JsonConstantsString.OpenBraket)
                {
                    //var obj = ExtractOject(ref deserializer, ref typeGo, ref json);
                    //typeGo.AddArrayValue(arrayInstance, obj);
                }
                else if (character == JsonConstantsString.OpenSquareBrackets)
                {
                    //var obj = ExtractArray(deserializer, generic, ref json);
                    //typeGo.AddArrayValue(arrayInstance, obj);
                }
                else if (character == JsonConstantsString.Quotes)
                {
                    //var value = json.ExtractString();
                    //typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
                }
                else if (character == JsonConstantsString.Comma)
                {
                    continue;
                }
                else if (character == JsonConstantsString.CloseSquareBrackets)
                {
                    break;
                }
                else
                {
                    //bool isClosed = false;
                    //var value = json.ExtractValue();
                    //if (value[value.Length - 1] == JsonConstantsString.Comma)
                    //    value = value.Slice(0, value.Length - 1);
                    //if (value[value.Length - 1] == JsonConstantsString.CloseSquareBrackets)
                    //{
                    //    value = value.Slice(0, value.Length - 1);
                    //    isClosed = true;
                    //}
                    //if (generic.JsonDeserialize != null)
                    //    typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
                    //if (isClosed)
                    //    break;
                }
            }
            return default;
            //return typeGo.Cast == null ? arrayInstance : typeGo.Cast(arrayInstance);
        }


        /// <summary>
        /// Extract the list of properties from an object
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="typeGo"></param>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        public static T ExtractOject(ref JsonDeserializer deserializer, ref TypeGoInfo<T> typeGo, ref JsonSpanReader jsonReader)
        {
            T instance = default;
            while (!jsonReader.IsFinished)
            {
                //read tp uneascape char
                char character = jsonReader.Read();
                if (character == JsonConstantsString.Comma)
                    continue;
                else if (character == JsonConstantsString.CloseBracket)
                    break;
                ReadOnlySpan<char> key = jsonReader.ExtractKey();
                //read to uneascape char
                jsonReader.Read();
#if (NETSTANDARD2_0 || NET45)
                string propertyname = new string(key.ToArray());
#else
                string propertyname = new string(key);
#endif
                if (typeGo.Properties.TryGetValue(propertyname, out BasePropertyGoInfo<T> basePropertyGo))
                {
                    if (instance == null)
                        instance = typeGo.CreateInstance();

                    ExtractProperty(ref instance, ref deserializer, basePropertyGo, ref jsonReader);
                    //var value = Extract(deserializer, basePropertyGo.GetTypeGoInfo<T>(), ref json);
                    //basePropertyGo.InternalSetValue(ref instance,ref value);
                }
                else if (propertyname == JsonConstantsString.ValuesRefrencedTypeNameNoQuotes)
                {
                    instance = Extract(ref deserializer, typeGo, ref jsonReader);
                }
                else if (propertyname == JsonConstantsString.RefRefrencedTypeNameNoQuotes)
                {
                    //var value = Extract(deserializer, typeGo, ref json);

                    //var type = TypeGoInfo.Generate(typeof(int), deserializer);
                    //var result = (int)type.JsonDeserialize(deserializer, (string)value);
                    //deserializer.DeSerializedObjects.TryGetValue(result, out instance);
                }
                else
                {
                    //Extract(deserializer, basePropertyGo?.TypeGoInfo, ref json);
                }
            }
            return typeGo.Cast == null ? instance : typeGo.Cast(instance);
        }
    }
}

