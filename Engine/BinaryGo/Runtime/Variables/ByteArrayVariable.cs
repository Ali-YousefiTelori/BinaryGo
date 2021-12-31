using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Byte[] serializer and deserializer
    /// </summary>
    public class ByteArrayVariable : BaseVariable, ISerializationVariable<byte[]>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ByteArrayVariable() : base(typeof(byte[]))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<byte[]> typeGoInfo, ITypeOptions options)
        {
            typeGoInfo.IsNoQuotesValueType = false;
            //set the default value of variable
            typeGoInfo.DefaultValue = default;
            typeGoInfo.JsonSerialize = JsonSerialize;
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref byte[] value)
        {
            handler.TextWriter.Write(JsonConstantsString.Quotes);
            handler.TextWriter.Write(Convert.ToBase64String(value));
            handler.TextWriter.Write(JsonConstantsString.Quotes);
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public byte[] JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            return Convert.FromBase64String(new string(text));
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref byte[] value)
        {
            stream.Write(BitConverter.GetBytes(value.Length));
            stream.Write(value);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public byte[] BinaryDeserialize(ref BinarySpanReader reader)
        {
            int length = BitConverter.ToInt32(reader.Read(sizeof(int)));
            if (length == 0)
                return null;
            return reader.Read(length).ToArray();
        }

    }
}