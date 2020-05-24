using JsonGo.Json;
using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JsonGo.Binary
{
    public class BinarySerializer : IJson
    {
        static BinarySerializer()
        {
            SingleIntance = new BinarySerializer();
        }

        internal static JsonOptionInfo DefaultOptions { get; set; } = new JsonOptionInfo();

        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// add new value to types
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// get typefo value from 
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        //public BinarySerializeHandler SerializeHandler { get; set; } = new BinarySerializeHandler();
        internal JsonOptionInfo Options { get; set; }
        public BinarySerializer()
        {
            Options = DefaultOptions;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            //SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = Options.IsGenerateLoopReference;
            Setting.HasGenerateRefrencedTypes = Options.IsGenerateLoopReference;

            //SerializeFunction = (TypeGoInfo typeGoInfo, Stream stream, ref object dataRef) =>
            //{
            //    SerializeObject(ref dataRef, typeGoInfo);
            //};
        }

        public BinarySerializer(JsonOptionInfo jsonOptionInfo)
        {
            Options = jsonOptionInfo;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            //SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = jsonOptionInfo.IsGenerateLoopReference;
            Setting.HasGenerateRefrencedTypes = jsonOptionInfo.IsGenerateLoopReference;

            //SerializeFunction = async (TypeGoInfo typeGoInfo, Stream stream, object dataRef) =>
            //{
            //   await  SerializeObject(dataRef, typeGoInfo);
            //};
        }

        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();

        /// <summary>
        /// start of referenced index
        /// </summary>
        public int ReferencedIndex { get; set; } = 0;
        /// <summary>
        /// single instance of serializer to accesss faster
        /// </summary>
        public static BinarySerializer SingleIntance { get; set; }
        /// <summary>
        /// serialize object function
        /// </summary>
        //public BinaryFunctionTypeGo SerializeFunction { get; set; }

        /// <summary>
        /// string builder of json serialization
        /// </summary>
        public MemoryStream Writer { get; set; }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        public MemoryStream Serialize(object data)
        {
            Writer = new MemoryStream();
            ReferencedIndex = 0;
            Dictionary<object, int> serializedObjects = new Dictionary<object, int>();
            //SerializeHandler.AddSerializedObjects = serializedObjects.Add;
            //SerializeHandler.TryGetValueOfSerializedObjects = serializedObjects.TryGetValue;
            SerializeObject(data);
            return Writer;
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(object data)
        {
            Type dataType = data.GetType();
            if (!TryGetValueOfTypeGo(dataType, out TypeGoInfo typeGoInfo))
            {
                typeGoInfo = TypeGoInfo.Generate(dataType, this);
            }
            typeGoInfo.BinarySerialize(Writer, ref data);
        }
    }
}
