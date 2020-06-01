using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// long serializer and deserializer
    /// </summary>
    public class LongVariable : ISerializationVariable
    {
        /// <summary>
        /// initalize this variable to your typeGo
        /// </summary>
        /// <param name="typeGoInfo">typeGo to initialize variable on it</param>
        /// <param name="options">options of setting of variable serializer or deserializer</param>
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

            //set the default value of variable
            typeGoInfo.DefaultValue = default(long);
        }
    }
}
