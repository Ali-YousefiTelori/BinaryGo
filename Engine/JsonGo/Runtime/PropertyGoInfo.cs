using JsonGo.Deserialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// generate type details on memory
    /// </summary>
    public class PropertyGoInfo
    {
        /// <summary>
        /// type of property
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// current TypeGoInfo of property type
        /// </summary>
        public TypeGoInfo TypeGoInfo { get; set; }
        /// <summary>
        /// name of property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// get value of property
        /// </summary>
        public Func<Serializer, object, object> GetValue { get; set; }
        /// <summary>
        /// set value of property
        /// </summary>
        public Action<Deserializer, object, object> SetValue { get; set; }
    }
}
