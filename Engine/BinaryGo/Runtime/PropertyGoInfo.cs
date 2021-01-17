using BinaryGo.Binary.Deserialize;
using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Json.Deserialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BinaryGo.Runtime
{
    /// <summary>
    /// delegate for get property value of objects
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate TProperty GetPropertyValue<TObject, TProperty>(ref TObject obj);

    /// <summary>
    /// Generates type details in memory
    /// </summary>
    public class PropertyGoInfo<TObject, TPropertyType> : BasePropertyGoInfo<TObject>
    {
        /// <summary>
        /// property info of a type
        /// </summary>
        public PropertyGoInfo(PropertyInfo property, ITypeOptions options)
        {
            //coming from change structure
            if (property == null)
            {
                if (options.TryGetValueOfTypeGo(typeof(TPropertyType), out object typeGoInfoProperty))
                {
                    TypeGoInfo = (TypeGoInfo<TPropertyType>)typeGoInfoProperty;
                }
                else
                {
                    TypeGoInfo = BaseTypeGoInfo.Generate<TPropertyType>(options);
                }
                GetValue = (ref TObject obj) => default;
                SetValue = (TObject obj, TPropertyType value) => { };
            }
            //coming from normal binary Go
            else
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
            Type = typeof(TPropertyType);
        }

        /// <summary>
        /// default value with type safed
        /// </summary>
        public TPropertyType TypedDefaultValue = default;
        /// <summary>
        /// Current TypeGoInfo mirror of property type
        /// </summary>
        public TypeGoInfo<TPropertyType> TypeGoInfo;
        /// <summary>
        /// Gets property value
        /// </summary>
        public GetPropertyValue<TObject, TPropertyType> GetValue;
        /// <summary>
        /// Set value of property
        /// </summary>
        public Action<TObject, TPropertyType> SetValue;

        ///// <summary>
        ///// Gets property value
        ///// </summary>
        //public JsonGetValue<TProperty, TObject> JsonGetValue { get; set; }
        ///// <summary>
        ///// Sets property value
        ///// </summary>
        //public Action<JsonDeserializer, TObject, TProperty> JsonSetValue { get; set; }
        //internal override object InternalGetValue(ref TObject instance)
        //{
        //    return GetValue(instance);
        //}

        //internal override void JsonSerialize(ref JsonSerializeHandler handler, ref object value)
        //{
        //    var unboxedValue = (TPropertyType)value;
        //    TypeGoInfo.JsonSerialize(ref handler, ref unboxedValue);
        //}


        internal override void TypedJsonSerialize(ref JsonSerializeHandler handler, ref TObject value)
        {
            TPropertyType propertyValue = GetValue(ref value);
            if (propertyValue == null || propertyValue.Equals(DefaultValue))
                return;
            handler.TextWriter.Write(NameSerialized);
            TypeGoInfo.JsonSerialize(ref handler, ref propertyValue);
            handler.TextWriter.Write(JsonConstantsString.Comma);
        }

        //internal override void InternalSetValue(ref TObject instance, ref object value)
        //{
        //    SetValue(instance, (TPropertyType)value);
        //}

        internal override void JsonDeserializeString(ref TObject instance, ref JsonSpanReader reader)
        {
            var extract = reader.ExtractString();
            SetValue(instance, TypeGoInfo.JsonDeserialize(ref extract));
        }

        internal override void JsonDeserializeObject(ref TObject instance, ref JsonDeserializer deserializer, ref JsonSpanReader reader)
        {
            var deserializedObject = FastDeserializerExtractor<TPropertyType>.ExtractOject(ref deserializer, ref TypeGoInfo, ref reader);
            SetValue(instance, deserializedObject);
        }

        internal override void JsonDeserializeArray(ref TObject instance, ref JsonDeserializer deserializer, ref JsonSpanReader reader)
        {

        }
        /// <summary>
        /// json deserialize values of number or bool
        /// </summary>
        /// <param name="instance">instance of property to set value</param>
        /// <param name="reader">json text reader</param>
        internal override void JsonDeserializeValue(ref TObject instance, ref JsonSpanReader reader)
        {
            var extract = reader.ExtractValue();
            SetValue(instance, TypeGoInfo.JsonDeserialize(ref extract));
        }

        internal override void BinarySerialize(ref BufferBuilder stream, ref TObject value)
        {
            TPropertyType propertyValue = GetValue(ref value);
            TypeGoInfo.BinarySerialize(ref stream, ref propertyValue);
        }

        internal override void BinaryDeserialize(ref BinarySpanReader reader, ref TObject value)
        {
            SetValue(value, TypeGoInfo.BinaryDeserialize(ref reader));
        }

        /// <summary>
        /// Get binary member
        /// </summary>
        /// <returns></returns>
        internal override MemberBinaryModelInfo GetBinaryMember(BaseOptionInfo option, Dictionary<Type, BinaryModelInfo> generatedModels)
        {
            MemberBinaryModelInfo memberBinaryModelInfo = new MemberBinaryModelInfo()
            {
                Name = Name,
                ResultType = BinaryModelInfo.GetBinaryModel(TypeGoInfo, option, generatedModels),
                CanRead = true,
                CanWrite = true,
                Type = MemberBinaryModelType.Property
            };

            return memberBinaryModelInfo;
        }

        //internal override void BinarySerialize(ref BufferBuilder stream, ref object value)
        //{
        //    var unboxedValue = (TPropertyType)value;
        //    TypeGoInfo.BinarySerialize(ref stream, ref unboxedValue);
        //}

        //internal override object BinaryDeserialize(ref BinarySpanReader reader)
        //{
        //    return TypeGoInfo.BinaryDeserialize(ref reader);
        //}
    }
}
