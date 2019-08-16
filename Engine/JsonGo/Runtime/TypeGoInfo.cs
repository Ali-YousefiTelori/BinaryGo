using JsonGo.DataTypes;
using JsonGo.Deserialize;
using JsonGo.Helpers;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="stringBuilder">builder to add newdata</param>
    /// <param name="data">any object to serialize</param>
    /// <param name="serializer">json serializer</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void FunctionGo(Serializer serializer, StringBuilder stringBuilder, ref object data);
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="stringBuilder">builder to add newdata</param>
    /// <param name="data">any object to serialize</param>
    /// <param name="typeGoInfo">typego of jsongo</param>
    /// <param name="serializer">json serializer</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void FunctionTypeGo(TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object data);
    public delegate object DeserializeFunc(Deserializer deserializer, ReadOnlySpan<byte> data);

    /// <summary>
    /// generate type details on memory
    /// </summary>
    public class TypeGoInfo
    {
        /// <summary>
        /// if the type is simple like int,bye,bool,enum they can serialize without quots
        /// </summary>
        public bool IsNoQuotesValueType { get; set; } = true;
        /// <summary>
        /// chached types
        /// </summary>
        internal static Dictionary<Type, TypeGoInfo> Types = new Dictionary<Type, TypeGoInfo>();
        /// <summary>
        /// type of data
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// serialize action
        /// </summary>
        public FunctionGo Serialize { get; set; }
        /// <summary>
        /// deserialize string to object
        /// </summary>
        public DeserializeFunc Deserialize { get; set; }
        /// <summary>
        /// create instance of type
        /// </summary>
        public Func<object> CreateInstance { get; set; }
        /// <summary>
        /// cast to real object
        /// </summary>
        public Func<object, object> Cast { get; set; }
        /// <summary>
        /// add array value to array type go
        /// </summary>
        public Action<object, object> AddArrayValue { get; set; }

        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> Properties { get; set; }
        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> DirectProperties { get; set; }
        /// <summary>
        /// array of all properties
        /// </summary>
        public PropertyGoInfo[] ArrayProperties { get; set; }
        /// <summary>
        /// generic types
        /// </summary>
        public List<TypeGoInfo> Generics { get; set; } = new List<TypeGoInfo>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeGoInfo Generate(Type type)
        {
            if (Types.TryGetValue(type, out TypeGoInfo find))
                return find;
            TypeGoInfo typeGoInfo = new TypeGoInfo
            {
                Properties = new Dictionary<string, PropertyGoInfo>(),
                Type = type
            };
            Types[type] = typeGoInfo;
            if (type == typeof(DateTime))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((DateTime)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    var text = Encoding.UTF8.GetString(x.ToArray());
                    if (DateTime.TryParse(text, out DateTime value))
                        return value;
                    return default(DateTime);
                };
            }
            else if (type == typeof(uint))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((uint)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out uint value, out int bytes))
                        return value;
                    return default(uint);
                };
            }
            else if (type == typeof(long))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((long)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out long value, out int bytes))
                        return value;
                    return default(long);
                };
            }
            else if (type == typeof(short))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((short)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out short value, out int bytes))
                        return value;
                    return default(short);
                };
            }
            else if (type == typeof(byte))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((byte)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out byte value, out int bytes))
                        return value;
                    return default(byte);
                };
            }
            else if (type == typeof(double))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((double)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out double value, out int bytes))
                        return value;
                    return default(double);
                };
            }
            else if (type == typeof(float))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((float)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out float value, out int bytes))
                        return value;
                    return default(float);
                };
            }
            else if (type == typeof(decimal))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((decimal)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out decimal value, out int bytes))
                        return value;
                    return default(decimal);
                };
            }
            else if (type == typeof(sbyte))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((sbyte)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out sbyte value, out int bytes))
                        return value;
                    return default(sbyte);
                };
            }
            else if (type == typeof(ulong))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((ulong)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out ulong value, out int bytes))
                        return value;
                    return default(ulong);
                };
            }
            else if (type == typeof(bool))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((bool)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out bool value, out int bytes))
                        return value;
                    return default(bool);
                };
            }
            else if (type == typeof(ushort))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((ushort)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out ushort value, out int bytes))
                        return value;
                    return default(ushort);
                };
            }
            else if (type == typeof(int))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append((int)data);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out int value, out int bytes))
                        return value;
                    return default(int);
                };
            }
            else if (type == typeof(byte[]))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(Convert.ToBase64String((byte[])data));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return Convert.FromBase64String(TextHelper.SpanToString(x));
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append(((string)data).Replace("\"", "\\\""));
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return TextHelper.SpanToString(x);
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(Convert.ToInt32(data));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out int value, out int bytes))
                        return Enum.ToObject(type, value);
                    return Enum.ToObject(type, 0);
                };

            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                foreach (var item in type.GetGenericArguments())
                {
                    if (!Types.TryGetValue(item, out TypeGoInfo typeGoInfoProperty))
                        Types[item] = typeGoInfoProperty = Generate(item);
                    typeGoInfo.Generics.Add(typeGoInfoProperty);
                }

                //add $Id dproperties
                typeGoInfo.Properties[JsonConstants.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = Generate(typeof(int)),
                    Type = typeof(int),
                    Name = JsonConstants.IdRefrencedTypeNameNoQuotes,
                    SetValue = (serializer, instance, value) =>
                    {
                        serializer.DeSerializedObjects.Add((int)value, instance);
                    }
                };

                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    serializer.SerializeArrayFunction(typeGoInfo, serializer, stringBuilder, ref data);
                };
                if (type.IsArray)
                {
                    List<int> items = new List<int>();
                    items.ToArray();
                    var elementType = type.GetElementType();
                    var newType = typeof(List<>).MakeGenericType(elementType);
                    if (!Types.TryGetValue(elementType, out TypeGoInfo typeGoInfoProperty))
                        Types[elementType] = typeGoInfoProperty = Generate(elementType);
                    typeGoInfo.Generics.Add(typeGoInfoProperty);
                    typeGoInfo.CreateInstance = () => Activator.CreateInstance(newType);
                    var castMethod = typeof(TypeGoInfo).GetMethod("GetArray", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    typeGoInfo.Cast = (obj) => castMethod.Invoke(null, new object[] { obj });
                    var method = newType.GetMethod("Add");
                    typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                }
                else
                {
                    typeGoInfo.CreateInstance = () => Activator.CreateInstance(type);
                    var method = type.GetMethod("Add");
                    typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                }
            }
            else
            {
                typeGoInfo.IsNoQuotesValueType = false;
                foreach (var property in type.GetProperties())
                {
                    if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        continue;
                    var del = GetDelegateInstance(type, property);
                    if (!Types.TryGetValue(property.PropertyType, out TypeGoInfo typeGoInfoProperty))
                        Types[property.PropertyType] = typeGoInfoProperty = Generate(property.PropertyType);
                    typeGoInfo.Properties[property.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = property.PropertyType,
                        Name = property.Name,
                        GetValue = x => del.GetPropertyValue(x),
                        SetValue = del.SetPropertyValue,
                        //SetValue = (x, val) => property.SetValue(x, val),
                    };
                }
                //add $Id dproperties
                typeGoInfo.Properties[JsonConstants.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = Generate(typeof(int)),
                    Type = typeof(int),
                    Name = JsonConstants.IdRefrencedTypeNameNoQuotes,
                    SetValue = (serializer, instance, value) =>
                    {
                        serializer.DeSerializedObjects.Add((int)value, instance);
                    }
                };


                foreach (var item in type.GetFields())
                {
                    if (item.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        continue;

                    if (!Types.TryGetValue(item.FieldType, out TypeGoInfo typeGoInfoProperty))
                        Types[item.FieldType] = typeGoInfoProperty = Generate(item.FieldType);
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = item.FieldType,
                        Name = item.Name,
                        //GetValue = item.GetValue,
                        //SetValue = item.SetValue
                    };
                }
                typeGoInfo.ArrayProperties = typeGoInfo.Properties.Values.ToArray();
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    serializer.SerializeFunction(typeGoInfo, serializer, builder, ref data);
                };
                typeGoInfo.CreateInstance = () => Activator.CreateInstance(type);
            }

            return typeGoInfo;
        }
        public static IPropertyCallerInfo GetDelegateInstance(Type type, PropertyInfo propertyInfo)
        {
            var openGetterType = typeof(Func<,>);
            var concreteGetterType = openGetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            var openSetterType = typeof(Action<,>);
            var concreteSetterType = openSetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            Delegate getterInvocation = Delegate.CreateDelegate(concreteGetterType, null, propertyInfo.GetGetMethod());
            Delegate setterInvocation = Delegate.CreateDelegate(concreteSetterType, null, propertyInfo.GetSetMethod());

            var callerType = typeof(PropertyCallerInfo<,>);
            var callerGenericType = callerType
                .MakeGenericType(type, propertyInfo.PropertyType);

            return (IPropertyCallerInfo)Activator.CreateInstance(callerGenericType, getterInvocation, setterInvocation);
        }

        static T[] GetArray<T>(IEnumerable<T> iList)
        {
            var result = new T[iList.Count()];

            Array.Copy(iList.ToArray(), 0, result, 0, result.Length);
            return result;
        }
    }

    public interface IPropertyCallerInfo
    {
        object GetPropertyValue(object instance);
        void SetPropertyValue(Deserializer deserializer, object instance, object value);
    }
    public class PropertyCallerInfo<TType, TPropertyType> : IPropertyCallerInfo
    {
        public PropertyCallerInfo(Func<TType, TPropertyType> funcGetValue, Action<TType, TPropertyType> funcSetValue)
        {
            GetValue = funcGetValue;
            SetValue = funcSetValue;
        }
        public Func<TType, TPropertyType> GetValue { get; set; }
        public Action<TType, TPropertyType> SetValue { get; set; }

        public object GetPropertyValue(object instance)
        {
            return GetValue((TType)instance);
        }

        public void SetPropertyValue(Deserializer deserializer, object instance, object value)
        {
            SetValue((TType)instance, (TPropertyType)value);
        }
    }
}
