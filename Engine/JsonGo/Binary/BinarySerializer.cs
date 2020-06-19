using JsonGo.Helpers;
using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JsonGo.Binary
{
    /// <summary>
    /// JsonGo binary serializer: serializes your object to byte array or memory stream
    /// </summary>
    public class BinarySerializer : ISerializer
    {
        static BinarySerializer()
        {
            BaseTypeGoInfo.GenerateDefaultVariables(DefaultOptions);
        }

        internal static BaseOptionInfo DefaultOptions { get; set; } = new BaseOptionInfo();
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
        internal BaseOptionInfo Options { get; set; }
        /// <summary>
        /// Initialize seralizer 
        /// </summary>
        public BinarySerializer()
        {
            Options = DefaultOptions;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            //SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;
            Setting.HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;

            //SerializeFunction = (TypeGoInfo typeGoInfo, Stream stream, ref object dataRef) =>
            //{
            //    SerializeObject(ref dataRef, typeGoInfo);
            //};
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

        /// <summary>
        /// Start of referenced index
        /// </summary>
        public int ReferencedIndex { get; set; } = 0;
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
        public Span<byte> Serialize(object data)
        {
            Writer = new MemoryStream();
            ReferencedIndex = 0;
            Dictionary<object, int> serializedObjects = new Dictionary<object, int>();
            //SerializeHandler.AddSerializedObjects = serializedObjects.Add;
            //SerializeHandler.TryGetValueOfSerializedObjects = serializedObjects.TryGetValue;
            //SerializeObject(data);
            return Writer.ToArray().AsSpan();
        }
    }
}
