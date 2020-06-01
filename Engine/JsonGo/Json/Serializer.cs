using JsonGo.CompileTime;
using JsonGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Json
{
    /// <summary>
    /// serialize json to an object
    /// </summary>
    public class Serializer : ITypeGo
    {
        static Serializer()
        {
            TypeGoInfo.Generate(typeof(DateTime), DefaultOptions);
            TypeGoInfo.Generate(typeof(uint), DefaultOptions);
            TypeGoInfo.Generate(typeof(long), DefaultOptions);
            TypeGoInfo.Generate(typeof(short), DefaultOptions);
            TypeGoInfo.Generate(typeof(byte), DefaultOptions);
            TypeGoInfo.Generate(typeof(double), DefaultOptions);
            TypeGoInfo.Generate(typeof(float), DefaultOptions);
            TypeGoInfo.Generate(typeof(decimal), DefaultOptions);
            TypeGoInfo.Generate(typeof(sbyte), DefaultOptions);
            TypeGoInfo.Generate(typeof(ulong), DefaultOptions);
            TypeGoInfo.Generate(typeof(bool), DefaultOptions);
            TypeGoInfo.Generate(typeof(ushort), DefaultOptions);
            TypeGoInfo.Generate(typeof(int), DefaultOptions);
            TypeGoInfo.Generate(typeof(byte[]), DefaultOptions);
            TypeGoInfo.Generate(typeof(int[]), DefaultOptions);
            TypeGoInfo.Generate(typeof(string), DefaultOptions);
        }

        /// <summary>
        /// default options of serialization
        /// </summary>
        public static JsonOptionInfo DefaultOptions { get; set; } = new JsonOptionInfo();

        /// <summary>
        /// generate $ref and $values and support for loop reference serialization and deserialization
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// add new value to types
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// get typefo value from 
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// the serialize handler let the serializer to fast access to pointers
        /// </summary>
        public JsonSerializeHandler SerializeHandler { get; set; } = new JsonSerializeHandler();
        internal JsonOptionInfo Options { get; set; }
        /// <summary>
        /// json serializer, serialize your object to json
        /// </summary>
        public Serializer()
        {
            Options = DefaultOptions;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;
            Setting.HasGenerateRefrencedTypes = Options.HasGenerateRefrencedTypes;

            SerializeFunction = (TypeGoInfo typeGoInfo, JsonSerializeHandler handler, ref object dataRef) =>
            {
                SerializeObject(ref dataRef, typeGoInfo);
            };
        }

        /// <summary>
        /// serialize your object to json
        /// </summary>
        /// <param name="jsonOptionInfo">your json serializer option</param>
        public Serializer(JsonOptionInfo jsonOptionInfo)
        {
            Options = jsonOptionInfo;

            AddTypes = Options.Types.Add;
            TryGetValueOfTypeGo = Options.Types.TryGetValue;
            SerializeHandler.Serializer = this;

            HasGenerateRefrencedTypes = jsonOptionInfo.HasGenerateRefrencedTypes;
            Setting.HasGenerateRefrencedTypes = jsonOptionInfo.HasGenerateRefrencedTypes;

            SerializeFunction = (TypeGoInfo typeGoInfo, JsonSerializeHandler handler, ref object dataRef) =>
            {
                SerializeObject(ref dataRef, typeGoInfo);
            };
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
        public static Serializer NormalIntance
        {
            get
            {
                return new Serializer();
            }
        }
        /// <summary>
        /// serialize object function
        /// </summary>
        public JsonFunctionTypeGo SerializeFunction { get; set; }

        /// <summary>
        /// string builder of json serialization
        /// </summary>
        public StringBuilder Writer { get; set; }

        /// <summary>
        /// remove last cama from serialization
        /// </summary>
        public void RemoveLastCama()
        {
            if (Writer[Writer.Length - 1] == JsonConstantsString.Comma)
                Writer.Length--;
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        public string Serialize(object data)
        {
            Writer = new StringBuilder(256);
            SerializeHandler.Append = Writer.Append;
            SerializeHandler.AppendChar = Writer.Append;
            ReferencedIndex = 0;
            Dictionary<object, int> serializedObjects = new Dictionary<object, int>();
            SerializeHandler.AddSerializedObjects = serializedObjects.Add;
            SerializeHandler.TryGetValueOfSerializedObjects = serializedObjects.TryGetValue;
            SerializeObject(ref data, out TypeGoInfo typeGo);
            //if (typeGo.IsNoQuotesValueType)
            //{
            //    Writer.Insert(0, JsonConstantsString.Quotes);
            //    Writer.Append(JsonConstantsString.Quotes);
            //}
            return Writer.ToString();
        }

        #region CompileTimeSerialization
        /// <summary>
        /// compile time serialization is faster way to serialize your data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SerializeCompile<T>(T data)
        {
            Writer = new StringBuilder(256);
            //ClearSerializedObjects();
            ReferencedIndex = 0;
            if (GetSerializer(out Action<Serializer, StringBuilder, T> serializer, ref data))
                serializer(this, Writer, data);
            return Writer.ToString();
        }

        /// <summary>
        /// continue serialization in compile time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void ContinueSerializeCompile<T>(T data)
        {
            if (GetSerializer(out Action<Serializer, StringBuilder, T> serializer, ref data))
                serializer(this, Writer, data);
        }

        /// <summary>
        /// get serializer oof compile time serialization to serialize it faster
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool GetSerializer<T>(out Action<Serializer, StringBuilder, T> serializer, ref T data)
        {
            serializer = TypeInfo<T>.Serialize;
            if (serializer == null)
            {
                var genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IEnumerable<>))
                {
                    var type = typeof(IEnumerable<>).MakeGenericType(typeof(T).GetGenericArguments()[0]);
                    if (TypeManager.CompiledTypes.TryGetValue(type, out CompileTime.TypeInfo typeInfo))
                    {
                        typeInfo.DynamicSerialize(this, Writer, data);
                        return false;
                    }
                }
                throw new Exception($"Type {typeof(T)} not initialized in compile time!");
            }
            return true;
        }
        #endregion

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <param name="typeGoInfo"></param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(ref object data, out TypeGoInfo typeGoInfo)
        {
            Type dataType = data.GetType();
            if (!TryGetValueOfTypeGo(dataType, out typeGoInfo))
            {
                typeGoInfo = TypeGoInfo.Generate(dataType, this);
            }
            typeGoInfo.JsonSerialize(SerializeHandler, ref data);
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(ref object data, TypeGoInfo typeGoInfo)
        {
            Func<char, StringBuilder> appendChar = Writer.Append;
            Func<string, StringBuilder> append = Writer.Append;
            appendChar(JsonConstantsString.OpenBraket);
            var properties = typeGoInfo.SerializeProperties;
            var length = properties.Length;
            for (int i = 0; i < length; i++)
            {
                var property = properties[i];
                var propertyType = property.TypeGoInfo;
                object propertyValue = property.JsonGetValue(SerializeHandler, data);
                if (propertyValue == null || propertyValue.Equals(propertyType.DefaultValue))
                    continue;
                appendChar(JsonConstantsString.Quotes);
                append(property.Name);
                append(JsonConstantsString.QuotesColon);
                propertyType.JsonSerialize(SerializeHandler, ref propertyValue);
                appendChar(JsonConstantsString.Comma);
            }
            RemoveLastCama();
            appendChar(JsonConstantsString.CloseBracket);
        }
    }
}
