using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Interfaces
{
    /// <summary>
    /// an interface tomake initialize method for typego
    /// </summary>
    public interface ISerializationVariable
    {
        /// <summary>
        /// initialize yout variable to typeGo
        /// </summary>
        /// <param name="typeGoInfo">typego</param>
        /// <param name="options">options or settings of variable serializer</param>
        void Initialize(TypeGoInfo typeGoInfo, ITypeGo options);
    }
}
