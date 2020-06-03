using JsonGo.Json.Deserialize;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// Generates type details in memory
    /// </summary>
    public class PropertyGoInfo
    {
        /// <summary>
        /// Property type
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Current TypeGoInfo mirror of property type
        /// </summary>
        public TypeGoInfo TypeGoInfo { get; set; }
        /// <summary>
        /// Property Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets property value
        /// </summary>
        public Func<object, object> GetValue { get; set; }
        /// <summary>
        /// Set value of property
        /// </summary>
        public Action<object, object> SetValue { get; set; }
        /// <summary>
        /// Gets property value
        /// </summary>
        public Func<JsonSerializeHandler, object, object> JsonGetValue { get; set; }
        /// <summary>
        /// Sets property value
        /// </summary>
        public Action<JsonDeserializer, object, object> JsonSetValue { get; set; }
    }
}
