using JsonGo.Helpers;
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
        /// <param name="createInstance">index of start string</param>
        /// <returns>value deserialized</returns>
        internal static ReadOnlySpan<byte> Extract(Deserializer deserializer, TypeGoInfo typeGo, ref object instance, Func<object> createInstance, ref JsonSpanReader json)
        {
            var character = json.Read();
            if (character == JsonConstants.Quotes)
            {
                //if (typeGo.IsNoQuotesValueType)
                return json.ExtractString();
                //else
                //    return json.ExtractStringQuotes();

            }
            else if (character == JsonConstants.OpenBraket)
            {
                //if (createInstance != null)
                //    instance = createInstance();
                ExtractOject(deserializer, typeGo, ref instance, ref json);
            }
            else if (character == JsonConstants.OpenSquareBrackets)
            {
                if (typeGo.CreateInstance != null)
                    instance = typeGo.CreateInstance();
                ExtractArray(deserializer, typeGo, ref instance, ref json);
            }
            else
            {
                if (createInstance != null)
                    instance = null;
                return json.ExtractValue();
            }
            return null;
        }

        static void ExtractArray(Deserializer deserializer, TypeGoInfo typeGo, ref object instance, ref JsonSpanReader json)
        {
            var generic = typeGo.Generics.First();
            while (true)
            {
                var character = json.Read();
                if (character == JsonConstants.OpenBraket)
                {
                    object genericInstance = null;
                    ExtractOject(deserializer, generic, ref genericInstance, ref json);
                    typeGo.AddArrayValue(instance, genericInstance);
                }
                else if (character == JsonConstants.OpenSquareBrackets)
                {
                    object genericInstance = null;
                    ExtractArray(deserializer, generic, ref genericInstance, ref json);
                    typeGo.AddArrayValue(instance, genericInstance);
                }
                else if (character == JsonConstants.Comma)
                {
                    continue;
                }
                else if (character == JsonConstants.CloseSquareBrackets)
                {
                    break;
                }
                else if (character == JsonConstants.Quotes)
                {
                    var value = json.ExtractString();
                    typeGo.AddArrayValue(instance, generic.Deserialize(deserializer, value));
                }
                else
                {
                    var value = json.ExtractValue();
                    typeGo.AddArrayValue(instance, generic.Deserialize(deserializer, value));
                }

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
        static void ExtractOject(Deserializer deserializer, TypeGoInfo typeGo, ref object instance, ref JsonSpanReader json)
        {
            while (!json.IsFinished)
            {
                //read tp uneascape char
                var character = json.Read();
                if (character == JsonConstants.Comma)
                    continue;
                else if (character == JsonConstants.CloseBracket)
                {
                    break;
                }
                var key = json.ExtractKey();
                //read to uneascape char
                json.Read();
                var propertyname = TextHelper.SpanToString(key);
                if (typeGo.Properties.TryGetValue(propertyname, out PropertyGoInfo propertyGo))
                {
                    if (instance == null && typeGo.CreateInstance != null)
                        instance = typeGo.CreateInstance();
                    object propertyInstance = instance;
                    var value = Extract(deserializer, propertyGo.TypeGoInfo, ref propertyInstance, propertyGo.TypeGoInfo.CreateInstance, ref json);
                    var deserialize = propertyGo.TypeGoInfo.Deserialize;
                    if (deserialize != null)
                        propertyGo.SetValue(deserializer, propertyInstance, deserialize(deserializer, value));
                    else
                        propertyGo.SetValue(deserializer, instance, propertyInstance);
                }
                else if (propertyname == JsonConstants.ValuesRefrencedTypeNameNoQuotes)
                {
                    Extract(deserializer, typeGo, ref instance, null, ref json);
                }
                else if (propertyname == JsonConstants.RefRefrencedTypeNameNoQuotes)
                {
                    object propertyInstance = null;
                    var value = Extract(deserializer, null, ref propertyInstance, null, ref json);

                    var type = TypeGoInfo.Generate(typeof(int));
                    var result = (int)type.Deserialize(deserializer, value);
                    deserializer.DeSerializedObjects.TryGetValue(result, out instance);
                }
                else
                {
                    Extract(deserializer, propertyGo?.TypeGoInfo, ref instance, null, ref json);
                }
            }
        }
    }
}
