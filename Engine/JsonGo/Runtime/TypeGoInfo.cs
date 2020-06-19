using System;
using System.Collections.Generic;

namespace JsonGo.Runtime
{
    /// <summary>
    /// Generate type details in memory
    /// </summary>
    public class TypeGoInfo<TType> : BaseTypeGoInfo
    {
        /// <summary>
        /// Default value of object
        /// </summary>
        public TType DefaultValue;
        /// <summary>
        /// If the type is simple like int, byte, bool, enum it can be serialized without quotes
        /// </summary>
        public bool IsNoQuotesValueType = true;
        /// <summary>
        /// Data type
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Serialize action as text
        /// </summary>
        public JsonActionGo<TType> JsonSerialize;
        /// <summary>
        /// get value of serialized
        /// </summary>
        public JsonFunctionGo<TType> GetJsonSerialized;
        /// <summary>
        /// serialize json as binary to a memory stream action
        /// </summary>
        public JsonActionGo<TType> JsonBinarySerialize;
        /// <summary>
        /// Binary serialize
        /// </summary>
        public BinaryFunctionGo BinarySerialize;
        /// <summary>
        /// Deserializes binary to object
        /// </summary>
        public BinaryDeserializeFunc BinaryDeserialize;
        /// <summary>
        /// Deserializes string to object
        /// </summary>
        public DeserializeFunc JsonDeserialize;
        /// <summary>
        /// Creates type instance
        /// </summary>
        public Func<object> CreateInstance;
        /// <summary>
        /// Casts to real object
        /// </summary>
        public Func<object, object> Cast;
        /// <summary>
        /// Adds array value to TypeGo array
        /// </summary>
        public Action<object, object> AddArrayValue;

        /// <summary>
        /// Type properties 
        /// </summary>
        public Dictionary<string, BasePropertyGoInfo<TType>> Properties;
        /// <summary>
        /// Type properties
        /// </summary>
        public Dictionary<string, BasePropertyGoInfo<TType>> DirectProperties;
        /// <summary>
        /// Array of all properties to serialize
        /// </summary>
        public BasePropertyGoInfo<TType>[] SerializeProperties;
        /// <summary>
        /// Array of all properties to deserialize
        /// </summary>
        public BasePropertyGoInfo<TType>[] DeserializeProperties;
        ///// <summary>
        ///// Generic types
        ///// </summary>
        //public List<TypeGoInfo> Generics { get; set; } = new List<TypeGoInfo>();

    }
}
