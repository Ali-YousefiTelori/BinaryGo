using BinaryGo.Binary.StructureModels;
using BinaryGo.Runtime.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryGo.Runtime
{
    /// <summary>
    /// Generate type details in memory
    /// </summary>
    public class TypeGoInfo<TType> : BaseTypeGoInfo
    {
        /// <summary>
        /// Default value of object
        /// </summary>
        public TType DefaultValue;
        /// <summary>
        /// If the type is simple like int, byte, bool, enum it can be serialized without quotes
        /// </summary>
        public bool IsNoQuotesValueType = true;
        /// <summary>
        /// Serialize action as text
        /// </summary>
        public JsonActionGo<TType> JsonSerialize;
        /// <summary>
        /// Binary serialize
        /// </summary>
        public BinaryFunctionGo<TType> BinarySerialize;
        /// <summary>
        /// Deserializes binary to object
        /// </summary>
        public BinaryDeserializeFunc<TType> BinaryDeserialize;
        /// <summary>
        /// Deserializes string to object
        /// </summary>
        public DeserializeFunc<TType> JsonDeserialize;
        /// <summary>
        /// Creates type instance
        /// </summary>
        public Func<TType> CreateInstance;
        /// <summary>
        /// Casts to real object
        /// </summary>
        public Func<object, TType> Cast;
        /// <summary>
        /// Adds array value to TypeGo array
        /// </summary>
        public Action<List<TType>, TType> AddArrayValue;

        /// <summary>
        /// Type properties 
        /// </summary>
        public Dictionary<string, BasePropertyGoInfo<TType>> Properties;
        /// <summary>
        /// Array of all properties to serialize
        /// </summary>
        public BasePropertyGoInfo<TType>[] SerializeProperties;
        /// <summary>
        /// Array of all properties to deserialize
        /// </summary>
        public BasePropertyGoInfo<TType>[] DeserializeProperties;

        #region Structure Changes

        internal override Dictionary<string, BaseTypeGoInfo> InternalProperties
        {
            get
            {
                return Properties.ToDictionary(x => x.Key, x => x.Value.BaseTypeGoInfo);
            }
        }

        internal override void AddProperty(string propertyName, object propertyGoInfo)
        {
            Properties.Add(propertyName, (BasePropertyGoInfo<TType>)propertyGoInfo);
        }

        internal override void RemoveProperty(string propertyName)
        {
            Properties.Remove(propertyName);
        }

        internal override void ReGenerateProperties(List<MemberBinaryModelInfo> properties)
        {
            ObjectVariable<TType> objectVariable = (ObjectVariable<TType>)Variable;
            objectVariable.RebuildProperties(properties);
            Generateproperties();
        }

        internal void Generateproperties()
        {
            SerializeProperties = Properties.Values.ToArray();
            DeserializeProperties = Properties.Values.ToArray();
        }

        #endregion
    }
}
