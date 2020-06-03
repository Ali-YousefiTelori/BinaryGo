using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.IO;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Uint serializer and deserializer
    /// </summary>
    public class UIntVariable : ISerializationVariable
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
                handler.Append(((uint)data).ToString(currentCulture));
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                if (uint.TryParse(x, out uint value))
                    return value;
                return default(uint);
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                stream.Write(BitConverter.GetBytes((uint)data).AsSpan());
            };

            //binary deserialization
            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
            {
                return BitConverter.ToUInt32(reader.Read(sizeof(uint)));
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(uint);
        }
    }
}
