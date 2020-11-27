using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Json.Deserialize
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
            var character = json.Read();
            if (character == JsonConstantsString.Quotes)
            {
                var extract = json.ExtractString();
                return typeGo.JsonDeserialize(ref extract);
            }
            else if (character == JsonConstantsString.OpenBraket)
            {
                return ExtractOject(deserializer, typeGo, ref json);
            }
            else if (character == JsonConstantsString.OpenSquareBrackets)
            {
                return ExtractArray(deserializer, typeGo, ref json);
            }
            else
            {
                var value = json.ExtractValue();
                return typeGo.JsonDeserialize(ref value);
            }
        }

        internal static void ExtractProperty(ref T instance ,ref JsonDeserializer deserializer, BasePropertyGoInfo<T> basePropertyGo, ref JsonSpanReader json)
        {
            var character = json.Read();
            if (character == JsonConstantsString.Quotes)
            {
                basePropertyGo.JsonDeserializeString(ref instance, ref json);
                //return typeGo.JsonDeserialize(ref extract);
            }
            else if (character == JsonConstantsString.OpenBraket)
            {
                //return ExtractOject(deserializer, typeGo, ref json);
            }
            else if (character == JsonConstantsString.OpenSquareBrackets)
            {
                return ExtractArray(deserializer, typeGo, ref json);
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
            throw new NotSupportedException();
            //var arrayInstance = typeGo.CreateInstance();
            //var generic = typeGo.Generics.First();
            //while (true)
            //{
            //    var character = json.Read();
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
        }

        /// <summary>
        /// Extract the list of properties from an object
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="typeGo"></param>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        static T ExtractOject(JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader jsonReader)
        {
            T instance = default;
            while (!jsonReader.IsFinished)
            {
                //read tp uneascape char
                var character = jsonReader.Read();
                if (character == JsonConstantsString.Comma)
                    continue;
                else if (character == JsonConstantsString.CloseBracket)
                    break;
                var key = jsonReader.ExtractKey();
                //read to uneascape char
                jsonReader.Read();
                var propertyname = new string(key);
                if (typeGo.Properties.TryGetValue(propertyname, out BasePropertyGoInfo<T> basePropertyGo))
                {
                    if (instance == null)
                        instance = typeGo.CreateInstance();

                    ExtractProperty(ref instance,ref deserializer, basePropertyGo, ref jsonReader);
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

