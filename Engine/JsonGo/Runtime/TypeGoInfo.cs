using JsonGo.DataTypes;
using System;
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
    /// <summary>
    /// generate type details on memory
    /// </summary>
    public class TypeGoInfo
    {
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
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> Properties { get; set; }
        /// <summary>
        /// array of all properties
        /// </summary>
        public PropertyGoInfo[] ArrayProperties { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeGoInfo Generate(Type type)
        {
            TypeGoInfo typeGoInfo = new TypeGoInfo
            {
                Properties = new Dictionary<string, PropertyGoInfo>()
            };
            if (type == typeof(DateTime))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((DateTime)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(uint))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((uint)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(long))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((long)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(short))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((short)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(byte))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((byte)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(double))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((double)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(float))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((float)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(decimal))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((decimal)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(sbyte))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((sbyte)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(ulong))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((ulong)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(bool))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((bool)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(ushort))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((ushort)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(int))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append((int)data);
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append(((string)data).Replace("\"", "\\\""));
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append(Convert.ToInt32(data));
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder stringBuilder, ref object data) =>
                {
                    serializer.SerializeArrayFunction(typeGoInfo, serializer, stringBuilder, ref data);
                };
            }
            else
            {
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
                        SetValue = property.SetValue
                    };
                }
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
                        SetValue = item.SetValue
                    };
                }
                typeGoInfo.ArrayProperties = typeGoInfo.Properties.Values.ToArray();
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    serializer.SerializeFunction(typeGoInfo, serializer, builder, ref data);
                };
            }

            return typeGoInfo;
        }
        public static IPropertyCallerInfo GetDelegateInstance(Type type, PropertyInfo propertyInfo)
        {
            var openGetterType = typeof(Func<,>);
            var concreteGetterType = openGetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            Delegate getterInvocation = Delegate.CreateDelegate(concreteGetterType, null, propertyInfo.GetGetMethod());

            var callerType = typeof(PropertyCallerInfo<,>);
            var callerGenericType = callerType
                .MakeGenericType(type, propertyInfo.PropertyType);

            return (IPropertyCallerInfo)Activator.CreateInstance(callerGenericType, getterInvocation);
        }

    }

    public interface IPropertyCallerInfo
    {
        object GetPropertyValue(object instance);
    }
    public class PropertyCallerInfo<TType, TPropertyType>: IPropertyCallerInfo
    {
        public PropertyCallerInfo(Func<TType, TPropertyType> func)
        {
            GetValue = func;
        }
        public Func<TType, TPropertyType> GetValue { get; set; }

        public object GetPropertyValue(object instance)
        {
            return GetValue((TType)instance);
        }
    }
}
