using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// Sbyte serializer and deserializer
    /// </summary>
    public class SByteNullableVariable : BaseVariable, ISerializationVariable<sbyte?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public SByteNullableVariable() : base(typeof(sbyte?))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<sbyte?> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref sbyte? value)
        {
#if (NETSTANDARD2_0 || NET45)
            if (value.HasValue)
                handler.TextWriter.Write(value.Value.ToString(CurrentCulture).AsSpan());
            else
                handler.TextWriter.Write(JsonConstantsString.Null.AsSpan());
#else
            if (value.HasValue)
                handler.TextWriter.Write(value.Value.ToString(CurrentCulture));
            else
                handler.TextWriter.Write(JsonConstantsString.Null);
#endif
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public sbyte? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0 || NET45)
            if (sbyte.TryParse(new string(text.ToArray()), out sbyte value))
                return value;
#else
            if (sbyte.TryParse(text, out sbyte value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref sbyte? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                stream.Write(new byte[] { (byte)value });
            }
            else
                stream.Write(0);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public sbyte? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
                return (sbyte)reader.Read(sizeof(sbyte))[0];
            return default;
        }
    }
}

