using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// details of a type
    /// </summary>
    public class TypeInfo
    {
        /// <summary>
        /// type of builder
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// all of generic arguments
        /// </summary>
        public List<TypeInfo> GenericArguments { get; set; } = new List<TypeInfo>();
        /// <summary>
        /// create instance of type
        /// </summary>
        public Func<object> CreateInstanceFunction { get; set; }
        public Action<Serializer, StringBuilder, object> DynamicSerialize { get; set; }

    }

    /// <summary>
    /// details of a type
    /// </summary>
    public class TypeInfo<T> : TypeInfo
    {
        /// <summary>
        /// all of properties of type
        /// </summary>
        public Dictionary<string, PropertyInfoBase> Properties { get; set; } = new Dictionary<string, PropertyInfoBase>();
        /// <summary>
        /// serialize object directly
        /// </summary>
        public static Action<Serializer, StringBuilder, T> Serialize { get; set; }
    }
}
