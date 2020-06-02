using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// Type's details
    /// </summary>
    public class TypeInfo
    {
        /// <summary>
        /// Builder type
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// List with all generic arguments
        /// </summary>
        public List<TypeInfo> GenericArguments { get; set; } = new List<TypeInfo>();
        /// <summary>
        /// Createa type instance
        /// </summary>
        public Func<object> CreateInstanceFunction { get; set; }
        /// <summary>
        /// Dynamic serializer function
        /// </summary>
        public Action<Serializer, StringBuilder, object> DynamicSerialize { get; set; }

    }

    /// <summary>
    /// Type details
    /// </summary>
    public class TypeInfo<T> : TypeInfo
    {
        /// <summary>
        /// Dictionary with all type properties
        /// </summary>
        public Dictionary<string, PropertyInfoBase> Properties { get; set; } = new Dictionary<string, PropertyInfoBase>();
        /// <summary>
        /// Serialize object directly
        /// </summary>
        public static Action<Serializer, StringBuilder, T> Serialize { get; set; }
    }
}
