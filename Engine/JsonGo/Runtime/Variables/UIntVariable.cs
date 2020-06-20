using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.IO;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Uint serializer and deserializer
    /// </summary>
    public class UIntVariable : BaseVariable, ISerializationVariable<uint>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public UIntVariable() : base(typeof(uint))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<uint> typeGoInfo, ITypeGo options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref uint value)
        {
            handler.TextWriter.Write(value.ToString(CurrentCulture));
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public uint JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (uint.TryParse(text, out uint value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder<byte> stream, ref uint value)
        {
            stream.Write(BitConverter.GetBytes(value).AsSpan());
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public uint BinaryDeserialize(ref BinarySpanReader reader)
        {
            return BitConverter.ToUInt32(reader.Read(sizeof(uint)));
        }
    }
}
