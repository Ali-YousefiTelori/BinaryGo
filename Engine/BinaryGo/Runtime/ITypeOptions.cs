using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BinaryGo.Runtime
{
    /// <summary>
    /// type options
    /// </summary>
    public interface ITypeOptions
    {
        /// <summary>
        /// Encoding of type go options
        /// </summary>
        Encoding Encoding { get; set; }
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
        /// <summary>
        /// custom types
        /// </summary>
        Dictionary<Type, Type> CustomTypeChanges { get; set; }
        /// <summary>
        /// CurrentCulture of serializer
        /// </summary>
        CultureInfo CurrentCulture { get; set; }
    }
}
