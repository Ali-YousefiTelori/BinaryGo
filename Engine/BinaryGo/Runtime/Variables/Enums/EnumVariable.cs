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
    public class EnumVariable<TEnum> where TEnum : struct, Enum
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
                return new EnumULongVariable<TEnum>();
            }
            else if (enumType == typeof(ushort))
            {
                return new EnumUShortVariable<TEnum>();
            }
            else if (enumType == typeof(int))
            {
                return new EnumIntVariable<TEnum>();
            }
            else if (enumType == typeof(byte))
            {
                return new EnumByteVariable<TEnum>();
            }
            else
            {
                throw new Exception($"Enum of type {enumType.FullName} not support yet, please contact programmer!");
            }
        }
    }
}
