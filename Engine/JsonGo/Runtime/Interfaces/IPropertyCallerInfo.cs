using JsonGo.Json.Deserialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime.Interfaces
{
    /// <summary>
    /// property get and set value helper
    /// </summary>
    public interface IPropertyCallerInfo
    {
        /// <summary>
        /// Gets the property value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object GetPropertyValue(object instance);
        /// <summary>
        /// Sets the property value
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        void SetPropertyValue(JsonDeserializer deserializer, object instance, object value);
    }
}
