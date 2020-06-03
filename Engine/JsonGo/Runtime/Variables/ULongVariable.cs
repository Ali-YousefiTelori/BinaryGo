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
    /// Ulong serializer and deserializer
    /// </summary>
    public class ULongVariable : ISerializationVariable
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
                handler.Append(((ulong)data).ToString(currentCulture));
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                if (ulong.TryParse(x, out ulong value))
                    return value;
                return default(ulong);
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                stream.Write(BitConverter.GetBytes((ulong)data).AsSpan());
            };

            //binary deserialization
            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
            {
                return BitConverter.ToUInt64(reader.Read(sizeof(ulong)));
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(ulong);
        }
    }
}