using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Deserialize
{
    /// <summary>
    /// instade of using recursive function i used manualy stack to make better performance
    /// </summary>
    public struct StackReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringBuilder"></param>
        public StackReader(StringBuilder stringBuilder) : this()
        {
            //Builder = stringBuilder;
        }
        public StackReader? Parent { get; set; }
        public StringBuilder Builder { get; set; }
    }
}
