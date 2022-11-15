using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Int serializer and deserializer
    /// </summary>
    public class IntVariable : BaseVariable, ISerializationVariable<int>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public IntVariable() : base(typeof(int))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<int> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref int value)
        {
#if (NETSTANDARD2_0 || NET45)
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
        public int JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0 || NET45)
            if (int.TryParse(new string(text.ToArray()), out int value))
                return value;
#else
            if (int.TryParse(text, out int value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref int value)
        {
            stream.Write(ref value);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public int BinaryDeserialize(ref BinarySpanReader reader)
        {
#if (NETSTANDARD2_0 || NET45)
            return BitConverter.ToInt32(reader.Read(sizeof(int)).ToArray(), 0);
#else
            return BitConverter.ToInt32(reader.Read(sizeof(int)));
#endif
        }

    }
}
