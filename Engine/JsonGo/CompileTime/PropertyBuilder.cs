using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// Base property
    /// </summary>
    public abstract class PropertyInfoBase
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets the property's value
        /// </summary>
        public Func<object, object> GetValue { get; set; }
        /// <summary>
        /// Set value
        /// </summary>
        public Action<object, object> SetValue { get; set; }

    }

    /// <summary>
    /// Type's property info
    /// </summary>
    public class PropertyInfo : PropertyInfoBase
    {
        /// <summary>
        /// Get value
        /// </summary>
        public Action<StringBuilder> Serialize { get; set; }
    }
}
