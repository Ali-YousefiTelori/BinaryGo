using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// Manages types
    /// </summary>
    public class TypeManager
    {
        static TypeManager()
        {
            Current = new TypeManager();
        }

        /// <summary>
        /// Current type manager
        /// </summary>
        public static TypeManager Current { get; private set; }
        /// <summary>
        /// Compiled types in memory
        /// </summary>
        public static Dictionary<Type, TypeInfo> CompiledTypes { get; set; } = new Dictionary<Type, TypeInfo>();
        /// <summary>
        /// Add a type
        /// </summary>
        /// <param name="typeInfo"></param>
        public void Add(TypeInfo typeInfo)
        {
            CompiledTypes.Add(typeInfo.Type, typeInfo);
        }
        /// <summary>
        /// Gets type info
        /// </summary>
        /// <returns></returns>
        public TypeInfo<T> Get<T>()
        {
            return (TypeInfo<T>)CompiledTypes[typeof(T)];
        }
    }
}
