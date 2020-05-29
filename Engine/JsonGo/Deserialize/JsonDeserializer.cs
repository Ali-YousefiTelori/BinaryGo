using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;

namespace JsonGo.Deserialize
{

    /// <summary>
    /// deserializer of json
    /// </summary>
    public class JsonDeserializer : ITypeGo
    {
        static FastExtractFunction FastExtractFunction { get; set; }

        static JsonDeserializer()
        {
            SingleIntance = new JsonDeserializer();
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
        /// add new value to types
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// get typefo value from 
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }

        internal JsonOptionInfo Options { get; set; } = new JsonOptionInfo() { HasGenerateRefrencedTypes = true };
        /// <summary>
        /// save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// single instance of deserialize to access faster
        /// </summary>
        public static JsonDeserializer SingleIntance { get; set; }
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();
        /// <summary>
        /// has support for loop reference of types
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
        /// deserialize a json to a type
        /// </summary>
        /// <typeparam name="T">type of deserialize</typeparam>
        /// <param name="json">json to deserialize</param>
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
