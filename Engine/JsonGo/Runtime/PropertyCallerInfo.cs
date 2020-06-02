using JsonGo.Json.Deserialize;
using JsonGo.Runtime.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// Property value caller
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    public class PropertyCallerInfo<TType, TPropertyType> : IPropertyCallerInfo
    {
        /// <summary>
        /// Property value caller
        /// </summary>
        public PropertyCallerInfo(Func<TType, TPropertyType> funcGetValue, Action<TType, TPropertyType> funcSetValue)
        {
            GetValue = funcGetValue;
            SetValue = funcSetValue;
        }
        /// <summary>
        /// Gets property value
        /// </summary>
        public Func<TType, TPropertyType> GetValue { get; set; }
        /// <summary>
        /// Sets property value
        /// </summary>
        public Action<TType, TPropertyType> SetValue { get; set; }
        /// <summary>
        /// Gets property value of an object
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetPropertyValue(object instance)
        {
            return GetValue((TType)instance);
        }

        /// <summary>
        /// Sets property value of an object
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(JsonDeserializer deserializer, object instance, object value)
        {
            SetValue((TType)instance, (TPropertyType)value);
        }
    }
}
