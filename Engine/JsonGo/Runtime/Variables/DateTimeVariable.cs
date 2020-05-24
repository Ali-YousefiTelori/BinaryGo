using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// date and time serializer and deserializer
    /// </summary>
    public class DateTimeVariable : ISerializationVariable
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
                handler.AppendChar(JsonConstantsString.Quotes);
                handler.Append(((DateTime)data).ToString(currentCulture));
                handler.AppendChar(JsonConstantsString.Quotes);
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                if (DateTime.TryParse(x, out DateTime value))
                    return value;
                return default(DateTime);
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                stream.Write(BitConverter.GetBytes(((DateTime)data).Ticks).AsSpan());
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(DateTime);
        }
    }
}
