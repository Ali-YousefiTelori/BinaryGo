using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Runtime.Variables.Structures;
using System;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Decimal serializer and deserializer
    /// </summary>
    public class DecimalVariable : BaseVariable, ISerializationVariable<decimal>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public DecimalVariable() : base(typeof(decimal))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<decimal> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref decimal value)
        {
            handler.TextWriter.Write(value.ToString(CurrentCulture));
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public decimal JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (decimal.TryParse(text, out decimal value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref decimal value)
        {
            stream.Write(ref value);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public decimal BinaryDeserialize(ref BinarySpanReader reader)
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

    }
}
