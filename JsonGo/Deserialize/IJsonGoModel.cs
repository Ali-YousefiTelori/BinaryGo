using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Deserialize
{
    /// <summary>
    /// json go model before deserialize
    /// </summary>
    public interface IJsonGoModel
    {
        /// <summary>
        /// generate type to object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        object Generate(Type type, Deserializer deserializer);
        /// <summary>
        /// add property,item,value to model
        /// </summary>
        /// <param name="nameOrValue"></param>
        /// <param name="value"></param>
        void Add(string nameOrValue, IJsonGoModel value);
    }
}
