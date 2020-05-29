using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Binary
{
    /// <summary>
    /// binary serializer helper
    /// </summary>
    public class BinarySerializeHandler
    {
        /// <summary>
        /// serializer
        /// </summary>
        public BinarySerializer Serializer { get; set; }
        /// <summary>
        /// add object to serialized object for references
        /// </summary>
        public Action<object, int> AddSerializedObjects { get; set; }
        /// <summary>
        /// find object to check is serialized for references
        /// </summary>
        public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
