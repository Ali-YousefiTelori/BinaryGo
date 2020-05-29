using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Json
{
    public delegate bool TryGetValue<TKey, TResult>(TKey key, out TResult result);
    /// <summary>
    /// default saved data of serialization and deserialization
    /// </summary>
    public class JsonOptionInfo : IJson
    {
        public JsonOptionInfo()
        {
            AddTypes = Types.Add;
            TryGetValueOfTypeGo = Types.TryGetValue;
        }
        /// <summary>
        /// chached types
        /// </summary>
        internal Dictionary<Type, TypeGoInfo> Types { get; set; } = new Dictionary<Type, TypeGoInfo>();
        /// <summary>
        /// save serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        internal Dictionary<object, int> SerializedObjects { get; set; } = new Dictionary<object, int>();

        public bool HasGenerateRefrencedTypes { get; set; }


        public Action<Type, TypeGoInfo> AddTypes { get; set; }

        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
    }
}
