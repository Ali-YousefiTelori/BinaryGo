using BinaryGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BinaryGo.Helpers
{
    /// <summary>
    /// Default's saved data of serialization and deserialization
    /// </summary>
    public class BaseOptionInfo : ITypeOptions
    {
        /// <summary>
        /// Set default values from dictionary to actions
        /// </summary>
        public BaseOptionInfo()
        {
            Encoding = Encoding.UTF8;
            CurrentCulture = new CultureInfo("en-US");
            AddToCustomTypes<ICollection, Array>();
            AddToCustomTypes<IEnumerable, Array>();
            AddToCustomTypes<IList, Array>();
            AddToCustomTypes(typeof(IEnumerable<>), typeof(List<>));
            AddToCustomTypes(typeof(ICollection<>), typeof(List<>));
            AddToCustomTypes(typeof(IList<>), typeof(List<>));
            AddTypes = Types.Add;
            TryGetValueOfTypeGo = Types.TryGetValue;
        }
        /// <summary>
        /// Encoding of type go options
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// Current culture
        /// </summary>
        public CultureInfo CurrentCulture { get; set; }
        /// <summary>
        /// lock object to pervent cocurrent initializer call
        /// </summary>
        public object LockObject { get; set; } = new object();
        /// <summary>
        /// Cached types
        /// </summary>
        public Dictionary<Type, object> Types { get; set; } = new Dictionary<Type, object>();
        /// <summary>
        /// Saves serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        internal Dictionary<object, int> SerializedObjects { get; set; } = new Dictionary<object, int>();

        /// <summary>
        /// custome types
        /// </summary>
        public Dictionary<Type, Type> CustomTypeChanges { get; set; } = new Dictionary<Type, Type>();

        /// <summary>
        /// Loop reference generation
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Adds type to typeGo dictionary for faster access
        /// </summary>
        public Action<Type, object> AddTypes { get; set; }

        /// <summary>
        /// Gets TypeGo value from a specific type
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }

        /// <summary>
        /// Add types or interfaces to automatic custom type
        /// </summary>
        /// <typeparam name="TCustomType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public void AddToCustomTypes<TCustomType, TResult>()
        {
            AddToCustomTypes(typeof(TCustomType), typeof(TResult));
        }

        /// <summary>
        /// Adds types or interfaces to automatic custom type
        /// </summary>
        public void AddToCustomTypes(Type type, Type result)
        {
            CustomTypeChanges[type] = result;
        }
    }
}
