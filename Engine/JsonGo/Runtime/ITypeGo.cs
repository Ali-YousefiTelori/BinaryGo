using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// This interface ha a fast access from everywhere to add and get type from memory
    /// </summary>
    public interface ITypeGo
    {
        /// <summary>
        /// Adds new value to types
        /// </summary>
        Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value from 
        /// </summary>
        TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// Support for loop refereence serialization
        /// </summary>
        bool HasGenerateRefrencedTypes { get; set; }
    }

}
