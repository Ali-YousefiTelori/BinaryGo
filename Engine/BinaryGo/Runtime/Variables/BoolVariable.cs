using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Bool serializer and deserializer
    /// </summary>
    public class BoolVariable : BaseVariable, ISerializationVariable<bool>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public BoolVariable() : base(typeof(bool))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<bool> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref bool value)
        {
#if (NETSTANDARD2_0)
            if (value)
                handler.TextWriter.Write(JsonConstantsString.True.AsSpan());
            else
                handler.TextWriter.Write(JsonConstantsString.False.AsSpan());
#else
            if (value)
                handler.TextWriter.Write(JsonConstantsString.True);
            else
                handler.TextWriter.Write(JsonConstantsString.False);
#endif

        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public bool JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0)
            if (bool.TryParse(new string(text.ToArray()), out bool value))
                return value;
#else
            if (bool.TryParse(text, out bool value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref bool value)
        {
            stream.Write(ref value);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public bool BinaryDeserialize(ref BinarySpanReader reader)
        {
#if (NETSTANDARD2_0)
            return BitConverter.ToBoolean(reader.Read(sizeof(bool)).ToArray(), 0);
#else
            return BitConverter.ToBoolean(reader.Read(sizeof(bool)));
#endif
        }
    }
}