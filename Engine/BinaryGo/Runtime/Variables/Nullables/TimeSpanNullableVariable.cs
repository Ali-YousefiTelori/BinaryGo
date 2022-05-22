using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// TimeSpan serializer and deserializer
    /// </summary>
    public class TimeSpanNullableVariable : BaseVariable, ISerializationVariable<TimeSpan?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public TimeSpanNullableVariable() : base(typeof(TimeSpan?))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TimeSpan?> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref TimeSpan? value)
        {
            if (value.HasValue)
            {
                handler.TextWriter.Write(JsonConstantsString.Quotes);
                handler.TextWriter.Write(value.Value.ToString());
                handler.TextWriter.Write(JsonConstantsString.Quotes);
            }
            else
                handler.TextWriter.Write(JsonConstantsString.Null);

        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public TimeSpan? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (TimeSpan.TryParse(text, out TimeSpan value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref TimeSpan? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                stream.Write(BitConverter.GetBytes(value.Value.Ticks).AsSpan());
            }
            else
                stream.Write(0);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public TimeSpan? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
                return new TimeSpan(BitConverter.ToInt64(reader.Read(sizeof(long))));
            return default;
        }
    }
}