using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        /// Save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
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
            try
            {
                var dataType = typeof(T);
                if (!TryGetValueOfTypeGo(dataType, out object typeGoInfo))
                {
                    typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);
                }
                var binaryReader = new BinarySpanReader(reader);
                return ((TypeGoInfo<T>)typeGoInfo).BinaryDeserialize(ref binaryReader);
            }
            finally
            {
                DeSerializedObjects.Clear();
            }
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
            foreach (var model in newStructureModels)
            {
                bool hasChanged = false;
                (Type Type, BaseTypeGoInfo TypeGo) = FindType(model);
                foreach (var property in TypeGo.InternalProperties)
                {
                    if (!model.Properties.Any(x => x.Name == property.Key))
                    {
                        hasChanged = true;
                        TypeGo.RemoveProperty(property.Key);
                    }
                }

                foreach (var property in model.Properties)
                {
                    if (!TypeGo.InternalProperties.Any(x => x.Key == property.Name))
                    {
                        hasChanged = true;
                        var instance = Activator.CreateInstance(typeof(PropertyGoInfo<,>)
                            .MakeGenericType(TypeGo.Type, GetTypeOfProperty(property)), null, Options);
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
        public string GetStrcutureModelName(Type type)
        {
            return $"{Path.GetFileName(type.Assembly.Location)} {type.Namespace}.{type.Name}";
        }

        Type GetTypeOfProperty(MemberBinaryModelInfo memberBinaryModel)
        {
            var fullName = memberBinaryModel.ResultType.GetFullName();
            var find = ReflectionHelper.VariableTypes.FirstOrDefault(x => x.Value == fullName);
            if (find.Value == null)
                throw new Exception($"Property of type {fullName} not found!");
            return find.Key;
        }

        (Type Type, BaseTypeGoInfo TypeGo) FindType(BinaryModelInfo binaryModel)
        {
            string modelFullName = binaryModel.ToString();
            MovedTypes.TryGetValue(modelFullName, out Type type);
            foreach (var item in Options.Types)
            {
                var typeFullName = GetStrcutureModelName(item.Key);
                if (typeFullName == modelFullName || item.Key == type)
                    return (item.Key, (BaseTypeGoInfo)item.Value);
            }
            throw new Exception($"I cannot find {modelFullName} did you initialzie it before use?");
        }
    }
}

