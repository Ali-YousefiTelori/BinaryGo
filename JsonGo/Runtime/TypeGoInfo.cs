using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
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
        /// is simple type like int,string,double,enum etc
        /// </summary>
        public bool IsSimpleType { get; set; }
        /// <summary>
        /// serialize action
        /// </summary>
        public Func<Serializer, StringBuilder, object, StringBuilder> Serialize { get; set; }

        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> Properties { get; set; } = new Dictionary<string, PropertyGoInfo>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="refrencedId"></param>
        /// <param name="serializer"></param>
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
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (serializer, builder, data) =>
                {
                    builder.Append('\"');
                    builder.Append(data);
                    builder.Append('\"');
                    return builder;
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (serializer, builder, data) =>
                {
                    builder.Append('\"');
                    Serializer.SerializeString(builder, data as string);
                    builder.Append('\"');
                    return builder;
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (serializer, builder, data) =>
                {
                    builder.Append('\"');
                    builder.Append(Convert.ToInt32(data)); 
                    builder.Append('\"');
                    return builder;
                };
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                //if (serializer.Setting.HasGenerateRefrencedTypes)
                //{
                typeGoInfo.Serialize = (serializer, builder, data) =>
                {
                    if (GenerateReference(serializer, data, builder, out string refrencedId))
                        return builder;
                    return Serializer.SerializeArrayReference(builder, (IEnumerable)data, ref refrencedId, serializer);
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
                typeGoInfo.Serialize = (serializer, builder, data) =>
                {
                    if (GenerateReference(serializer, data, builder, out string refrencedId))
                        return builder;
                    return Serializer.SerializeObjectReference(builder, data, ref refrencedId, typeGoInfo, serializer);
                };
            }

            return typeGoInfo;
        }

       static  bool GenerateReference(Serializer serializer, object data,StringBuilder builder, out string refrencedId)
        {
            if (!serializer.SerializedObjects.TryGetValue(data, out refrencedId))
            {
                refrencedId = serializer.ReferencedIndex.ToString();
                serializer.SerializedObjects.Add(data, refrencedId);
                serializer.ReferencedIndex++;
                return false;
            }
            else
            {
                builder.Append("{");
                builder.Append($"{JsonSettingInfo.RefRefrencedTypeName}:\"{refrencedId}\"");
                builder.Append("}");
                return true;
            }
        }

    }
}
