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
        /// name of property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// get value of property
        /// </summary>
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
    }
}
