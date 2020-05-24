using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Binary
{
    public class BinarySerializeHandler
    {
        public BinarySerializer Serializer { get; set; }
        public Action<object, int> AddSerializedObjects { get; set; }
        public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
