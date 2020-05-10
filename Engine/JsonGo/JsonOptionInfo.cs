using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo
{
    public delegate bool TryGetValue<TKey, TResult>(TKey key, out TResult result);
    /// <summary>
    /// default saved data of serialization and deserialization
    /// </summary>
    public class JsonOptionInfo
    {
        //internal JsonOptionInfo()
        //{
        //    TryGetValueOfTypeGo = Types.TryGetValue;
        //    TryGetValueOfSerializedObjects = SerializedObjects.TryGetValue;
        //    ClearSerializedObjects = SerializedObjects.Clear;
        //    AddSerializedObjects = SerializedObjects.Add;
        //    AddTypes = Types.Add;
        //}

        ///// <summary>
        ///// add new value to types
        ///// </summary>
        //internal Action<Type, TypeGoInfo> AddTypes { get; set; }
        ///// <summary>
        ///// add new serilized object to memory
        ///// </summary>
        //internal Action<object ,int> AddSerializedObjects { get; set; }
        ///// <summary>
        ///// clear Serialized Objects
        ///// </summary>
        //internal Action ClearSerializedObjects { get; set; }
        ///// <summary>
        ///// get typefo value from 
        ///// </summary>
        //internal TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        ///// <summary>
        ///// get value from serializedobjects
        ///// </summary>
        //internal TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
        /// <summary>
        /// chached types
        /// </summary>
        internal Dictionary<Type, TypeGoInfo> Types { get; set; } = new Dictionary<Type, TypeGoInfo>();
        /// <summary>
        /// save serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        internal Dictionary<object, int> SerializedObjects { get; set; } = new Dictionary<object, int>();

        public bool IsGenerateLoopReference { get; set; }
    }
}
