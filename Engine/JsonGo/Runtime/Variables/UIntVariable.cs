using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.IO;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// uint serializer and deserializer
    /// </summary>
    public class UIntVariable : ISerializationVariable
    {
        /// <summary>
        /// initalize this variable to your typeGo
        /// </summary>
        /// <param name="typeGoInfo">typeGo to initialize variable on it</param>
        public void Initialize(TypeGoInfo typeGoInfo)
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

            //set the default value of variable
            typeGoInfo.DefaultValue = default(uint);
        }
    }
}
