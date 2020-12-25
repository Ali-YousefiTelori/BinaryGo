using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.IO;
using JsonGo.Json;
using System;

namespace JsonGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// Nullable Bool serializer and deserializer
    /// </summary>
    public class BoolNullableVariable : BaseVariable, ISerializationVariable<bool?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public BoolNullableVariable() : base(typeof(bool?))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<bool?> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                    handler.TextWriter.Write(JsonConstantsString.True);
                else
                    handler.TextWriter.Write(JsonConstantsString.False);
            }
            else
                handler.TextWriter.Write(JsonConstantsString.Null);

        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public bool? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (bool.TryParse(text, out bool value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref bool? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                stream.Write(BitConverter.GetBytes(value.Value));
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public bool? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
                return BitConverter.ToBoolean(reader.Read(sizeof(bool)));
            return default;
        }
    }
}