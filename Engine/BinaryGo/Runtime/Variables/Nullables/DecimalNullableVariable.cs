using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Runtime.Variables.Structures;
using System;

namespace BinaryGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// Decimal serializer and deserializer
    /// </summary>
    public class DecimalNullableVariable : BaseVariable, ISerializationVariable<decimal?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public DecimalNullableVariable() : base(typeof(decimal?))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<decimal?> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref decimal? value)
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
        public decimal? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
#if (NETSTANDARD2_0 || NET45)
            if (decimal.TryParse(new string(text.ToArray()), out decimal value))
                return value;
#else
            if (decimal.TryParse(text, out decimal value))
                return value;
#endif
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref decimal? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                stream.Write(value.Value);
            }
            else
                stream.Write(0);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public decimal? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
            {
                var data = reader.Read(16);
                return new DecimalStruct()
                {
                    Byte0 = data[0],
                    Byte1 = data[1],
                    Byte2 = data[2],
                    Byte3 = data[3],
                    Byte4 = data[4],
                    Byte5 = data[5],
                    Byte6 = data[6],
                    Byte7 = data[7],
                    Byte8 = data[8],
                    Byte9 = data[9],
                    Byte10 = data[10],
                    Byte11 = data[11],
                    Byte12 = data[12],
                    Byte13 = data[13],
                    Byte14 = data[14],
                    Byte15 = data[15]

                }.Value;
            }
            return default;
        }

    }
}
