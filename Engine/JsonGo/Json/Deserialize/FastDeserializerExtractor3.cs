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
        internal static object Extract(JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader json)
        {
            var character = json.Read();
            if (character == JsonConstantsString.Quotes)
            {
                return typeGo.JsonDeserialize(deserializer, json.ExtractString());
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
                if (value[value.Length - 1] == JsonConstantsString.Comma)
                    value = value.Slice(0, value.Length - 1);
                if (typeGo == null || typeGo.JsonDeserialize == null)
                {
                    return new string(value);
                }
                //json.BackIndex();
                return typeGo.JsonDeserialize(deserializer, value);
            }
        }

        internal static object ExtractArray(JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader json)
        {
            throw new NotImplementedException();
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
        /// <param name="json"></param>
        /// <returns></returns>
        static object ExtractOject(JsonDeserializer deserializer, TypeGoInfo<T> typeGo, ref JsonSpanReader json)
        {
            throw new NotImplementedException();
            //object instance = null;
            //while (!json.IsFinished)
            //{
            //    //read tp uneascape char
            //    var character = json.Read();
            //    if (character == JsonConstantsString.Comma)
            //        continue;
            //    else if (character == JsonConstantsString.CloseBracket)
            //        break;
            //    var key = json.ExtractKey();
            //    //read to uneascape char
            //    json.Read();
            //    var propertyname = new string(key);
            //    if (typeGo.Properties.TryGetValue(propertyname, out BasePropertyGoInfo<T> propertyGo))
            //    {
            //        if (instance == null)
            //            instance = typeGo.CreateInstance();
            //        var value = Extract(deserializer, propertyGo.TypeGoInfo, ref json);
            //        propertyGo.JsonSetValue(deserializer, instance, value);
            //    }
            //    else if (propertyname == JsonConstantsString.ValuesRefrencedTypeNameNoQuotes)
            //    {
            //        instance = Extract(deserializer, typeGo, ref json);
            //    }
            //    else if (propertyname == JsonConstantsString.RefRefrencedTypeNameNoQuotes)
            //    {
            //        var value = Extract(deserializer, typeGo, ref json);

            //        var type = TypeGoInfo.Generate(typeof(int), deserializer);
            //        var result = (int)type.JsonDeserialize(deserializer, (string)value);
            //        deserializer.DeSerializedObjects.TryGetValue(result, out instance);
            //    }
            //    else
            //    {
            //        Extract(deserializer, propertyGo?.TypeGoInfo, ref json);
            //    }
            //}
            //return typeGo.Cast == null ? instance : typeGo.Cast(instance);
        }
    }
}

