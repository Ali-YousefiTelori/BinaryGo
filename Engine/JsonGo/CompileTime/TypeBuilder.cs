using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.CompileTime
{
    /// <summary>
    /// build your types 
    /// </summary>
    public class TypeBuilder<T>
    {
        /// <summary>
        /// created insatnce function
        /// </summary>
        private Func<object> CreateInstanceFunction { get; set; }
        /// <summary>
        /// serialize func
        /// </summary>
        private Action<Serializer, StringBuilder, T> SerializeFunction { get; set; }
        /// <summary>
        /// all of properties of type
        /// </summary>
        public Dictionary<string, PropertyInfoBase> Properties { get; set; } = new Dictionary<string, PropertyInfoBase>();
        /// <summary>
        /// all of generic arguments
        /// </summary>
        public List<TypeInfo> GenericArguments { get; set; } = new List<TypeInfo>();
        /// <summary>
        /// function of create instance of object
        /// </summary>
        /// <param name="createInstance"></param>
        /// <returns></returns>
        public TypeBuilder<T> CreateInstance(Func<object> createInstance)
        {
            CreateInstanceFunction = createInstance;
            return this;
        }

        public static TypeBuilder<T> Create()
        {
            return new TypeBuilder<T>();
        }

        /// <summary>
        /// add property to type
        /// </summary>
        /// <returns></returns>
        public TypeBuilder<T> SerializeObject(Action<Serializer, StringBuilder, T> serialize)
        {
            SerializeFunction = serialize;
            return this;
        }
        public TypeBuilder<T> AddGenericArgument(TypeInfo typeInfo)
        {
            GenericArguments.Add(typeInfo);
            return this;
        }

        /// <summary>
        /// build a type
        /// </summary>
        public TypeInfo<T> Build()
        {
            TypeInfo<T> typeInfo = new TypeInfo<T>()
            {
                Type = typeof(T),
                CreateInstanceFunction = CreateInstanceFunction,
                Properties = Properties,
                GenericArguments = GenericArguments,
            };
            TypeInfo<T>.Serialize = SerializeFunction;
            TypeManager.Current.Add(typeInfo);
            return typeInfo;
        }
    }
}
