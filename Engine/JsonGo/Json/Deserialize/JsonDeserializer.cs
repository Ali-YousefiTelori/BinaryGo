using JsonGo.Helpers;
using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace JsonGo.Json.Deserialize
{
    /// <summary>
    /// Json deserializer
    /// </summary>
    public class JsonDeserializer : ISerializer
    {
        /// <summary>
        /// Serialization's default options
        /// </summary>
        public static BaseOptionInfo DefaultOptions { get; set; } = new BaseOptionInfo();

        //static FastExtractFunction FastExtractFunction { get; set; }

        static JsonDeserializer()
        {
            //ExtractFunction = DeserializerExtractor.Extract;
            //FastExtractFunction = FastDeserializerExtractor2.Extract;
            //FastExtractFunction = FastDeserializerExtractor.Extract;
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
        public Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value 
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }

        internal BaseOptionInfo Options { get; set; } = new BaseOptionInfo() { HasGenerateRefrencedTypes = true };
        /// <summary>
        /// Save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static JsonDeserializer NormalInstance
        {
            get
            {
                return new JsonDeserializer();
            }
        }

        /// <summary>
        /// Serializer's default settings
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();
        /// <summary>
        /// Support for types' loop reference
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }
        /// <summary>
        /// current serializer Culture
        /// </summary>
        public CultureInfo CurrentCulture { get; set; }
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
                if (!TryGetValueOfTypeGo(dataType, out object typeGoInfo))
                {
                    typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);
                }
                var reader = new JsonSpanReader(json.AsSpan());
                JsonDeserializer deserializer = this;
                var result = FastDeserializerExtractor<T>.Extract(ref deserializer, (TypeGoInfo<T>)typeGoInfo, ref reader);
                return result;
            }
            finally
            {
                //DeSerializedObjects.Clear();
            }
        }
    }
}
