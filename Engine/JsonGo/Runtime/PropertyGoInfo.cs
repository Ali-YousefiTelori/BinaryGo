using JsonGo.Binary.Deserialize;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.IO;
using System.Reflection;

namespace JsonGo.Runtime
{
    /// <summary>
    /// Generates type details in memory
    /// </summary>
    public class PropertyGoInfo<TObject, TPropertyType> : BasePropertyGoInfo<TObject>
    {
        /// <summary>
        /// property info of a type
        /// </summary>
        public PropertyGoInfo(PropertyInfo property, ITypeGo options)
        {
            if (options.TryGetValueOfTypeGo(property.PropertyType, out object typeGoInfoProperty))
            {
                TypeGoInfo = (TypeGoInfo<TPropertyType>)typeGoInfoProperty;
            }
            else
            {
                TypeGoInfo = BaseTypeGoInfo.Generate<TPropertyType>(options);
            }

            PropertyCallerInfo<TObject, TPropertyType> propertyCaller;

            try
            {
                propertyCaller = ReflectionHelper.GetDelegateInstance<TObject, TPropertyType>(property);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot create delegate for property {property.Name} in type {TypeGoInfo.Type.FullName}", ex);
            }
            GetValue = propertyCaller.GetValueAction;
            SetValue = propertyCaller.SetValueAction;
        }

        /// <summary>
        /// Current TypeGoInfo mirror of property type
        /// </summary>
        public TypeGoInfo<TPropertyType> TypeGoInfo { get; set; }
        /// <summary>
        /// Gets property value
        /// </summary>
        public Func<TObject, TPropertyType> GetValue { get; set; }
        /// <summary>
        /// Set value of property
        /// </summary>
        public Action<TObject, TPropertyType> SetValue { get; set; }

        ///// <summary>
        ///// Gets property value
        ///// </summary>
        //public JsonGetValue<TProperty, TObject> JsonGetValue { get; set; }
        ///// <summary>
        ///// Sets property value
        ///// </summary>
        //public Action<JsonDeserializer, TObject, TProperty> JsonSetValue { get; set; }
        internal override object InternalGetValue(ref TObject instance)
        {
            return GetValue(instance);
        }

        internal override void JsonSerialize(ref JsonSerializeHandler handler, ref object value)
        {
            var unboxedValue = (TPropertyType)value;
            TypeGoInfo.JsonSerialize(ref handler, ref unboxedValue);
        }

        internal override void InternalSetValue(ref TObject instance, ref object value)
        {
            SetValue(instance, (TPropertyType)value);
        }

        internal override object JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            throw new NotImplementedException();
        }

        internal override void BinarySerialize(ref BufferBuilder<byte> stream, ref object value)
        {
            var unboxedValue = (TPropertyType)value;
            TypeGoInfo.BinarySerialize(ref stream, ref unboxedValue);
        }

        internal override object BinaryDeserialize(ref BinarySpanReader reader)
        {
            return TypeGoInfo.BinaryDeserialize(ref reader);
        }
    }
}
