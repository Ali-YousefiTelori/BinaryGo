using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// Date and time serializer and deserializer
    /// </summary>
    public class DateTimeNullableVariable : BaseVariable, ISerializationVariable<DateTime?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public DateTimeNullableVariable() : base(typeof(DateTime?))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<DateTime?> typeGoInfo, ITypeOptions options)
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
        /// format of datetime serialization
        /// </summary>
        public string Format = "yyyy-MM-ddTHH\\:mm\\:sszzz";

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref DateTime? value)
        {
#if (NETSTANDARD2_0 || NET45)
            if (value.HasValue)
            {
                handler.TextWriter.Write(JsonConstantsString.Quotes);
                handler.TextWriter.Write(value.Value.ToString(Format, CurrentCulture).AsSpan());
                handler.TextWriter.Write(JsonConstantsString.Quotes);
            }
            else
                handler.TextWriter.Write(JsonConstantsString.Null.AsSpan());
#else
            if (value.HasValue)
            {
                handler.TextWriter.Write(JsonConstantsString.Quotes);
                handler.TextWriter.Write(value.Value.ToString(Format, CurrentCulture));
                handler.TextWriter.Write(JsonConstantsString.Quotes);
            }
            else
                handler.TextWriter.Write(JsonConstantsString.Null);
#endif
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public DateTime? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0 || NET45)
            if (DateTime.TryParse(new string(text.ToArray()), out DateTime value))
                return value;
#else
            if (DateTime.TryParse(text, out DateTime value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref DateTime? value)
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
        public DateTime? BinaryDeserialize(ref BinarySpanReader reader)
        {
#if (NETSTANDARD2_0 || NET45)
            if (reader.Read() == 1)
                return new DateTime(BitConverter.ToInt64(reader.Read(sizeof(long)).ToArray(), 0));
#else
            if (reader.Read() == 1)
                return new DateTime(BitConverter.ToInt64(reader.Read(sizeof(long))));
#endif
            return default;
        }
    }
}
