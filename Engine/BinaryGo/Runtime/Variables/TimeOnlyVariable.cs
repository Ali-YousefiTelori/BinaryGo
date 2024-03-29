﻿#if (NET6_0)
using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// Time only of dot net 6.0 serializer and deserializer
    /// </summary>
    public class TimeOnlyVariable : BaseVariable, ISerializationVariable<TimeOnly>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public TimeOnlyVariable() : base(typeof(TimeOnly))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TimeOnly> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref TimeOnly value)
        {
            handler.TextWriter.Write(JsonConstantsString.Quotes);
            handler.TextWriter.Write(value.ToString(CurrentCulture));
            handler.TextWriter.Write(JsonConstantsString.Quotes);
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public TimeOnly JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (TimeOnly.TryParse(text, out TimeOnly value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref TimeOnly value)
        {
            stream.Write(value.Ticks);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public TimeOnly BinaryDeserialize(ref BinarySpanReader reader)
        {
            return new TimeOnly(BitConverter.ToInt64(reader.Read(sizeof(long))));
        }
    }
}
#endif