using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Binary
{
    /// <summary>
    /// Binary serializer helper
    /// </summary>
    public class BinarySerializeHandler
    {
        /// <summary>
        /// The binary serializer
        /// </summary>
        public BinarySerializer Serializer { get; set; }
        /// <summary>
        /// Add an object to serialized objects for references
        /// </summary>
        public Action<object, int> AddSerializedObjects { get; set; }
        ///// <summary>
        ///// Find an object to check if is serialized for references
        ///// </summary>
        //public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
