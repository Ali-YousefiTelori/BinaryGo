
using JsonGo.Helpers;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    internal static class FastDeserializerExtractor2
    {
        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="typeGo"></param>
        /// <param name="instance"></param>
        /// <param name="json">json value</param>
        /// <param name="createInstance">index of start string</param>
        /// <returns>value deserialized</returns>
        internal static void Extract(TypeGoInfo typeGo, ref object instance, Func<object> createInstance, ref ReadOnlySpan<byte> _buffer)
        {
            int startIndex = 0;
            int endIndex = 0;
            // StringBuilder stringBuilder = null;
            int index = -1;
            int length = _buffer.Length;

            ExtractEmpty:
            index++;
            if (index >= length)
            {
                return;
            }
            var character = _buffer[index];
            if (JsonSpanReader2.SkipValues.Contains(character))
                goto ExtractEmpty;
            else
            {
                if (character == JsonConstants.Comma)
                {
                    goto ExtractEmpty;
                }
                else if (character == JsonConstants.Colon)
                {
                    goto ExtractEmpty;
                }
                else if (character == JsonConstants.OpenBraket)
                {
                    goto CreateObject;
                }
                else if (character == JsonConstants.OpenSquareBrackets)
                {

                }
                else if (character == JsonConstants.Quotes)
                {
                    goto ExtractString;
                }
                else
                    goto ExtractValue;

            }

            CreateObject:
            //stackReader = new StackReader(new StringBuilder()) { Parent = stackReader };
            if (createInstance != null)
                instance = createInstance();
            goto ExtractEmpty;


            ExtractString:
            startIndex = index;
            endIndex = index;
            //stringBuilder = new StringBuilder();
            bool canSkip = false;
            while (true)
            {
                index++;
                character = _buffer[index];
                if (character == JsonConstants.Quotes)
                {
                    if (canSkip)
                    {
                        canSkip = false;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (character == JsonConstants.BackSlash)
                    canSkip = true;
                endIndex = index;
                //stringBuilder.Append((char)character);
            }

            goto ExtractEmpty;

            ExtractValue:
            startIndex = index;
            endIndex = index;
            //stringBuilder = new StringBuilder();
            //stringBuilder.Append((char)_buffer[index]);
            while (true)
            {
                index++;
                character = _buffer[index];
                if (character == JsonConstants.Space || character == JsonConstants.Comma || character == JsonConstants.CloseBracket || character == JsonConstants.CloseSquareBrackets)
                {
                    break;
                }
                else
                    endIndex = index;
            }
            goto ExtractEmpty;
        }

        //static void ExtractArray(TypeGoInfo typeGo, object instance, ref JsonSpanReader json)
        //{
        //    var generic = typeGo.Generics.First();
        //    while (true)
        //    {
        //        var character = json.Read();
        //        if (character == JsonConstants.OpenBraket)
        //        {
        //            var genericInstance = generic.CreateInstance();
        //            ExtractOject(generic, genericInstance, ref json);
        //            typeGo.AddArrayValue(instance, genericInstance);
        //        }
        //        else if (character == JsonConstants.OpenSquareBrackets)
        //        {
        //            var genericInstance = generic.CreateInstance();
        //            ExtractArray(generic, genericInstance, ref json);
        //            typeGo.AddArrayValue(instance, genericInstance);
        //        }
        //        else if (character == JsonConstants.Comma)
        //        {
        //            continue;
        //        }
        //        else if (character == JsonConstants.CloseSquareBrackets)
        //        {
        //            break;
        //        }
        //        else if (character == JsonConstants.Quotes)
        //        {
        //            json.ExtractString();
        //        }
        //        else
        //            throw new Exception($"end of character not support '{character}' index of {"json._Index"} i think i must find '}}' character");

        //    }

        //}

        ///// <summary>
        ///// extract list of properties from object
        ///// </summary>
        ///// <param name="typeGo"></param>
        ///// <param name="instance"></param>
        ///// <param name="json"></param>
        ///// <param name="indexOf"></param>
        ///// <returns></returns>
        //static void ExtractOject(TypeGoInfo typeGo, object instance, ref JsonSpanReader json)
        //{
        //    while (!json.IsFinished)
        //    {
        //        //read tp uneascape char
        //        var character = json.Read();
        //        if (character == JsonConstants.Comma)
        //            continue;
        //        else if (character == JsonConstants.CloseBracket)
        //        {
        //            break;
        //        }
        //        var key = json.ExtractKey();
        //        //read tp uneascape char
        //        json.Read();

        //        if (typeGo.Properties.TryGetValue(TextHelper.SpanToString(key), out PropertyGoInfo propertyGo))
        //        {
        //            object propertyInstance = instance;
        //            var value = Extract(propertyGo.TypeGoInfo, ref propertyInstance, propertyGo.TypeGoInfo.CreateInstance, ref json);
        //            var deserialize = propertyGo.TypeGoInfo.Deserialize;
        //            if (deserialize != null)
        //                propertyGo.SetValue(propertyInstance, deserialize(value));
        //            else
        //                propertyGo.SetValue(instance, propertyInstance);
        //        }
        //        else
        //        {
        //            Extract(propertyGo.TypeGoInfo, ref instance, null, ref json);
        //        }
        //    }
        //}
    }
}
