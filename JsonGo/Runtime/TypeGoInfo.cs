using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Runtime
{
    public delegate StringBuilder FunctionGo(Serializer serializer, StringBuilder stringBuilder, ref object data);
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
                    return builder;
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    builder.Append(JsonSettingInfo.Quotes); ;
                    builder.Append((data as string).Replace("\"", "\\\""));
                    builder.Append(JsonSettingInfo.Quotes);
                    return builder;
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    builder.Append(JsonSettingInfo.Quotes);
                    builder.Append(Convert.ToInt32(data));
                    builder.Append(JsonSettingInfo.Quotes);
                    return builder;
                };
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                //if (serializer.Setting.HasGenerateRefrencedTypes)
                //{
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    if (GenerateReference(serializer, ref data, builder, out object refrencedId))
                        return builder;
                    return Serializer.SerializeArrayReference(builder, (IEnumerable)data, ref  refrencedId, serializer);
                };
                //}
                //else
                //{
                //    typeGoInfo.Serialize = () =>
                //    {
                //        return serializer.SerializeArray(list);
                //    };
                //}
            }
            else
            {
                foreach (var item in type.GetProperties())
                {
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        Type = item.PropertyType,
                        Name = item.Name,
                        GetValue = item.GetValue,
                        SetValue = item.SetValue
                    };
                }
                foreach (var item in type.GetFields())
                {
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        Type = item.FieldType,
                        Name = item.Name,
                        GetValue = item.GetValue,
                        SetValue = item.SetValue
                    };
                }
                typeGoInfo.ArrayProperties = typeGoInfo.Properties.Values.ToArray();
                typeGoInfo.Serialize = (Serializer serializer, StringBuilder builder, ref object data) =>
                {
                    if (GenerateReference(serializer, ref data, builder, out object refrencedId))
                        return builder;
                    return Serializer.SerializeObjectReference(builder, data, ref refrencedId, typeGoInfo, serializer);
                };
            }

            return typeGoInfo;
        }

        static bool GenerateReference(Serializer serializer, ref object data, StringBuilder builder, out object refrencedId)
        {
            if (!serializer.SerializedObjects.TryGetValue(data, out refrencedId))
            {
                refrencedId = serializer.ReferencedIndex;
                serializer.SerializedObjects.Add(data, refrencedId);
                serializer.ReferencedIndex++;
                return false;
            }
            else
            {
                builder.Append(JsonSettingInfo.OpenBraket);
                builder.Append(JsonSettingInfo.RefRefrencedTypeName);
                builder.Append(JsonSettingInfo.ColonQuotes);
                builder.Append(refrencedId);
                builder.Append(JsonSettingInfo.Quotes);
                builder.Append(JsonSettingInfo.CloseBracket);
                return true;
            }
        }

    }
}
