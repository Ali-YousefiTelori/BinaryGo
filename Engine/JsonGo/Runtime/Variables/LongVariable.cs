using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Long serializer and deserializer
    /// </summary>
    public class LongVariable : ISerializationVariable
    {
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo typeGoInfo, ITypeGo options)
        {
            var currentCulture = TypeGoInfo.CurrentCulture;
            typeGoInfo.IsNoQuotesValueType = false;

            //json serialize
            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
            {
                handler.Append(((long)data).ToString(currentCulture));
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                if (long.TryParse(x, out long value))
                    return value;
                return default(long);
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                stream.Write(BitConverter.GetBytes((long)data).AsSpan());
            };

            //binary deserialization
            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
            {
                return BitConverter.ToInt64(reader.Read(sizeof(long)));
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(long);
        }
    }
}
