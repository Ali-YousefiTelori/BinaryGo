﻿using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Short serializer and deserializer
    /// </summary>
    public class ShortVariable : BaseVariable, ISerializationVariable<short>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ShortVariable() : base(typeof(short))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<short> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref short value)
        {
#if (NETSTANDARD2_0)
            handler.TextWriter.Write(value.ToString(CurrentCulture).AsSpan());
#else
            handler.TextWriter.Write(value.ToString(CurrentCulture));
#endif
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public short JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0)
            if (short.TryParse(new string(text.ToArray()), out short value))
                return value;
#else
            if (short.TryParse(text, out short value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref short value)
        {
            stream.Write(ref value);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public short BinaryDeserialize(ref BinarySpanReader reader)
        {
#if (NETSTANDARD2_0)
            return BitConverter.ToInt16(reader.Read(sizeof(short)).ToArray(), 0);
#else
            return BitConverter.ToInt16(reader.Read(sizeof(short)));
#endif
        }
    }
}
