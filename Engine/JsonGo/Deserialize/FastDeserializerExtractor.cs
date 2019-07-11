using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    internal static class FastDeserializerExtractor
    {
        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="typeGo"></param>
        /// <param name="instance"></param>
        /// <param name="json">json value</param>
        /// <param name="indexOf">index of start string</param>
        /// <returns>value deserialized</returns>
        internal static JsonSpanReader Extract(TypeGoInfo typeGo, object instance, ref JsonSpanReader json)
        {
            var character = json.Read();
            if (character == JsonConstansts.Quotes)
            {
                return json.ExtractString();
            }
            else if (character == JsonConstansts.OpenBraket)
            {
                ExtractOject(typeGo, instance, ref json);
            }
            else if (character == JsonConstansts.OpenSquareBrackets)
            {
                ExtractArray(typeGo, instance, ref json);
            }
            else
            {
                return json.ExtractValue();
            }
            return json;
        }

        static void ExtractArray(TypeGoInfo typeGo, object instance, ref JsonSpanReader json)
        {
            var generic = typeGo.Generics.First();
            while (true)
            {
                var character = json.Read();
                if (character == JsonConstansts.OpenBraket)
                {
                    var genericInstance = generic.CreateInstance();
                    ExtractOject(generic, genericInstance, ref json);
                    typeGo.AddArrayValue(instance, genericInstance);
                }
                else if (character == JsonConstansts.OpenSquareBrackets)
                {
                    var genericInstance = generic.CreateInstance();
                    ExtractArray(generic, genericInstance, ref json);
                    typeGo.AddArrayValue(instance, genericInstance);
                }
                else if (character == JsonConstansts.Comma)
                {
                    continue;
                }
                else if (character == JsonConstansts.CloseSquareBrackets)
                {
                    break;
                }
                else if (character == JsonConstansts.Quotes)
                {
                    json.ExtractString();
                }
                else
                    throw new Exception($"end of character not support '{character}' index of {json.Index} i think i must find '}}' character");

            }

        }

        /// <summary>
        /// extract list of properties from object
        /// </summary>
        /// <param name="typeGo"></param>
        /// <param name="instance"></param>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        static void ExtractOject(TypeGoInfo typeGo, object instance, ref JsonSpanReader json)
        {
            while (!json.IsFinished)
            {
                var character = json.Read();
                if (character == JsonConstansts.Comma)
                    continue;
                else if (character == JsonConstansts.CloseBracket)
                {
                    break;
                }
                var key = json.ExtractKey();
                //read coma char
                json.Read();
                var value = Extract(typeGo, instance, ref json);
                if (typeGo.Properties.TryGetValue(key.ToString(), out PropertyGoInfo propertyGo))
                {
                    var text = value.ToString();
                    if (propertyGo.TypeGoInfo.Deserialize != null)
                        propertyGo.SetValue(instance, propertyGo.TypeGoInfo.Deserialize(text));
                    else if (text != JsonConstansts.Null)
                        propertyGo.SetValue(instance, text);
                }
            }
        }
    }
}
