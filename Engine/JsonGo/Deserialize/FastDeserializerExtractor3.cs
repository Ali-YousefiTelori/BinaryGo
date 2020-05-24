using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    internal static class FastDeserializerExtractor3
    {
        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="typeGo"></param>
        /// <param name="json">json value</param>
        /// <returns>value deserialized</returns>
        internal static object Extract(JsonDeserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader json)
        {
            var character = json.Read();
            if (character == JsonConstantsBytes.Quotes)
            {
                return typeGo.JsonDeserialize(deserializer, json.ExtractString());
            }
            else if (character == JsonConstantsBytes.OpenBraket)
            {
                return ExtractOject(deserializer, typeGo, ref json);
            }
            else if (character == JsonConstantsBytes.OpenSquareBrackets)
            {
                throw new NotSupportedException();
                //return ExtractArray(deserializer, typeGo, ref json);
            }
            else
            {
                if (typeGo == null || typeGo.JsonDeserialize == null )
                {
                    json.ExtractValue();
                    return null;
                }
                return typeGo.JsonDeserialize(deserializer, json.ExtractValue());
            }
        }

        //static object ExtractArray(Deserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader json)
        //{
        //    var generic = typeGo.Generics.First();
        //    while (true)
        //    {
        //        var character = json.Read();
        //        if (character == JsonConstantsBytes.OpenBraket)
        //        {
        //            object genericInstance = null;
        //            ExtractOject(deserializer, generic, ref genericInstance, ref json);
        //            typeGo.AddArrayValue(instance, genericInstance);
        //        }
        //        else if (character == JsonConstantsBytes.OpenSquareBrackets)
        //        {
        //            object genericInstance = null;
        //            ExtractArray(deserializer, generic, ref genericInstance, ref json);
        //            typeGo.AddArrayValue(instance, genericInstance);
        //        }
        //        else if (character == JsonConstantsBytes.Comma)
        //        {
        //            continue;
        //        }
        //        else if (character == JsonConstantsBytes.CloseSquareBrackets)
        //        {
        //            break;
        //        }
        //        else if (character == JsonConstantsBytes.Quotes)
        //        {
        //            var value = json.ExtractString();
        //            typeGo.AddArrayValue(instance, generic.Deserialize(deserializer, value));
        //        }
        //        else
        //        {
        //            var value = json.ExtractValue();
        //            if (generic.Deserialize != null)
        //                typeGo.AddArrayValue(instance, generic.Deserialize(deserializer, value));
        //        }

        //    }

        //}

        /// <summary>
        /// extract list of properties from object
        /// </summary>
        /// <param name="typeGo"></param>
        /// <param name="instance"></param>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        static object ExtractOject(JsonDeserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader json)
        {
            var instance = typeGo.CreateInstance();
            while (!json.IsFinished)
            {
                //read tp uneascape char
                var character = json.Read();
                if (character == JsonConstantsBytes.Comma)
                    continue;
                else if (character == JsonConstantsBytes.CloseBracket)
                    break;
                var key = json.ExtractKey();
                //read to uneascape char
                json.Read();
                var propertyname = new string(key);
                if (typeGo.Properties.TryGetValue(propertyname, out PropertyGoInfo propertyGo))
                {
                    var value = Extract(deserializer, propertyGo.TypeGoInfo, ref json);
                    propertyGo.JsonSetValue(deserializer, instance, value);
                }
                //else if (propertyname == JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes)
                //{
                //    Extract(deserializer, typeGo, ref json);
                //}
                //else if (propertyname == JsonConstantsBytes.RefRefrencedTypeNameNoQuotes)
                //{
                //    //object propertyInstance = null;
                //    //var value = Extract(deserializer, null, ref propertyInstance, ref json);

                //    //var type = TypeGoInfo.Generate(typeof(int), deserializer);
                //    //var result = (int)type.Deserialize(deserializer, value);
                //    //deserializer.DeSerializedObjects.TryGetValue(result, out instance);
                //}
                else
                {
                    Extract(deserializer, propertyGo?.TypeGoInfo, ref json);
                }
            }
            return null;
        }
    }
}

