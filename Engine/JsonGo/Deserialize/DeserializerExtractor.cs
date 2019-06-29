using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Deserialize
{
    internal static class DeserializerExtractor
    {
        private const string SupportedValue = "0123456789.truefalsTRUEFALS-n";
        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="json">json value</param>
        /// <param name="indexOf">index of start string</param>
        /// <returns>value deserialized</returns>
        internal static IJsonGoModel Extract(ReadOnlySpan<char> json, ref int indexOf)
        {
            IJsonGoModel jsonGoModel = null;
            bool canSkip = true;
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (canSkip && IsWhiteSpace(ref character))
                    continue;
                if (character == '{')
                {
                    jsonGoModel = new ObjectModel();
                    indexOf++;
                    ExtractOject(jsonGoModel, json, ref indexOf);

                    return jsonGoModel;
                }
                else if (character == '[')
                {
                    jsonGoModel = new ArrayModel();
                    indexOf++;
                    ExtractArray(jsonGoModel, json, ref indexOf);
                    return jsonGoModel;
                }
                else if (character == '\"')
                {
                    jsonGoModel = new ValueModel(ExtractString(json, ref indexOf));
                    return jsonGoModel;
                }
                else
                {
                    jsonGoModel = new ValueModel(ExtractValue(json, ref indexOf));
                    return jsonGoModel;
                }

            }
            return jsonGoModel;
        }

        internal static void ExtractArray(IJsonGoModel iJsonGoModel, ReadOnlySpan<char> json, ref int indexOf)
        {
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (IsWhiteSpace(ref character))
                    continue;
                if (character == '{')
                {
                    ObjectModel jsonGoModel = new ObjectModel();
                    indexOf++;
                    ExtractOject(jsonGoModel, json, ref indexOf);
                    i = indexOf;
                    iJsonGoModel.Add(null, jsonGoModel);
                }
                else if (character == '[')
                {
                    ArrayModel jsonGoModel = new ArrayModel();
                    indexOf++;
                    ExtractArray(jsonGoModel, json, ref indexOf);
                    i = indexOf;
                    iJsonGoModel.Add(null, jsonGoModel);
                }
                else if (character == ',')
                {
                    continue;
                }
                else if (character == ']')
                {
                    break;
                }
                else if (character == '"')
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
        internal static string ExtractString(ReadOnlySpan<char> json, ref int indexOf)
        {
            StringBuilder result = new StringBuilder();
            bool started = false;
            bool canSkipOneTime = false;
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (started && character == '\\' && json.Length > i + 1 && json[i + 1] == '"')
                {
                    canSkipOneTime = true;
                    continue;
                }
                result.Append(character);
                if (character == '\"')
                {
                    if (canSkipOneTime)
                    {
                        canSkipOneTime = false;
                        continue;
                    }
                    if (!started)
                        started = true;
                    else
                        break;
                }
            }

            return result.ToString();
        }
        /// <summary>
        /// extract value from json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        internal static string ExtractValue(ReadOnlySpan<char> json, ref int indexOf)
        {
            StringBuilder result = new StringBuilder();
            bool started = false;
            for (int i = indexOf; i < json.Length; i++)
            {
                char character = json[i];
                bool found = false;
                for (int j = 0; j < SupportedValue.Length; j++)
                {
                    if (SupportedValue[j] == character)
                    {
                        if (!started)
                            started = true;
                        result.Append(character);
                        indexOf = i;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    break;
                //if (SupportedValue.Contains(character))
                //{
                //    if (!started)
                //        started = true;
                //    result.Append(character);
                //    indexOf = i;
                //}
                //else
                //    break;
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
        internal static void ExtractOject(IJsonGoModel objectModel, ReadOnlySpan<char> json, ref int indexOf)
        {
            bool canSkip = true;
            bool findKey = false;
            bool findStartOfValue = false;
            //char previousCharacter = default(char);
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (canSkip && IsWhiteSpace(ref character))
                    continue;
                if (findKey)
                {
                    keyBuilder.Append(character);
                    if (character == '\"')
                    {
                        findKey = false;
                        findStartOfValue = true;
                        canSkip = true;
                    }
                    //else
                    //    throw new Exception($"I tried to find start of key, I think here must be '\"' char instead of '{character}' char index of {i} from json {json}");
                }
                else if (findStartOfValue)
                {
                    if (character == ':')
                    {
                        indexOf++;
                        IJsonGoModel value = Extract(json, ref indexOf);
                        objectModel.Add(keyBuilder.ToString().Trim('\"'), value);
                        keyBuilder.Clear();
                        i = indexOf;
                        canSkip = true;
                        findKey = false;
                        findStartOfValue = false;
                        //previousCharacter = default(char);
                    }
                    else
                        throw new Exception($"I tried to find start of value, I think here must be ':' char instead of '{character}' char index of {i} from json {new string(json.ToArray())}");
                }
                else if (character == '\"')
                {
                    if (!findKey)
                    {
                        findKey = true;
                        keyBuilder.Append(character);
                        canSkip = false;
                    }
                }
                else if (character == '}')
                {
                    break;
                }
                else if (character == '{')
                    continue;
                else if (character != ',')
                    throw new Exception($"I tried to find start of object, I think here must be '{{' char instead of '{character}' char index of {i} from json {new string(json.ToArray())}");

                //previousCharacter = character;
            }
        }

        /// <summary>
        /// check if a character is whitespace or empty
        /// </summary>
        /// <param name="value">character to check</param>
        /// <returns>is char is white space</returns>
        internal static bool IsWhiteSpace(ref char value)
        {
            return value == '\b' || value == '\f' || value == '\n' || value == '\r' || value == '\t' || value == ' ';
        }
    }
}
