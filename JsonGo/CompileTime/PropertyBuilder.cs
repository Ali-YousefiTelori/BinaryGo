using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// base of property bu
    /// </summary>
    public abstract class PropertyInfoBase
    {
        /// <summary>
        /// name of property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// get Value
        /// </summary>
        public Func<object, object> GetValue { get; set; }
        /// <summary>
        /// set Value
        /// </summary>
        public Action<object, object> SetValue { get; set; }

    }

    /// <summary>
    /// property info of type
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    public class PropertyInfo : PropertyInfoBase
    {
        /// <summary>
        /// get Value
        /// </summary>
        public Action<StringBuilder> Serialize { get; set; }
    }
}
