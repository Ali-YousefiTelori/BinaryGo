using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// manage types
    /// </summary>
    public class TypeManager
    {
        static TypeManager()
        {
            Current = new TypeManager();
        }

        /// <summary>
        /// current type manager
        /// </summary>
        public static TypeManager Current { get; private set; }
        /// <summary>
        /// compiled types in memory
        /// </summary>
        public static Dictionary<Type, TypeInfo> CompiledTypes { get; set; } = new Dictionary<Type, TypeInfo>();
        /// <summary>
        /// add a type
        /// </summary>
        /// <param name="typeInfo"></param>
        public void Add(TypeInfo typeInfo)
        {
            CompiledTypes.Add(typeInfo.Type, typeInfo);
        }
        /// <summary>
        /// get type info
        /// </summary>
        /// <returns></returns>
        public TypeInfo<T> Get<T>()
        {
            return (TypeInfo<T>)CompiledTypes[typeof(T)];
        }
    }
}
