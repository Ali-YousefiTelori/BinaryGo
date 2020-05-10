using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo
{
    public interface IJson
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
