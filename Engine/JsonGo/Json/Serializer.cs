using JsonGo.CompileTime;
using JsonGo.Helpers;
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
    /// Serializes json to an object
    /// </summary>
    public class Serializer : ITypeGo
    {
        static Serializer()
        {
            TypeGoInfo.GenerateDefaultVariables(DefaultOptions);
        }

        /// <summary>
        /// Serialization's default options
        /// </summary>
        public static BaseOptionInfo DefaultOptions { get; set; } = new BaseOptionInfo();

        /// <summary>
        /// Generates $ref and $values and support for serialization and deserialization loop reference 
        /// </summary>
        public bool HasGenerateRefrencedTypes { get; set; }

        /// <summary>
        /// Adds new value to types
        /// </summary>
        public Action<Type, TypeGoInfo> AddTypes { get; set; }
        /// <summary>
        /// Gets TypeGo value 
        /// </summary>
        public TryGetValue<Type, TypeGoInfo> TryGetValueOfTypeGo { get; set; }
        /// <summary>
        /// The serialize handler lets the serializer access faster to the pointers
        /// </summary>
        public JsonSerializeHandler SerializeHandler { get; set; } = new JsonSerializeHandler();
        internal BaseOptionInfo Options { get; set; }
        /// <summary>
        /// JsonGo serializer: serializes object to json
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
        /// Serializes object to json
        /// </summary>
        /// <param name="jsonOptionInfo">your json serializer option</param>
        public Serializer(BaseOptionInfo jsonOptionInfo)
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
        /// Serializer's default settings
        /// </summary>
        public JsonConstantsString Setting { get; set; } = new JsonConstantsString();

        /// <summary>
        /// Start value of referenced index
        /// </summary>
        public int ReferencedIndex { get; set; } = 0;
        /// <summary>
        /// With serializer's static single instance there's no need to new it manually every time: faster usage
        /// </summary>
        public static Serializer NormalInstance
        {
            get
            {
                return new Serializer();
            }
        }
        /// <summary>
        /// Serialize object function
        /// </summary>
        public JsonFunctionTypeGo SerializeFunction { get; set; }

        /// <summary>
        /// String builder for json serialization
        /// </summary>
        public StringBuilder Writer { get; set; }

        /// <summary>
        /// Removes last comma from serialization
        /// </summary>
        public void RemoveLastComma()
        {
            if (Writer[Writer.Length - 1] == JsonConstantsString.Comma)
                Writer.Length--;
        }

        /// <summary>
        /// Serializes an object to a json string
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <returns>Json string returned from your serialized object</returns>
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
        /// The "compile time" serialization is a faster way to serialize data
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
        /// Continue serialization in compile time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void ContinueSerializeCompile<T>(T data)
        {
            if (GetSerializer(out Action<Serializer, StringBuilder, T> serializer, ref data))
                serializer(this, Writer, data);
        }

        /// <summary>
        /// Gets "compile time" serializer to serialize data faster
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
        /// Serializes an object to a json string
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="typeGoInfo"></param>
        /// <returns>Json string from serialized object</returns>
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
        /// Serializes an object to a json string
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="typeGoInfo">TypeGo of jsongo</param>
        /// <returns>Json string from serialized object</returns>
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
            RemoveLastComma();
            appendChar(JsonConstantsString.CloseBracket);
        }
    }
}
