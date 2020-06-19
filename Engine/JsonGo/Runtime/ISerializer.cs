using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// This interface ha a fast access from everywhere to add and get type from memory
    /// base of any serializer need to impliment data to fast access
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Adds new value to types
        /// </summary>
        Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value from 
        /// </summary>
        TryGetValue<Type> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// Support for loop refereence serialization
        /// </summary>
        bool HasGenerateRefrencedTypes { get; set; }
    }

}
