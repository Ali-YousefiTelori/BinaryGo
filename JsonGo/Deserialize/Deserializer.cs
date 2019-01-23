using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Deserialize
{
    public class Deserializer
    {
        static Deserializer()
        {
            SingleIntance = new Deserializer();
        }

        private const string SupportedValue = "0123456789.truefalsTRUEFALS";
        /// <summary>
        /// save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<string, object> DeSerializedObjects { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// cache variable to access faster, methods,fields and properties
        /// </summary>
        internal static ConcurrentDictionary<Type, ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>> CacheNameVariables { get; set; } = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>>();

        /// <summary>
        /// single instance of deserialize to access faster
        /// </summary>
        public static Deserializer SingleIntance { get; set; }
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonSettingInfo Setting { get; set; } = new JsonSettingInfo();
        /// <summary>
        /// deserialize a json to a type
        /// </summary>
        /// <typeparam name="T">type of deserialize</typeparam>
        /// <param name="json">json to deserialize</param>
        /// <returns>deserialized type</returns>
        public T Dersialize<T>(string json)
        {
            int indexOf = 0;
            DeSerializedObjects.Clear();
            IJsonGoModel jsonModel = Extract(ref json, ref indexOf);
            return (T)jsonModel.Generate(typeof(T), this);
        }

        /// <summary>
        /// deserialize a json to a type
        /// </summary>
        /// <param name="type">type of deserialize</param>
        /// <param name="json">json to deserialize</param>
        /// <returns>deserialized type</returns>
        public object Dersialize(string json, Type type)
        {
            int indexOf = 0;
            DeSerializedObjects.Clear();
            IJsonGoModel jsonModel = Extract(ref json, ref indexOf);
            return jsonModel.Generate(type, this);
        }

        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="json">json value</param>
        /// <param name="type">type to deserialize</param>
        /// <param name="indexOf">index of start string</param>
        /// <returns>value deserialized</returns>
        internal IJsonGoModel Extract(ref string json, ref int indexOf)
        {
            IJsonGoModel jsonGoModel = null;
            bool canSkip = true;
            StringBuilder keyBuilder = new StringBuilder();
            StringBuilder valueBuilder = new StringBuilder();
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
                    foreach (KeyValuePair<string, IJsonGoModel> item in ExtractOject(ref json, ref indexOf))
                    {
                        jsonGoModel.Add(item.Key, item.Value);
                    }

                    return jsonGoModel;
                }
                else if (character == '[')
                {
                    jsonGoModel = new ArrayModel();
                    indexOf++;
                    foreach (IJsonGoModel item in ExtractArray(ref json, ref indexOf))
                    {
                        jsonGoModel.Add(null, item);
                    }
                    return jsonGoModel;
                }
                else if (character == '\"')
                {
                    jsonGoModel = new ValueModel(ExtractString(ref json, ref indexOf));
                    return jsonGoModel;
                }
                else
                {
                    jsonGoModel = new ValueModel(ExtractValue(ref json, ref indexOf));
                    return jsonGoModel;
                }

            }
            return jsonGoModel;
        }

        internal List<IJsonGoModel> ExtractArray(ref string json, ref int indexOf)
        {
            List<IJsonGoModel> result = new List<IJsonGoModel>();
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
                    foreach (KeyValuePair<string, IJsonGoModel> item in ExtractOject(ref json, ref indexOf))
                    {
                        jsonGoModel.Add(item.Key, item.Value);
                    }
                    i = indexOf;
                    result.Add(jsonGoModel);
                }
                else if (character == '[')
                {
                    ArrayModel jsonGoModel = new ArrayModel();
                    indexOf++;
                    foreach (IJsonGoModel item in ExtractArray(ref json, ref indexOf))
                    {
                        jsonGoModel.Add(null, item);
                    }
                    i = indexOf;
                    result.Add(jsonGoModel);
                }
                else if (character == ',')
                {
                    continue;
                }
                else if (character == ']')
                {
                    break;
                }
                else
                    throw new Exception($"end of character not support '{character}' index of {i} i think i must find '}}' character");

            }
            return result;
        }

        /// <summary>
        /// extract list of properties from object
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        internal Dictionary<string, IJsonGoModel> ExtractOject(ref string json, ref int indexOf)
        {
            Dictionary<string, IJsonGoModel> result = new Dictionary<string, IJsonGoModel>();
            bool canSkip = true;
            bool findKey = false;
            bool findStartOfValue = false;
            char previousCharacter = default(char);
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
                        IJsonGoModel value = Extract(ref json, ref indexOf);
                        result.Add(keyBuilder.ToString().Trim('\"'), value);
                        keyBuilder.Clear();
                        i = indexOf;
                        canSkip = true;
                        findKey = false;
                        findStartOfValue = false;
                        previousCharacter = default(char);
                    }
                    else
                        throw new Exception($"I tried to find start of value, I think here must be ':' char instead of '{character}' char index of {i} from json {json}");
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
                    throw new Exception($"I tried to find start of object, I think here must be '{{' char instead of '{character}' char index of {i} from json {json}");

                previousCharacter = character;
            }
            return result;
        }

        /// <summary>
        /// get type of a json parameter name
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="key">json parameter name</param>
        /// <returns>type of json parameter</returns>
        internal Type GetKeyType(object obj, string key)
        {
            if (obj == null)
                return null;
            key = key.Trim('\"');
            Type type = obj.GetType();
            PropertyInfo propertyInfo = FindCachedMember<PropertyInfo>(type, key);
            if (propertyInfo == null)
            {
                FieldInfo fieldInfo = FindCachedMember<FieldInfo>(type, key);
                if (fieldInfo == null)
                    return null;
                return fieldInfo.FieldType;
            }
            else
            {
                return propertyInfo.PropertyType;
            }
        }
        /// <summary>
        /// extract string from inside of double " char
        /// </summary>
        /// <param name="json"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        internal string ExtractString(ref string json, ref int indexOf)
        {
            StringBuilder result = new StringBuilder();
            bool started = false;
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                result.Append(character);
                if (character == '\"')
                {
                    if (!started)
                        started = true;
                    else
                        break;
                }
            }

            return result.ToString();
        }

        internal string ExtractValue(ref string json, ref int indexOf)
        {
            StringBuilder result = new StringBuilder();
            bool started = false;
            for (int i = indexOf; i < json.Length; i++)
            {
                char character = json[i];
                if (SupportedValue.Contains(character))
                {
                    if (!started)
                        started = true;
                    result.Append(character);
                    indexOf = i;
                }
                else
                    break;
            }

            return result.ToString();
        }
        /// <summary>
        /// set value of json parameter key to an instance of object
        /// </summary>
        /// <param name="obj">object to change parameter</param>
        /// <param name="value">value to set</param>
        /// <param name="key">parameter name of object</param>
        internal void SetValue(object obj, object value, string key)
        {
            if (obj == null)
                return;
            key = key.Trim('\"');
            Type type = obj.GetType();
            PropertyInfo propertyInfo = FindCachedMember<PropertyInfo>(type, key);
            if (propertyInfo == null)
            {
                FieldInfo fieldInfo = FindCachedMember<FieldInfo>(type, key);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(obj, value);
                }
            }
            else
            {
                propertyInfo.SetValue(obj, value);
            }
        }

        internal object GetValue(Type type, object value)
        {
            if (value == null)
                return null;
            if (type.IsEnum)
            {
                value = Convert.ChangeType(value, typeof(int));
                value = Enum.ToObject(type, (int)value);
            }
            else
                value = Convert.ChangeType(value, type);
            return value;
        }

        /// <summary>
        /// check if a character is whitespace or empty
        /// </summary>
        /// <param name="value">character to check</param>
        /// <returns>is char is white space</returns>
        private bool IsWhiteSpace(ref char value)
        {
            return value == '\b' || value == '\f' || value == '\n' || value == '\r' || value == '\t' || value == ' ';
        }
        /// <summary>
        /// find parameter or metod or field name from cached
        /// </summary>
        /// <typeparam name="T">type of memberinfo like method,field,property</typeparam>
        /// <param name="type">type of object to research</param>
        /// <param name="name">name of parameter</param>
        /// <returns>member like method,field,property thet found</returns>
        internal T FindCachedMember<T>(Type type, string name) where T : class
        {
            Type tType = typeof(T);
            bool exist = CacheNameVariables.TryGetValue(type, out ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>> members);
            if (!exist)
            {
                members = new ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>();
                CacheNameVariables.TryAdd(type, members);
            }

            exist = members.TryGetValue(tType, out ConcurrentDictionary<string, MemberInfo> values);

            if (!exist)
            {
                values = new ConcurrentDictionary<string, MemberInfo>();
                members.TryAdd(tType, values);
            }
            name = name.ToLower();
            if (values.TryGetValue(name, out MemberInfo value))
                return value as T;
            else
            {
                if (tType == typeof(MethodInfo))
                {
                    MethodInfo find = type.GetMethods().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (find != null)
                    {
                        values.TryAdd(name, find);
                        return find as T;
                    }
                    else
                        return null;
                }
                else if (tType == typeof(PropertyInfo))
                {
                    PropertyInfo find = type.GetProperties().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (find != null)
                    {
                        values.TryAdd(name, find);
                        return find as T;
                    }
                    else
                        return null;
                }
                else if (tType == typeof(FieldInfo))
                {
                    FieldInfo find = type.GetFields().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (find != null)
                    {
                        values.TryAdd(name, find);
                        return find as T;
                    }
                    else
                        return null;
                }
            }
            return null;
        }
    }
}
