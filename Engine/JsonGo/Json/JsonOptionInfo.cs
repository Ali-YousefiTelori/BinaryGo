using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Json
{
    /// <summary>
    /// Default's saved data of serialization and deserialization
    /// </summary>
    public class JsonOptionInfo : ITypeGo
    {
        /// <summary>
        /// Set default values from dictionary to actions
        /// </summary>
        public JsonOptionInfo()
        {
            AddTypes = Types.Add;
            TryGetValueOfTypeGo = Types.TryGetValue;
        }
        /// <summary>
        /// Cached types
        /// </summary>
        internal Dictionary<Type, TypeGoInfo> Types { get; set; } = new Dictionary<Type, TypeGoInfo>();
        /// <summary>
        /// Saves serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        internal Dictionary<object, int> SerializedObjects { get; set; } = new Dictionary<object, int>();

        /// <summary>
        /// Loop reference generation
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Adds type to typeGo dictionary for faster access
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }

        /// <summary>
        /// Gets TypeGo value from a specific type
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
    }
}
