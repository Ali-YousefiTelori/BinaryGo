﻿using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryGo.Binary.Deserialize
{
    /// <summary>
    /// Json deserializer
    /// </summary>
    public class BinaryDeserializer : ISerializer
    {
        /// <summary>
        /// Serialization's default options
        /// </summary>
        public static BaseOptionInfo DefaultOptions { get; set; } = new BaseOptionInfo();
        /// <summary>
        /// 
        /// </summary>
        public BinaryDeserializer()
        {
            Options = DefaultOptions;
        }

        BaseOptionInfo _Options;
        /// <summary>
        /// Adds new value to types
        /// </summary>
        public Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// Gets typefo value 
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }

        /// <summary>
        /// options and cached data for serialize and deserialize
        /// cached data like types
        /// </summary>
        public BaseOptionInfo Options
        {
            get
            {
                return _Options;
            }
            set
            {
                _Options = value;
                AddTypes = _Options.Types.Add;
                TryGetValueOfTypeGo = _Options.Types.TryGetValue;
            }
        }

        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static BinaryDeserializer NormalInstance
        {
            get
            {
                return new BinaryDeserializer();
            }
        }

        /// <summary>
        /// Support for types' loop reference
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Deserializes a stream to a type
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="reader">SpanReader binary to deserialize</param>
        /// <returns>deserialized type</returns>
        public T Deserialize<T>(ReadOnlySpan<byte> reader)
        {
            Type dataType = typeof(T);
            if (!TryGetValueOfTypeGo(dataType, out object typeGoInfo))
            {
                typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);
            }
            BinarySpanReader binaryReader = new BinarySpanReader(reader);
            return ((TypeGoInfo<T>)typeGoInfo).BinaryDeserialize(ref binaryReader);
        }

        Dictionary<string, Type> MovedTypes { get; set; } = new Dictionary<string, Type>();
        /// <summary>
        /// add a type as renamed
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="type"></param>
        public void AddMovedType(string fullName, Type type)
        {
            MovedTypes[fullName] = type;
        }

        /// <summary>
        /// Build your new structure to deserialize structure
        /// </summary>
        /// <param name="newStructureModels"></param>
        public void BuildStructure(List<BinaryModelInfo> newStructureModels)
        {
            foreach (BinaryModelInfo model in newStructureModels)
            {
                bool hasChanged = false;
                (Type Type, BaseTypeGoInfo TypeGo) = FindType(model);
                var properties = TypeGo.GetInternalProperties(Options);
                foreach (KeyValuePair<string, BaseTypeGoInfo> property in properties)
                {
                    //property removes
                    if (!model.Properties.Any(x => x.Name == property.Key))
                    {
                        hasChanged = true;
                        TypeGo.RemoveProperty(property.Key);
                    }
                }

                foreach (MemberBinaryModelInfo property in model.Properties)
                {
                    //property adds
                    if (!properties.Any(x => x.Key == property.Name))
                    {
                        hasChanged = true;
                        object instance = Activator.CreateInstance(typeof(PropertyGoInfo<,>)
                            .MakeGenericType(TypeGo.Type, GetTypeOfProperty(property)), null, Options);
                        TypeGo.AddProperty(property.Name, instance);
                    }
                    //Property type has change
                    else if (properties.Any(x => (ReflectionHelper.VariableTypes.ContainsKey(x.Value.Type) || ReflectionHelper.VariableTypes.ContainsValue(property.ResultType.GetFullName())) && x.Key == property.Name && GetStrcutureModelName(x.Value.Type) != property.ResultType.ToString()))
                    {
                        hasChanged = true;
                        object instance = Activator.CreateInstance(typeof(PropertyGoInfo<,>)
                            .MakeGenericType(TypeGo.Type, GetTypeOfProperty(property)), null, Options);
                        TypeGo.RemoveProperty(property.Name);
                        TypeGo.AddProperty(property.Name, instance);
                    }
                }


                if (hasChanged)
                {
                    TypeGo.ReGenerateProperties(model.Properties);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetStrcutureModelName(Type type)
        {
            return $"{Path.GetFileName(type.Assembly.Location)} {type.Namespace}.{type.Name}";
        }

        Type GetTypeOfProperty(MemberBinaryModelInfo memberBinaryModel)
        {
            string fullName = memberBinaryModel.ResultType.GetFullName();
            KeyValuePair<Type, string> find = ReflectionHelper.VariableTypes.FirstOrDefault(x => x.Value == fullName);
            if (find.Value == null)
            {
                if (MovedTypes.TryGetValue(memberBinaryModel.ResultType.ToString(), out Type type))
                    return type;
                throw new Exception($"Property of type {fullName} not found!");
            }
            return find.Key;
        }

        (Type Type, BaseTypeGoInfo TypeGo) FindType(BinaryModelInfo binaryModel)
        {
            string modelFullName = binaryModel.ToString();
            MovedTypes.TryGetValue(modelFullName, out Type type);
            foreach (KeyValuePair<Type, object> item in Options.Types)
            {
                string typeFullName = GetStrcutureModelName(item.Key);
                if (typeFullName == modelFullName || item.Key == type)
                    return (item.Key, (BaseTypeGoInfo)item.Value);
            }
            throw new Exception($"I cannot find {modelFullName} did you initialzie it before use?");
        }
    }
}

