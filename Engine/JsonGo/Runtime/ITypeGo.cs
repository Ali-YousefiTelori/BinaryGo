using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// the interface with a fast way access to add and get type from memory
    /// </summary>
    public interface ITypeGo
    {
        /// <summary>
        /// add new value to types
        /// </summary>
        Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// get typefo value from 
        /// </summary>
        TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// support for loop refereence serialization
        /// </summary>
        bool HasGenerateRefrencedTypes { get; set; }
    }

}
