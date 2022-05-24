﻿using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;
using System.Runtime.CompilerServices;

namespace BinaryGo.Runtime.Variables.Enums
{
    /// <summary>
    /// Enum that inheritance ulong
    /// </summary>
    public class EnumNullableULongVariable<TEnum> : BaseVariable, ISerializationVariable<TEnum?>
         where TEnum : struct, Enum
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public EnumNullableULongVariable() : base(typeof(TEnum?))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TEnum?> typeGoInfo, ITypeOptions options)
        {
            typeGoInfo.IsNoQuotesValueType = false;
            //set the default value of variable
            typeGoInfo.DefaultValue = default;

            //set delegates to access faster and make it pointer directly usage
            typeGoInfo.JsonSerialize = JsonSerialize;

            //set delegates to access faster and make it pointer directly usage for json deserializer
            typeGoInfo.JsonDeserialize = JsonDeserialize;

            //set delegates to access faster and make it pointer directly usage for binary serializer
            typeGoInfo.BinarySerialize = BinarySerialize;

            //set delegates to access faster and make it pointer directly usage for binary deserializer
            typeGoInfo.BinaryDeserialize = BinaryDeserialize;
        }

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref TEnum? value)
        {
            if (value.HasValue)
            {
                var data = value.Value;
#if (NETSTANDARD2_0)
                handler.TextWriter.Write(Unsafe.As<TEnum, ulong>(ref data).ToString(CurrentCulture).AsSpan());
#else
                handler.TextWriter.Write(Unsafe.As<TEnum, ulong>(ref data).ToString(CurrentCulture));
#endif
            }
            else
            {
#if (NETSTANDARD2_0)
                handler.TextWriter.Write(JsonConstantsString.Null.AsSpan());
#else
                handler.TextWriter.Write(JsonConstantsString.Null);
#endif
            }
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public TEnum? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0)
            if (ulong.TryParse(new string(text.ToArray()), out ulong value))
                return Unsafe.As<ulong, TEnum>(ref value);
#else
            if (ulong.TryParse(text, out ulong value))
                return Unsafe.As<ulong, TEnum>(ref value);
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref TEnum? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                var data = value.Value;
                stream.Write(BitConverter.GetBytes(Unsafe.As<TEnum, ulong>(ref data)));
            }
            else
            {
                stream.Write(0);
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public TEnum? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
            {
#if (NETSTANDARD2_0)
                var value = BitConverter.ToUInt64(reader.Read(sizeof(ulong)).ToArray(), 0);
#else
                var value = BitConverter.ToUInt64(reader.Read(sizeof(ulong)));
#endif
                return Unsafe.As<ulong, TEnum>(ref value);
            }
            return default;
        }
    }
}
