using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryGo.Runtime.Variables
{
    /// <summary>
    /// base initializer of variables
    /// </summary>
    public abstract class BaseVariable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public BaseVariable(Type type)
        {
            Type = type;
        }
        /// <summary>
        /// type of variable
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// default options of variable type go
        /// </summary>
        public ITypeOptions Options { get; set; }
        /// <summary>
        /// Adds new value to types
        /// </summary>
        public Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value from 
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// Support for loop refereence serialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }
        /// <summary>
        /// CurrentCulture of serializer
        /// </summary>
        public CultureInfo CurrentCulture { get; set; }
        /// <summary>
        /// Initialize variable to typeGo
        /// </summary>
        /// <param name="typeGoInfo">TypeGo</param>
        /// <param name="options">Serializer's options or settings</param>
        public void InitializeBase(BaseTypeGoInfo typeGoInfo, ITypeOptions options)
        {
            typeGoInfo.Variable = this;
            CurrentCulture = options.CurrentCulture;
            Options = options;

            AddTypes = options.AddTypes;
            TryGetValueOfTypeGo = options.TryGetValueOfTypeGo;
            HasGenerateRefrencedTypes = options.HasGenerateRefrencedTypes;

            Type thisType = GetType();
            MethodInfo findMethod = thisType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name == "Initialize").FirstOrDefault();
            if (findMethod == null)
                throw new Exception($"I tried to find method Initialize in type of {thisType.FullName} but not found this method pls add it");
            findMethod.Invoke(this, new object[] { typeGoInfo, options });
        }

        ///// <summary>
        ///// Initalizes TypeGo variable
        ///// </summary>
        ///// <param name="typeGoInfo">TypeGo variable to initialize</param>
        ///// <param name="options">Serializer or deserializer options</param>
        //public virtual void Initialize<T>(TypeGoInfo<T> typeGoInfo, ITypeGo options)
        //{
        //    Initialize((BaseTypeGoInfo)typeGoInfo, options);
        //}

        ///// <summary>
        ///// Initalizes TypeGo variable
        ///// </summary>
        ///// <param name="baseTypeGoInfo">TypeGo variable to initialize</param>
        ///// <param name="options">Serializer or deserializer options</param>
        //public virtual void Initialize(BaseTypeGoInfo baseTypeGoInfo, ITypeGo options)
        //{

        //}
    }
}
