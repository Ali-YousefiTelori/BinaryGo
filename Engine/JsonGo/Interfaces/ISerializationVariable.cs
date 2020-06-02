using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Interfaces
{
    /// <summary>
    /// This interface is needed to initialize method for TypeGo
    /// </summary>
    public interface ISerializationVariable
    {
        /// <summary>
        /// Initialize variable to typeGo
        /// </summary>
        /// <param name="typeGoInfo">TypeGo</param>
        /// <param name="options">Serializer's options or settings</param>
        void Initialize(TypeGoInfo typeGoInfo, ITypeGo options);
    }
}
