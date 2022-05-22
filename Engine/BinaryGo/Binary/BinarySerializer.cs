using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryGo.Binary
{
    /// <summary>
    /// BinaryGo binary serializer: serializes your object to byte array or memory stream
    /// </summary>
    public class BinarySerializer : ISerializer
    {
        static BinarySerializer()
        {
            BaseTypeGoInfo.GenerateDefaultVariables(DefaultOptions);
        }

        BaseOptionInfo _Options;
        /// <summary>
        /// 
        /// </summary>
        public static BaseOptionInfo DefaultOptions { get; internal set; } = new BaseOptionInfo();
        /// <summary>
        /// Support for objects' loop reference
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Add new value to types
        /// </summary>
        public Action<Type, object> AddTypes { get; set; }
        /// <summary>
        /// get typefo value from 
        /// </summary>
        public TryGetValue<Type> TryGetValueOfTypeGo { get; set; }
        //public BinarySerializeHandler SerializeHandler { get; set; } = new BinarySerializeHandler();
        /// <summary>
        /// 
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
                AddTypes = Options.Types.Add;
                TryGetValueOfTypeGo = Options.Types.TryGetValue;
                HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;
                Setting.HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;
            }
        }
        /// <summary>
        /// Initialize seralizer 
        /// </summary>
        public BinarySerializer()
        {
            Options = DefaultOptions;
        }

        /// <summary>
        /// Initialize seralizer with options
        /// </summary>
        public BinarySerializer(BaseOptionInfo jsonOptionInfo)
        {
            Options = jsonOptionInfo;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            //SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = jsonOptionInfo.HasGenerateRefrencedTypes;
            Setting.HasGenerateRefrencedTypes = jsonOptionInfo.HasGenerateRefrencedTypes;

            //SerializeFunction = async (TypeGoInfo typeGoInfo, Stream stream, object dataRef) =>
            //{
            //   await  SerializeObject(dataRef, typeGoInfo);
            //};
        }

        /// <summary>
        /// Default serializer settings
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();

        ///// <summary>
        ///// Start of referenced index
        ///// </summary>
        //public int ReferencedIndex { get; set; } = 0;
        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static BinarySerializer NormalInstance
        {
            get
            {
                return new BinarySerializer();
            }
        }

        /// <summary>
        /// The string builder for json serialization
        /// </summary>
        public MemoryStream Writer { get; set; }

        /// <summary>
        /// Serializes an object into a json string
        /// </summary>
        /// <param name="data">Any object to serialize into json</param>
        /// <returns>The json string returned after serialization</returns>
        public Span<byte> Serialize<T>(T data)
        {
            TypeGoInfo<T> typeGoInfo;
            Type dataType = typeof(T);

            if (!TryGetValueOfTypeGo(dataType, out object typeGo))
                typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);
            else
                typeGoInfo = (TypeGoInfo<T>)typeGo;

            // The serialize handler lets the serializer access faster to the pointers
            //JsonSerializeHandler serializeHandler = new JsonSerializeHandler
            //{
            //    BinaryWriter = new BufferBuilder(typeGoInfo.Capacity),
            //    //AddSerializedObjects = serializedObjects.Add,
            //    //TryGetValueOfSerializedObjects = serializedObjects.TryGetValue
            //};
            BufferBuilder binaryWriter = new BufferBuilder(typeGoInfo.Capacity);
            //ReferencedIndex = 0;
            typeGoInfo.BinarySerialize(ref binaryWriter, ref data);
            typeGoInfo.Capacity = Math.Max(typeGoInfo.Capacity, binaryWriter.Length);
            return binaryWriter.ToSpan()[..binaryWriter.Length];
        }

        /// <summary>
        /// Get structure of models of typeGo
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static List<BinaryModelInfo> GetStructureModels(BaseOptionInfo option = default)
        {
            if (option == null)
                option = DefaultOptions;
            List<BinaryModelInfo> result = new List<BinaryModelInfo>();
            Dictionary<Type, BinaryModelInfo> generatedModels = new Dictionary<Type, BinaryModelInfo>();
            foreach (KeyValuePair<Type, object> type in option.Types)
            {
                Type typeGoType = ((BaseTypeGoInfo)type.Value).Type;
                if (typeGoType.Assembly.FullName.StartsWith("System.Private.CoreLib"))
                    continue;
                result.Add(BinaryModelInfo.GetBinaryModel(typeGoType, option, generatedModels));
            }
            return result;
        }
    }
}
