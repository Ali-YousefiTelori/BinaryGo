﻿using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Byte serializer and deserializer
    /// </summary>
    public class ByteVariable : BaseVariable, ISerializationVariable<byte>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ByteVariable() : base(typeof(byte))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<byte> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref byte value)
        {
            handler.TextWriter.Write(value.ToString(CurrentCulture));
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public byte JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (byte.TryParse(text, out byte value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder<byte> stream, ref byte value)
        {
            stream.Write(new byte[] { value });
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public byte BinaryDeserialize(ref BinarySpanReader reader)
        {
            return reader.Read(sizeof(byte))[0];
        }
    }
}

