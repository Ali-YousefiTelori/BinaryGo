using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Helpers
{
    /// <summary>
    /// Default's saved data of serialization and deserialization
    /// </summary>
    public class BaseOptionInfo : ITypeGo
    {
        /// <summary>
        /// Set default values from dictionary to actions
        /// </summary>
        public BaseOptionInfo()
        {
            AddTypes = Types.Add;
            TryGetValueOfTypeGo = Types.TryGetValue;
        }

        /// <summary>
        /// lock object to pervent cocurrent initializer call
        /// </summary>
        public object LockObject { get; set; } = new object();
        /// <summary>
        /// Cached types
        /// </summary>
        public Dictionary<Type, TypeGoInfo> Types { get; set; } = new Dictionary<Type, TypeGoInfo>();
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
