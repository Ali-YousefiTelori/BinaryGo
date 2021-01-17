using BinaryGo.IO;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGo.Json
{
    /// <summary>
    /// Json serializer handler to use serialization and deserialization methods faster
    /// </summary>
    public ref struct JsonSerializeHandler
    {
        /// <summary>
        /// writer of char
        /// </summary>
        public BufferCharBuilder TextWriter;
        /// <summary>
        /// writer of char
        /// </summary>
        public BufferBuilder BinaryWriter;
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
        public TryGetValue<object> TryGetValueOfSerializedObjects { get; set; }
    }
}
