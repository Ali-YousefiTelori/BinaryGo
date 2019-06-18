using JsonGo.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public Dictionary<string, PropertyGoInfo> Properties { get; set; } = new Dictionary<string, PropertyGoInfo>();
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
            TypeGoInfo typeGoInfo = new TypeGoInfo();
            if (type == typeof(int) ||
                type == typeof(DateTime) ||
                type == typeof(uint) ||
                type == typeof(long) ||
                type == typeof(short) ||
                type == typeof(byte) ||
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(decimal) ||
                type == typeof(sbyte) ||
                type == typeof(ulong) ||
                type == typeof(bool) ||
                type == typeof(ushort))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    builder.Append(JsonSettingInfo.Quotes);
                    builder.Append(data);
                    builder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    builder.Append(JsonSettingInfo.Quotes);
                    builder.Append((data as string).Replace("\"", "\\\""));
                    builder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    builder.Append(JsonSettingInfo.Quotes);
                    builder.Append(Convert.ToInt32(data));
                    builder.Append(JsonSettingInfo.Quotes);
                };
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    serializer.SerializeArrayFunction(typeGoInfo, serializer, builder, ref data);
                };
            }
            else
            {
                foreach (var item in type.GetProperties())
                {
                    if (item.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        continue;
                    if (!Types.TryGetValue(item.PropertyType, out TypeGoInfo typeGoInfoProperty))
                        Types[item.PropertyType] = typeGoInfoProperty = Generate(item.PropertyType);
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = item.PropertyType,
                        Name = item.Name,
                        GetValue = item.GetValue,
                        SetValue = item.SetValue
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
                        GetValue = item.GetValue,
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


    }
}
