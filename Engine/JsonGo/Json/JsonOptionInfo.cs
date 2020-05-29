using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Json
{
    /// <summary>
    /// default saved data of serialization and deserialization
    /// </summary>
    public class JsonOptionInfo : ITypeGo
    {
        /// <summary>
        /// set default values from dictionary to actions
        /// </summary>
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

        /// <summary>
        /// the loop reference generations
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// add type to typeGo dictionary to access  faster
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }

        /// <summary>
        /// get type go value from a type
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
    }
}
