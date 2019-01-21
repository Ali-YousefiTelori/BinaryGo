using System;
using System.Collections.Concurrent;
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

        internal static ConcurrentDictionary<Type, ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>> CacheNameVariables { get; set; } = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>>();

        public static Deserializer SingleIntance { get; set; }

        public T Dersialize<T>(string json)
        {
            int indexOf = 0;
            return (T)Desialize(ref json, typeof(T), ref indexOf);
        }

        public object Dersialize(string json, Type type)
        {
            int indexOf = 0;
            return Desialize(ref json, type, ref indexOf);
        }

        internal object Desialize(ref string json, Type type, ref int indexOf)
        {
            object currentObject = Activator.CreateInstance(type);
            JsonType objectType = JsonType.None;
            bool canSkip = true;
            bool findingStartOfKey = false;
            bool findingKey = true;
            bool findingStartOfValue = false;
            bool findingValue = false;
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
                        else if (character == '}' || character == ']')
                            break;
                    }
                    else if (findingKey)
                    {
                        if (character == '\"' && keyBuilder.Length > 0)
                        {
                            findingKey = false;
                            findingStartOfValue = true;
                        }
                        else if (objectType == JsonType.Array && character == '{')
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
                            if (type.IsArray)
                            {
                                array = (Array)currentObject;
                                elementType = array.GetType().GetElementType();
                            }
                            else
                            {
                                if (addMethod == null)
                                    throw new Exception($"Add method not found on type {type.FullName}");
                                elementType = addMethod.GetParameters().FirstOrDefault().ParameterType;
                            }

                            object value = Desialize(ref json, elementType, ref indexOf);
                            addMethod.Invoke(currentObject, new object[] { value });
                            i = indexOf;
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
                            object value = Desialize(ref json, GetKeyType(currentObject, key), ref indexOf);
                            SetValue(currentObject, value, key);
                            i = indexOf;
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

                            SetValue(currentObject, value, key);
                        }

                    }
                    else
                        throw new Exception("hell happens");
                }
                previousCharacter = character;
            }
            return currentObject;
        }

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

        private void SetValue(object obj, object value, string key)
        {
            key = key.Trim('\"');
            Type type = obj.GetType();
            PropertyInfo propertyInfo = FindCachedMember<PropertyInfo>(type, key);
            if (propertyInfo == null)
            {
                FieldInfo fieldInfo = FindCachedMember<FieldInfo>(type, key);
                if (fieldInfo.FieldType.IsEnum)
                {
                    value = Convert.ChangeType(value, typeof(int));
                    value = Enum.ToObject(fieldInfo.FieldType, (int)value);
                }
                else
                    value = Convert.ChangeType(value, fieldInfo.FieldType);
                fieldInfo.SetValue(obj, value);
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

        private bool IsWhiteSpace(ref char value)
        {
            return value == '\b' || value == '\f' || value == '\n' || value == '\r' || value == '\t' || value == ' ';
        }

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
