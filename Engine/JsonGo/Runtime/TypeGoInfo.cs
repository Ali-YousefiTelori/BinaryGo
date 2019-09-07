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
        static TypeGoInfo()
        {
            AddToCustomTypes<ICollection, Array>();
            AddToCustomTypes<IEnumerable, Array>();
            AddToCustomTypes(typeof(IEnumerable<>), typeof(List<>));
            AddToCustomTypes(typeof(ICollection<>), typeof(List<>));
        }
        /// <summary>
        /// default value of object
        /// </summary>
        public object DefaultValue { get; set; }
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
        /// array of all properties for serialize
        /// </summary>
        public PropertyGoInfo[] SerializeProperties { get; set; }
        /// <summary>
        /// array of all properties for deserialize
        /// </summary>
        public PropertyGoInfo[] DeserializeProperties { get; set; }
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
            var baseType = Nullable.GetUnderlyingType(type);
            bool isNullable = false;
            if (baseType != null)
            {
                isNullable = true;
            }
            else
                baseType = type;
            TypeGoInfo typeGoInfo = new TypeGoInfo
            {
                Properties = new Dictionary<string, PropertyGoInfo>(),
                Type = type,
            };
            Types[type] = typeGoInfo;
            if (baseType == typeof(DateTime))
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
                typeGoInfo.DefaultValue = default(DateTime);
            }
            else if (baseType == typeof(uint))
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
                typeGoInfo.DefaultValue = default(uint);
            }
            else if (baseType == typeof(long))
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
                typeGoInfo.DefaultValue = default(long);
            }
            else if (baseType == typeof(short))
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
                typeGoInfo.DefaultValue = default(short);
            }
            else if (baseType == typeof(byte))
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
                typeGoInfo.DefaultValue = default(byte);
            }
            else if (baseType == typeof(double))
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
                typeGoInfo.DefaultValue = default(double);
            }
            else if (baseType == typeof(float))
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
                typeGoInfo.DefaultValue = default(float);
            }
            else if (baseType == typeof(decimal))
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
                typeGoInfo.DefaultValue = default(decimal);
            }
            else if (baseType == typeof(sbyte))
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
                typeGoInfo.DefaultValue = default(sbyte);
            }
            else if (baseType == typeof(ulong))
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
                typeGoInfo.DefaultValue = default(ulong);
            }
            else if (baseType == typeof(bool))
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
                typeGoInfo.DefaultValue = default(bool);
            }
            else if (baseType == typeof(ushort))
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
                typeGoInfo.DefaultValue = default(ushort);
            }
            else if (baseType == typeof(int))
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
                typeGoInfo.DefaultValue = default(int);
            }
            else if (baseType == typeof(byte[]))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(Convert.ToBase64String((byte[])data));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return Convert.FromBase64String(TextHelper.SpanToString(x));
                };
                typeGoInfo.DefaultValue = default(byte[]);
            }
            else if (baseType == typeof(string))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append(((string)data).Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return TextHelper.SpanToString(x);
                };
                typeGoInfo.DefaultValue = default(string);
            }
            else if (baseType.IsEnum)
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(Convert.ToInt32(data));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (Utf8Parser.TryParse(x, out int value, out int bytes))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };
                typeGoInfo.DefaultValue = Activator.CreateInstance(baseType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(baseType))
            {
                baseType = GenerateTypeFromInterface(baseType);
                typeGoInfo.IsNoQuotesValueType = false;

                //add $Id dproperties
                typeGoInfo.Properties[JsonConstants.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = Generate(typeof(int)),
                    Type = typeof(int),
                    Name = JsonConstants.IdRefrencedTypeNameNoQuotes,
                    SetValue = (serializer, instance, value) =>
                    {
                        serializer.DeSerializedObjects.Add((int)value, instance);
                    },
                    GetValue = (serializer, data) =>
                    {
                        if (!serializer.SerializedObjects.TryGetValue(data, out int refrencedId))
                        {
                            serializer.ReferencedIndex++;
                            serializer.SerializedObjects.Add(data, serializer.ReferencedIndex);
                            return serializer.ReferencedIndex;
                        }
                        else
                        {
                            return refrencedId;
                        }
                    }
                };
                typeGoInfo.Properties[JsonConstants.ValuesRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = Generate(type),
                    Type = type,
                    Name = JsonConstants.ValuesRefrencedTypeNameNoQuotes,
                    SetValue = (serializer, instance, value) =>
                    {
                        if (Generate(instance.GetType()) is TypeGoInfo typeGo)
                        {
                            foreach (var item in (IEnumerable)value)
                            {
                                typeGo.AddArrayValue(instance, item);
                            }
                        }
                    },
                    GetValue = (serializer, data) =>
                    {
                        if (data == null)
                            return null;
                        var builder = serializer.Writer;
                        builder.Append(JsonSettingInfo.Quotes);
                        builder.Append(JsonConstants.ValuesRefrencedTypeNameNoQuotes);
                        builder.Append(JsonSettingInfo.QuotesColon);
                        builder.Append(JsonSettingInfo.OpenSquareBrackets);
                        var generic = typeGoInfo.Generics[0];
                        foreach (var item in (IEnumerable)data)
                        {
                            var obj = item;
                            generic.Serialize(serializer, builder, ref obj);
                            builder.Append(JsonConstants.Comma);
                        }
                        serializer.RemoveLastCama();
                        builder.Append(JsonSettingInfo.CloseSquareBrackets);
                        return null;
                    }
                };
                foreach (var item in baseType.GetGenericArguments())
                {
                    if (!Types.TryGetValue(item, out TypeGoInfo typeGoInfoProperty))
                        Types[item] = typeGoInfoProperty = Generate(item);
                    typeGoInfo.Generics.Add(typeGoInfoProperty);
                }

                if (baseType.IsArray)
                {
                    List<int> items = new List<int>();
                    items.ToArray();
                    var elementType = baseType.GetElementType();
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
                    typeGoInfo.CreateInstance = () => Activator.CreateInstance(baseType);
                    var method = baseType.GetMethod("Add");
                    typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                }
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    serializer.SerializeFunction(typeGoInfo, serializer, builder, ref data);
                };
                typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.GetValue != null).ToArray();
                typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.SetValue != null).ToArray();
                typeGoInfo.DefaultValue = null;
            }
            else
            {
                baseType = GenerateTypeFromInterface(baseType);
                typeGoInfo.IsNoQuotesValueType = false;
                //add $Id dproperties
                typeGoInfo.Properties[JsonConstants.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = Generate(typeof(int)),
                    Type = typeof(int),
                    Name = JsonConstants.IdRefrencedTypeNameNoQuotes,
                    SetValue = (serializer, instance, value) =>
                    {
                        serializer.DeSerializedObjects.Add((int)value, instance);
                    },
                    GetValue = (serializer, data) =>
                    {
                        if (!serializer.SerializedObjects.TryGetValue(data, out int refrencedId))
                        {
                            serializer.ReferencedIndex++;
                            serializer.SerializedObjects.Add(data, serializer.ReferencedIndex);
                            return serializer.ReferencedIndex;
                        }
                        else
                        {
                            return refrencedId;
                        }
                    }
                };
                foreach (var property in baseType.GetProperties())
                {
                    if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0 || !property.CanRead || !property.CanWrite || property.GetIndexParameters().Length > 0)
                        continue;
                    IPropertyCallerInfo del = null;
                    try
                    {
                        del = GetDelegateInstance(baseType, property);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Cannot create delegate for property {property.Name} in type {type.FullName}", ex);
                    }
                    if (!Types.TryGetValue(property.PropertyType, out TypeGoInfo typeGoInfoProperty))
                        Types[property.PropertyType] = typeGoInfoProperty = Generate(property.PropertyType);
                    typeGoInfo.Properties[property.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = property.PropertyType,
                        Name = property.Name,
                        GetValue = (serializer, x) => del.GetPropertyValue(x),
                        SetValue = del.SetPropertyValue,
                        //SetValue = (x, val) => property.SetValue(x, val),
                    };
                }



                foreach (var item in baseType.GetFields())
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
                typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.GetValue != null).ToArray();
                typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.SetValue != null).ToArray();
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    if (serializer.SerializedObjects.TryGetValue(data, out int refrencedId))
                    {
                        builder.Append(JsonConstants.OpenBraket);
                        builder.Append(JsonConstants.RefRefrencedTypeName);
                        builder.Append(JsonSettingInfo.Colon);
                        builder.Append(refrencedId);
                        builder.Append(JsonConstants.CloseBracket);
                    }
                    else
                        serializer.SerializeFunction(typeGoInfo, serializer, builder, ref data);
                };
                typeGoInfo.CreateInstance = () => Activator.CreateInstance(baseType);
                typeGoInfo.DefaultValue = null;
            }
            if (isNullable)
                typeGoInfo.DefaultValue = null;
            return typeGoInfo;
        }

        internal static Dictionary<Type, Type> CustomTypeChanges { get; set; } = new Dictionary<Type, Type>();
        /// <summary>
        /// generate interface ot types to new types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GenerateTypeFromInterface(Type type)
        {
            Type[] genericTypes = null;
            if (type.GenericTypeArguments.Length > 0)
            {
                genericTypes = type.GenericTypeArguments;
                type = type.GetGenericTypeDefinition();
            }

            if (CustomTypeChanges.TryGetValue(type, out Type newType))
            {
                type = newType;
            }

            if (genericTypes != null)
            {
                for (int i = 0; i < genericTypes.Length; i++)
                {
                    genericTypes[i] = GenerateTypeFromInterface(genericTypes[i]);
                }
                type = type.MakeGenericType(genericTypes);
            }
            return type;
        }

        /// <summary>
        /// add your types or interfaces to automatic custom type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static void AddToCustomTypes<TType, TResult>()
        {
            AddToCustomTypes(typeof(TType), typeof(TResult));
        }
        /// <summary>
        /// add your types or interfaces to automatic custom type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static void AddToCustomTypes(Type type, Type result)
        {
            CustomTypeChanges[type] = result;
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
