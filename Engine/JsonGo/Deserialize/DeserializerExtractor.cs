using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Deserialize
{
    internal static class DeserializerExtractor
    {
        private const string SupportedValue = "0123456789.truefalsTRUEFALS-n";
        private const string WhiteSpaceValue = "\b\f\n\r\t ";

        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="json">json value</param>
        /// <param name="indexOf">index of start string</param>
        /// <returns>value deserialized</returns>
        internal static IJsonGoModel Extract(ReadOnlySpan<char> json, ref int indexOf)
        {
            foreach (var character in json.Slice(indexOf))
            {
                if (character == JsonSettingInfo.OpenBraket)
                {
                    var jsonGoModel = new ObjectModel();
                    indexOf++;
                    ExtractOject(jsonGoModel, json, ref indexOf);

                    return jsonGoModel;
                }
                else if (character == JsonSettingInfo.OpenSquareBrackets)
                {
                    var jsonGoModel = new ArrayModel();
                    indexOf++;
                    ExtractArray(jsonGoModel, json, ref indexOf);
                    return jsonGoModel;
                }
                else if (character == JsonSettingInfo.Quotes)
                {
                    var jsonGoModel = new ValueModel(ExtractString(json, ref indexOf));
                    return jsonGoModel;
                }
                else if (WhiteSpaceValue.IndexOf(character) >= 0)
                    continue;
                else
                {
                    var jsonGoModel = new ValueModel(ExtractValue(json, ref indexOf));
                    return jsonGoModel;
                }
            }
            return null;
        }

        static void ExtractArray(IJsonGoModel iJsonGoModel, ReadOnlySpan<char> json, ref int indexOf)
        {
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (WhiteSpaceValue.IndexOf(character) >= 0)
                    continue;
                if (character == JsonSettingInfo.OpenBraket)
                {
                    ObjectModel jsonGoModel = new ObjectModel();
                    indexOf++;
                    ExtractOject(jsonGoModel, json, ref indexOf);
                    i = indexOf;
                    iJsonGoModel.Add(null, jsonGoModel);
                }
                else if (character == JsonSettingInfo.OpenSquareBrackets)
                {
                    ArrayModel jsonGoModel = new ArrayModel();
                    indexOf++;
                    ExtractArray(jsonGoModel, json, ref indexOf);
                    i = indexOf;
                    iJsonGoModel.Add(null, jsonGoModel);
                }
                else if (character == JsonSettingInfo.Comma)
                {
                    continue;
                }
                else if (character == JsonSettingInfo.CloseSquareBrackets)
                {
                    break;
                }
                else if (character == JsonSettingInfo.Quotes)
                {
                    var resultString = ExtractString(json, ref indexOf);
                    ValueModel valueModel = new ValueModel(resultString);
                    i = indexOf;
                    iJsonGoModel.Add(null, valueModel);
                }
                else
                    throw new Exception($"end of character not support '{character}' index of {i} i think i must find '}}' character");

            }
        }
        /// <summary>
        /// extract string from inside of double " char
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        static string ExtractString(ReadOnlySpan<char> json, ref int indexOf)
        {
            int start = indexOf;
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (character == JsonSettingInfo.Quotes)
                {
                    indexOf++;
                    IndexOfEndString(json, ref indexOf);
                    break;
                }
            }

            return new string(json.Slice(start, indexOf - start + 1).ToArray());
        }

        static string ExtractStringFast(ReadOnlySpan<char> json, ref int indexOf)
        {
            indexOf++;
            int start = indexOf;
            IndexOfEndString(json, ref indexOf);
            return new string(json.Slice(start, indexOf - start).ToArray());
        }
        /// <summary>
        /// find index of end of string
        /// string will ends with " char
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        static void IndexOfEndString(ReadOnlySpan<char> json, ref int indexOf)
        {
            char lastChar = default;
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (character == JsonSettingInfo.Quotes && lastChar != JsonSettingInfo.BackSlash)
                    break;
                lastChar = character;
            }
        }
        /// <summary>
        /// extract value from json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        static string ExtractValue(ReadOnlySpan<char> json, ref int indexOf)
        {
            StringBuilder result = new StringBuilder();
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                bool found = false;
                for (int j = 0; j < SupportedValue.Length; j++)
                {
                    if (SupportedValue[j] == character)
                    {
                        result.Append(character);
                        found = true;
                        break;
                    }
                }
                if (!found)
                    break;
            }

            return result.ToString();
        }
        /// <summary>
        /// extract list of properties from object
        /// </summary>
        /// <param name="objectModel"></param>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        static void ExtractOject(IJsonGoModel objectModel, ReadOnlySpan<char> json, ref int indexOf)
        {
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (WhiteSpaceValue.IndexOf(character) >= 0 || character == JsonSettingInfo.Comma)
                    continue;
                else if (character == JsonSettingInfo.CloseBracket)
                {
                    break;
                }
                else if (character == JsonSettingInfo.OpenBraket)
                    continue;
                var key = FastExtractPropertyKey(json, ref indexOf);
                indexOf++;
                var value = FastExtractPropertyValue(json, ref indexOf);
                objectModel.Add(key, value);
                i = indexOf;
            }
        }

        private static IJsonGoModel FastExtractPropertyValue(ReadOnlySpan<char> json, ref int indexOf)
        {
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (WhiteSpaceValue.IndexOf(character) >= 0)
                    continue;
                if (character == JsonSettingInfo.Colon)
                {
                    indexOf++;
                    IJsonGoModel value = Extract(json, ref indexOf);
                    return value;
                }
                else
                    break;
            }
            throw new Exception($"I tried to find start of value, I think here must be ':' char instead of '{json[indexOf]}' char index of {indexOf} from json {new string(json.ToArray())}");
        }

        private static string FastExtractPropertyKey(ReadOnlySpan<char> json, ref int indexOf)
        {
            char character = json[indexOf];
            if (character == JsonSettingInfo.Quotes)
            {
                return ExtractStringFast(json, ref indexOf);
            }
            else
                throw new Exception($"for find a key from object I think I need '\"' character but i found '{character}' index of {indexOf} from json {new string(json.ToArray())}");
        }
    }
}
