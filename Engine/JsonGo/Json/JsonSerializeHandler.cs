using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Json
{
    /// <summary>
    /// json serializer handler to access faster method to serailize and deserialize
    /// </summary>
    public class JsonSerializeHandler
    {
        /// <summary>
        /// append a text
        /// </summary>
        public RefFunc<StringBuilder> Append { get; set; }
        /// <summary>
        /// append a character
        /// </summary>
        public Func<char, StringBuilder> AppendChar { get; set; }
        /// <summary>
        /// serializer
        /// </summary>
        public Serializer Serializer { get; set; }
        /// <summary>
        /// add object to serialized for references
        /// </summary>
        public Action<object, int> AddSerializedObjects { get; set; }
        /// <summary>
        /// find object for serialziation for reference values
        /// </summary>
        public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
