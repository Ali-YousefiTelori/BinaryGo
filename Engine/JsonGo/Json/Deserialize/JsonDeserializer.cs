using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;

namespace JsonGo.Json.Deserialize
{

    /// <summary>
    /// Json deserializer
    /// </summary>
    public class JsonDeserializer : ITypeGo
    {
        static FastExtractFunction FastExtractFunction { get; set; }

        static JsonDeserializer()
        {
            SingleInstance = new JsonDeserializer();
            //ExtractFunction = DeserializerExtractor.Extract;
            //FastExtractFunction = FastDeserializerExtractor2.Extract;
            FastExtractFunction = FastDeserializerExtractor3.Extract;
        }

        /// <summary>
        /// 
        /// </summary>
        public JsonDeserializer()
        {
            Initialize();
        }
        /// <summary>
        /// Adds new value to types
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value 
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }

        internal JsonOptionInfo Options { get; set; } = new JsonOptionInfo() { HasGenerateRefrencedTypes = true };
        /// <summary>
        /// Save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static JsonDeserializer SingleInstance { get; set; }
        /// <summary>
        /// Serializer's default settings
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();
        /// <summary>
        /// Support for types' loop reference
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        void Initialize()
        {
            AddTypes = Options.Types.Add;
            //AddSerializedObjects = Options.SerializedObjects.Add;
            //ClearSerializedObjects = Options.SerializedObjects.Clear;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            //TryGetValueOfSerializedObjects = Options.SerializedObjects.TryGetValue;
        }

        /// <summary>
        /// Deserializes a json to a type
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="json">Json string to deserialize</param>
        /// <returns>deserialized type</returns>
        public T Deserialize<T>(string json)
        {
            try
            {
                var dataType = typeof(T);
                if (!TryGetValueOfTypeGo(dataType, out TypeGoInfo typeGoInfo))
                {
                    typeGoInfo = TypeGoInfo.Generate(dataType, this);
                }
                var reader = new JsonSpanReader(json.AsSpan());
                var result = FastExtractFunction(this, typeGoInfo, ref reader);
                return (T)result;
            }
            finally
            {
                DeSerializedObjects.Clear();
            }
        }
    }
}
