using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryGo.Runtime.Variables.Enums
{
    /// <summary>
    /// Enum serializer and deserializer
    /// </summary>
    public class EnumNullableVariable<TEnum> where TEnum : struct, Enum
    {
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        public static BaseVariable Initialize()
        {
            var type = typeof(TEnum);
            var enumType = Enum.GetUnderlyingType(type);
            if (enumType == typeof(uint))
            {
                return new EnumNullableUIntVariable<TEnum>();
            }
            else if (enumType == typeof(long))
            {
                return new EnumNullableLongVariable<TEnum>();
            }
            else if (enumType == typeof(short))
            {
                return new EnumNullableShortVariable<TEnum>();
            }
            else if (enumType == typeof(sbyte))
            {
                return new EnumNullableSByteVariable<TEnum>();
            }
            else if (enumType == typeof(ulong))
            {
                return new EnumNullableULongVariable<TEnum>();
            }
            else if (enumType == typeof(ushort))
            {
                return new EnumNullableUShortVariable<TEnum>();
            }
            else if (enumType == typeof(int))
            {
                return new EnumNullableIntVariable<TEnum>();
            }
            else
            {
                throw new Exception($"enum of type {enumType.FullName} not support yet, please contact programmer!");
            }
        }
    }
}
