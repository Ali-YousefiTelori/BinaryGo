using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables.Enums
{
    /// <summary>
    /// Enum serializer and deserializer
    /// </summary>
    public class EnumVariable<TEnum> where TEnum : struct, Enum
    {
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        public static BaseVariable Initialize()
        {
            var type = typeof(TEnum);
            var baseType = Nullable.GetUnderlyingType(type);
            if (baseType == null)
                baseType = type;
            var enumType = Enum.GetUnderlyingType(baseType);
            if (enumType == typeof(uint))
            {
                return new EnumUIntVariable<TEnum>();
            }
            else if (enumType == typeof(long))
            {
                return new EnumLongVariable<TEnum>();
            }
            else if (enumType == typeof(short))
            {
                return new EnumShortVariable<TEnum>();
            }
            else if (enumType == typeof(sbyte))
            {
                return new EnumSByteVariable<TEnum>();
            }
            else if (enumType == typeof(ulong))
            {
                return new EnumuLongVariable<TEnum>();
            }
            else if (enumType == typeof(ushort))
            {
                return new EnumUShortVariable<TEnum>();
            }
            else if (enumType == typeof(int))
            {
                return new EnumIntVariable<TEnum>();
            }
            else
            {
                throw new Exception($"enum of type {enumType.FullName} not support yet, please contact programmer!");
            }
        }
    }
}
