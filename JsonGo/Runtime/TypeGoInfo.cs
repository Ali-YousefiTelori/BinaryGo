using System;
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
        public Func<object, string> Serialize { get; set; }

        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> Properties { get; set; } = new Dictionary<string, PropertyGoInfo>();
        /// <summary>
        /// serialize string
        /// </summary>
        public static Func<string, string> SerializeStringFunction { get; set; }
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
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (data) =>
                {
                    return string.Concat('\"', data, '\"');
                };
            }
            else if (type == typeof(string))
            {
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (data) =>
                {
                    return string.Concat('\"', SerializeStringFunction(data as string), '\"');
                };
            }
            else if (type.IsEnum)
            {
                typeGoInfo.IsSimpleType = true;
                typeGoInfo.Serialize = (data) =>
                {
                    return string.Concat('\"', Convert.ToInt32(data), '\"');
                };
            }
            else
            {
                foreach (var item in type.GetProperties())
                {
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        Name = item.Name,
                        GetValue = item.GetValue
                    };
                }
            }
            
            return typeGoInfo;
        }

    }
}
