using JsonGo.CompileTime;
using JsonGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo
{
    /// <summary>
    /// serialize json to an object
    /// </summary>
    public class Serializer
    {
        static Serializer()
        {
            SingleIntance = new Serializer(true);
        }

        public Serializer() : this(true)
        {

        }

        public Serializer(bool generateReference)
        {
            Setting.HasGenerateRefrencedTypes = generateReference;
            //if (generateReference)
            //{
            //    SerializeFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
            //    {
            //        SerializeFunctionWithReference(typeGoInfo, ref dataRef);
            //    };
            //    SerializeArrayFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
            //    {
            //        SerializeArrayFunctionWithReference(typeGoInfo, ref dataRef);
            //    };
            //}
            //else
            //{
            SerializeFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
            {
                SerializeObject(ref dataRef, typeGoInfo);
                //SerializeFunctionWithoutReference(typeGoInfo, ref dataRef);
            };
            //SerializeArrayFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
            //{
            //    SerializeArray((IEnumerable)dataRef);
            //    //SerializeArrayFunctionWithoutReference(typeGoInfo, ref dataRef);
            //};
            //}
        }

        /// <summary>
        /// save serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        public Dictionary<object, int> SerializedObjects { get; set; }
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
        public static Serializer SingleIntance { get; set; }
        /// <summary>
        /// serialize object function
        /// </summary>
        public FunctionTypeGo SerializeFunction { get; set; }

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
            ReferencedIndex = 0;
            SerializedObjects = new Dictionary<object, int>();
            SerializeObject(ref data, out TypeGoInfo typeGo);
            if (typeGo.IsNoQuotesValueType)
            {
                Writer.Insert(0, JsonConstantsString.Quotes);
                Writer.Append(JsonConstantsString.Quotes);
            }
            return Writer.ToString();
        }

        #region CompileTimeSerialization
        public string SerializeCompile<T>(T data)
        {
            Writer = new StringBuilder(256);
            SerializedObjects = new Dictionary<object, int>();
            ReferencedIndex = 0;
            if (GetSerializer(out Action<Serializer, StringBuilder, T> serializer, ref data))
                serializer(this, Writer, data);
            return Writer.ToString();
        }

        public void ContinueSerializeCompile<T>(T data)
        {
            if (GetSerializer(out Action<Serializer, StringBuilder, T> serializer, ref data))
                serializer(this, Writer, data);
        }

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
            if (!TypeGoInfo.Types.TryGetValue(dataType, out typeGoInfo))
                typeGoInfo = TypeGoInfo.Types[dataType] = TypeGoInfo.Generate(dataType);
            typeGoInfo.Serialize(this, Writer, ref data);
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(ref object data, TypeGoInfo typeGoInfo)
        {
            Writer.Append(JsonConstantsString.OpenBraket);
            foreach (var property in typeGoInfo.SerializeProperties)
            {
                object propertyValue = property.GetValue(this, data);
                if (propertyValue == null || propertyValue.Equals(property.TypeGoInfo.DefaultValue))
                    continue;
                Writer.Append(JsonConstantsString.Quotes);
                Writer.Append(property.Name);
                Writer.Append(JsonConstantsString.QuotesColon);
                property.TypeGoInfo.Serialize(this, Writer, ref propertyValue);
                Writer.Append(JsonConstantsString.Comma);
            }
            RemoveLastCama();
            Writer.Append(JsonConstantsString.CloseBracket);
        }
    }
}
