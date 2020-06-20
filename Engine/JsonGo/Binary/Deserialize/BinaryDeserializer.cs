using JsonGo.Helpers;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Binary.Deserialize
{
    /// <summary>
    /// Json deserializer
    /// </summary>
    public class BinaryDeserializer : ISerializer
    {
        /// <summary>
        /// Serialization's default options
        /// </summary>
        public static BaseOptionInfo DefaultOptions { get; set; } = new BaseOptionInfo();
        /// <summary>
        /// 
        /// </summary>
        public BinaryDeserializer()
        {
            Options = DefaultOptions;
            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
        }

        /// <summary>
        /// Adds new value to types
        /// </summary>
        public Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value 
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }

        internal BaseOptionInfo Options { get; set; }
        /// <summary>
        /// Save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static BinaryDeserializer NormalInstance
        {
            get
            {
                return new BinaryDeserializer();
            }
        }

        /// <summary>
        /// Support for types' loop reference
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Deserializes a stream to a type
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="reader">SpanReader binary to deserialize</param>
        /// <returns>deserialized type</returns>
        public T Deserialize<T>(ReadOnlySpan<byte> reader)
        {
            try
            {
                var dataType = typeof(T);
                if (!TryGetValueOfTypeGo(dataType, out object typeGoInfo))
                {
                    typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);
                }
                var binaryReader = new BinarySpanReader(reader);
                return ((TypeGoInfo<T>)typeGoInfo).BinaryDeserialize(ref binaryReader);
            }
            finally
            {
                DeSerializedObjects.Clear();
            }
        }
    }
}

