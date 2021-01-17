using BinaryGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGo.CompileTime
{
    /// <summary>
    /// This class builds your types 
    /// </summary>
    public class TypeBuilder<T>
    {
        /// <summary>
        /// Created instance function
        /// </summary>
        private Func<object> CreateInstanceFunction { get; set; }
        /// <summary>
        /// Serialize func
        /// </summary>
        private Action<Serializer, StringBuilder, T> SerializeFunction { get; set; }
        private Action<Serializer, StringBuilder, object> DynamicSerializeFunction { get; set; }
        /// <summary>
        /// Dictionary with all properties' type. Key: name, Value: type as PropertyInfoBase
        /// </summary>
        public Dictionary<string, PropertyInfoBase> Properties { get; set; } = new Dictionary<string, PropertyInfoBase>();
        /// <summary>
        /// List with every generic argument
        /// </summary>
        public List<TypeInfo> GenericArguments { get; set; } = new List<TypeInfo>();
        /// <summary>
        /// This function creates object's instance
        /// </summary>
        /// <param name="createInstance"></param>
        /// <returns></returns>
        public TypeBuilder<T> CreateInstance(Func<object> createInstance)
        {
            CreateInstanceFunction = createInstance;
            return this;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public static TypeBuilder<T> Create()
        {
            return new TypeBuilder<T>();
        }



        ///<summary>
        /// Gets property type and returns the proper TypeBuilder       
        ///</summary>
        /// <returns></returns>
        public TypeBuilder<T> SerializeObject(Action<Serializer, StringBuilder, T> serialize)
        {
            SerializeFunction = serialize;
            return this;
        }

        /// <summary>
        /// Type on
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public TypeBuilder<T> On<T2>()
        {
            var type = TypeBuilder<T2>.Create();
            type.DynamicSerializeFunction = (serializer, builder, obj) =>
            {
                SerializeFunction(serializer, builder, (T)obj);
            };
            type.Build();
            return this;
        }
        /// <summary>
        /// Gets a generic argument type and returns the proper TypeBuilder
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public TypeBuilder<T> AddGenericArgument(TypeInfo typeInfo)
        {
            GenericArguments.Add(typeInfo);
            return this;
        }

        /// <summary>
        /// Builds the TypeBuilder
        /// </summary>
        public TypeBuilder<T> Build()
        {
            TypeInfo<T> typeInfo = new TypeInfo<T>()
            {
                Type = typeof(T),
                CreateInstanceFunction = CreateInstanceFunction,
                Properties = Properties,
                GenericArguments = GenericArguments,
                DynamicSerialize = DynamicSerializeFunction
            };
            TypeInfo<T>.Serialize = SerializeFunction;
            TypeManager.Current.Add(typeInfo);
            return this;
        }
    }
}
