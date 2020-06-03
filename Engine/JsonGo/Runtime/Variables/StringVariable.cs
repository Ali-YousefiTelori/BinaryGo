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
    /// String serializer and deserializer
    /// </summary>
    public class StringVariable : ISerializationVariable
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
                handler.AppendChar(JsonConstantsString.Quotes);
                var result = ((string)data).AsSpan();
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] == '"')
                    {
                        handler.Append("\\\"");
                    }
                    //else if (result[i] == '\r' && i < result.Length - 1 && result[i + 1] == '\n')
                    //{
                    //    handler.Append("\\r\\n");
                    //    i++;
                    //}
                    else if (result[i] == '\n')
                        handler.Append("\\n");
                    else if (result[i] == '\r')
                        handler.Append("\\r");
                    else if (result[i] == '\t')
                        handler.Append("\\t");
                    else
                        handler.AppendChar(result[i]);
                }
                //handler.Append(((string)data).Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
                handler.AppendChar(JsonConstantsString.Quotes);
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                return new string(x);
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                var text = ((string)data);
                stream.Write(BitConverter.GetBytes(text.Length));
                stream.Write(Encoding.UTF8.GetBytes(text));
            };

            //binary deserialization
            typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
            {
                var length = BitConverter.ToInt32(reader.Read(sizeof(int)));
                return Encoding.UTF8.GetString(reader.Read(length));
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(string);
        }
    }
}
