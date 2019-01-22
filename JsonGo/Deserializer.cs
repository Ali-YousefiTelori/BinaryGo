using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo
{
    /// <summary>
    /// json type of all values
    /// </summary>
    public enum JsonType : byte
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,
        /// <summary>
        /// object value
        /// </summary>
        Object = 1,
        /// <summary>
        /// array value
        /// </summary>
        Array = 2,
        /// <summary>
        /// strin or int simple value
        /// </summary>
        Value = 3
    }

    public class Deserializer
    {
        static Deserializer()
        {
            SingleIntance = new Deserializer();
        }

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
            return (T)Desialize(ref json, typeof(T), ref indexOf);
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
            return Desialize(ref json, type, ref indexOf);
        }

        /// <summary>
        /// deserialize json
        /// </summary>
        /// <param name="json">json value</param>
        /// <param name="type">type to deserialize</param>
        /// <param name="indexOf">index of start string</param>
        /// <returns>value deserialized</returns>
        internal object Desialize(ref string json, Type type, ref int indexOf)
        {
            object currentObject = null;
            JsonType objectType = JsonType.None;
            bool canSkip = true;
            bool findingStartOfKey = false;
            bool findingKey = true;
            bool findingStartOfValue = false;
            bool findingValue = false;
            bool hasValuesReferenced = false;
            char previousCharacter = default(char);
            StringBuilder keyBuilder = new StringBuilder();
            StringBuilder valueBuilder = new StringBuilder();
            for (int i = indexOf; i < json.Length; i++)
            {
                indexOf = i;
                char character = json[i];
                if (canSkip && IsWhiteSpace(ref character))
                    continue;
                if (character == '{' && objectType == JsonType.None)
                    objectType = JsonType.Object;
                else if (character == '[' && objectType == JsonType.None)
                    objectType = JsonType.Array;
                else if (canSkip && objectType == JsonType.None && (character == ']' || character == ']'))
                    break;
                else if (canSkip && objectType == JsonType.None)
                    throw new Exception($"unexpected character '{character}' index {i}");
                else
                {
                    canSkip = false;
                    if (findingStartOfKey)
                    {
                        if (character == ',')
                        {
                            findingKey = true;
                            findingStartOfKey = false;
                        }
                        else if ((character == '}' || character == ']') && !hasValuesReferenced)
                            break;
                        else if ((character == '}' || character == ']') && hasValuesReferenced)
                            hasValuesReferenced = false;
                    }
                    else if (findingKey)
                    {
                        if (character == '\"' && keyBuilder.Length > 0)
                        {
                            findingKey = false;
                            findingStartOfValue = true;
                        }
                        else if (objectType == JsonType.Array && (character == '{' || character == '['))
                        {
                            string key = keyBuilder.ToString();
                            keyBuilder.Clear();
                            valueBuilder.Clear();
                            findingValue = false;
                            findingStartOfKey = true;
                            canSkip = true;
                            MethodInfo addMethod = FindCachedMember<MethodInfo>(type, "Add");
                            Array array = null;
                            Type elementType = null;
                            if (key == Setting.RefRefrencedTypeName)
                            {
                                throw new NotSupportedException(json);
                            }
                            else if (key == Setting.ValuesRefrencedTypeName)
                            {
                                hasValuesReferenced = true;
                                objectType = JsonType.Array;
                            }
                            else if (key == Setting.IdRefrencedTypeName)
                            {
                                throw new NotSupportedException(json);
                            }
                            if (currentObject == null)
                                currentObject = Activator.CreateInstance(type);
                            if (type.IsArray)
                            {
                                array = (Array)currentObject;
                                elementType = array.GetType().GetElementType();
                            }
                            else if (addMethod != null)
                            {
                                elementType = addMethod.GetParameters().FirstOrDefault().ParameterType;
                            }
                            if (elementType != null)
                            {
                                object value = Desialize(ref json, elementType, ref indexOf);
                                addMethod.Invoke(currentObject, new object[] { value });
                                i = indexOf;
                            }
                        }
                        keyBuilder.Append(character);
                    }
                    else if (findingStartOfValue)
                    {
                        if (character == '\"')
                        {
                            findingValue = true;
                            findingStartOfValue = false;
                            valueBuilder.Append(character);
                        }
                        else if (character == '{' || character == '[')
                        {
                            string key = keyBuilder.ToString();
                            keyBuilder.Clear();
                            valueBuilder.Clear();
                            findingValue = false;
                            findingStartOfKey = true;
                            canSkip = true;
                            Type typeOfParameter = null;
                            if (key == Setting.RefRefrencedTypeName)
                            {
                                throw new NotSupportedException(json);
                            }
                            else if (key == Setting.ValuesRefrencedTypeName)
                            {
                                hasValuesReferenced = true;
                                objectType = JsonType.Array;
                            }
                            else if (key == Setting.IdRefrencedTypeName)
                            {
                                throw new NotSupportedException(json);
                            }
                            if (currentObject == null)
                                currentObject = Activator.CreateInstance(type);
                            if (currentObject is IEnumerable ilist)
                            {
                                MethodInfo addMethod = FindCachedMember<MethodInfo>(type, "Add");
                                if (type.IsArray)
                                {
                                    Array array = (Array)currentObject;
                                    typeOfParameter = array.GetType().GetElementType();
                                }
                                else
                                {
                                    if (addMethod == null)
                                        throw new Exception($"Add method not found on type {type.FullName}");
                                    typeOfParameter = addMethod.GetParameters().FirstOrDefault().ParameterType;
                                }
                                if (typeOfParameter != null)
                                {
                                    indexOf++;
                                    object value = Desialize(ref json, typeOfParameter, ref indexOf);
                                    if (value != null)
                                        addMethod.Invoke(currentObject, new object[] { value });
                                    i = indexOf;
                                }
                            }
                            else
                            {
                                typeOfParameter = GetKeyType(currentObject, key);
                                if (typeOfParameter != null)
                                {
                                    object value = Desialize(ref json, typeOfParameter, ref indexOf);
                                    SetValue(currentObject, value, key);
                                    i = indexOf;
                                }
                            }
                        }
                    }
                    else if (findingValue)
                    {
                        valueBuilder.Append(character);
                        if (character == '\"' && previousCharacter != '\\')
                        {
                            findingValue = false;
                            findingStartOfKey = true;
                            canSkip = true;

                            string key = keyBuilder.ToString();
                            string value = valueBuilder.ToString().Trim('\"');
                            keyBuilder.Clear();
                            valueBuilder.Clear();
                            if (key == Setting.RefRefrencedTypeName)
                            {
                                var findIndex = json.IndexOf('}', indexOf);
                                if (findIndex < 0)
                                    throw new Exception("index not found is json valid?");
                                indexOf = findIndex;
                                return DeSerializedObjects[value];
                            }
                            else
                            {
                                if (currentObject == null)
                                    currentObject = Activator.CreateInstance(type);
                                if (key == Setting.IdRefrencedTypeName)
                                {
                                    DeSerializedObjects.Add(value, currentObject);
                                }
                                else
                                    SetValue(currentObject, value, key);
                            }
                        }

                    }
                    else
                        throw new Exception("hell happens");
                }
                previousCharacter = character;
            }
            return currentObject;
        }

        /// <summary>
        /// get type of a json parameter name
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="key">json parameter name</param>
        /// <returns>type of json parameter</returns>
        private Type GetKeyType(object obj, string key)
        {
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
        /// set value of json parameter key to an instance of object
        /// </summary>
        /// <param name="obj">object to change parameter</param>
        /// <param name="value">value to set</param>
        /// <param name="key">parameter name of object</param>
        private void SetValue(object obj, object value, string key)
        {
            key = key.Trim('\"');
            Type type = obj.GetType();
            PropertyInfo propertyInfo = FindCachedMember<PropertyInfo>(type, key);
            if (propertyInfo == null)
            {
                FieldInfo fieldInfo = FindCachedMember<FieldInfo>(type, key);
                if (fieldInfo != null)
                {
                    if (fieldInfo.FieldType.IsEnum)
                    {
                        value = Convert.ChangeType(value, typeof(int));
                        value = Enum.ToObject(fieldInfo.FieldType, (int)value);
                    }
                    else
                        value = Convert.ChangeType(value, fieldInfo.FieldType);
                    fieldInfo.SetValue(obj, value);
                }
            }
            else
            {
                if (propertyInfo.PropertyType.IsEnum)
                {
                    value = Convert.ChangeType(value, typeof(int));
                    value = Enum.ToObject(propertyInfo.PropertyType, (int)value);
                }
                else
                    value = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, value);
            }

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
        private T FindCachedMember<T>(Type type, string name) where T : class
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
