using JsonGo.Helpers;
using JsonGo.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Deserialize
{
    //delegate void FastExtractFunction(TypeGoInfo typeGo, ref object instance, Func<object> createInstance, ref ReadOnlySpan<byte> _buffer);
    delegate object FastExtractFunction(Deserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader _buffer);

    /// <summary>
    /// deserializer of json
    /// </summary>
    public class Deserializer : IJson
    {
        static FastExtractFunction FastExtractFunction { get; set; }

        static Deserializer()
        {
            SingleIntance = new Deserializer();
            //ExtractFunction = DeserializerExtractor.Extract;
            //FastExtractFunction = FastDeserializerExtractor2.Extract;
            FastExtractFunction = FastDeserializerExtractor3.Extract;
        }
        public Deserializer()
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

        internal JsonOptionInfo Options { get; set; } = new JsonOptionInfo() { IsGenerateLoopReference = true };
        /// <summary>
        /// save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// single instance of deserialize to access faster
        /// </summary>
        public static Deserializer SingleIntance { get; set; }
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();

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

        /// <summary>
        /// get type of a json parameter name
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="key">json parameter name</param>
        /// <returns>type of json parameter</returns>
        //internal Type GetKeyType(object obj, string key)
        //{
        //    if (obj == null)
        //        return null;
        //    key = key.Trim('\"');
        //    Type dataType = obj.GetType();
        //    if (!TryGetValueOfTypeGo(dataType, out TypeGoInfo typeGoInfo))
        //    {
        //        typeGoInfo = TypeGoInfo.Generate(dataType, this);
        //        AddTypes(dataType, typeGoInfo);
        //    }
        //    if (typeGoInfo.Properties.TryGetValue(key, out PropertyGoInfo propertyGo))
        //    {
        //        return propertyGo.Type;
        //    }
        //    return null;
        //}

    }
}
