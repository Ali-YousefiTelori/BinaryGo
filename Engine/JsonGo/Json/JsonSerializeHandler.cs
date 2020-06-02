using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Json
{
    /// <summary>
    /// Json serializer handler to use serialization and deserialization methods faster
    /// </summary>
    public class JsonSerializeHandler
    {
        /// <summary>
        /// Appends a text
        /// </summary>
        public RefFunc<StringBuilder> Append { get; set; }
        /// <summary>
        /// Appends a character
        /// </summary>
        public Func<char, StringBuilder> AppendChar { get; set; }
        /// <summary>
        /// Serializer
        /// </summary>
        public Serializer Serializer { get; set; }
        /// <summary>
        /// Adds object to serialized for references
        /// </summary>
        public Action<object, int> AddSerializedObjects { get; set; }
        /// <summary>
        /// Finds serialization object for reference values
        /// </summary>
        public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
