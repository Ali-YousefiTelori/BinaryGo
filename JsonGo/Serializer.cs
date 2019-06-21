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
            SingleIntance = new Serializer();
        }

        public Serializer()
        {
            if (Setting.HasGenerateRefrencedTypes)
            {
                SerializeFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
                {
                    SerializeFunctionWithReference(typeGoInfo, ref dataRef);
                };
                SerializeArrayFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
                {
                    SerializeArrayFunctionWithReference(typeGoInfo, ref dataRef);
                };
            }
            else
            {
                SerializeFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
                {
                    SerializeFunctionWithoutReference(typeGoInfo, ref dataRef);
                };
                SerializeArrayFunction = (TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder, ref object dataRef) =>
                {
                    SerializeArrayFunctionWithoutReference(typeGoInfo, ref dataRef);
                };
            }
        }

        /// <summary>
        /// save serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        public Dictionary<object, int> SerializedObjects { get; set; } = new Dictionary<object, int>();
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonSettingInfo Setting { get; set; } = new JsonSettingInfo();

        /// <summary>
        /// start of referenced index
        /// </summary>
        public int ReferencedIndex { get; set; } = 1;
        /// <summary>
        /// single instance of serializer to accesss faster
        /// </summary>
        public static Serializer SingleIntance { get; set; }
        /// <summary>
        /// serialize object function
        /// </summary>
        public FunctionTypeGo SerializeFunction { get; set; }
        /// <summary>
        /// serialize array function
        /// </summary>
        public FunctionTypeGo SerializeArrayFunction { get; set; }

        /// <summary>
        /// string builder of json serialization
        /// </summary>
        //public StringBuilder Builder { get; set; } = new StringBuilder(256);
        public StringBuilder Writer { get; set; }

        public void RemoveLastCama()
        {
            if (Writer[Writer.Length - 1] == JsonSettingInfo.Comma)
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
            ReferencedIndex = 1;
            SerializedObjects.Clear();
            SerializeObject(ref data);
            return Writer.ToString();
        }

        public string SerializeCompile<T>(T data)
        {
            Writer = new StringBuilder(256);
            SerializedObjects.Clear();
            ReferencedIndex = 0;
            var serializer = TypeInfo<T>.Serialize;
            if (serializer == null)
                throw new Exception($"Type {typeof(T)} not initialized in compile time!");
            serializer(this, Writer, data);
            return Writer.ToString();
        }

        public void ContinueSerializeCompile<T>(T data)
        {
            var serializer = TypeInfo<T>.Serialize;
            if (serializer == null)
                throw new Exception($"Type {typeof(T)} not initialized in compile time!");
            serializer(this, Writer, data);
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <param name="typeGoInfo"></param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(ref object data)
        {
            Type dataType = data.GetType();
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
                typeGoInfo = TypeGoInfo.Types[dataType] = TypeGoInfo.Generate(dataType);
            typeGoInfo.Serialize(this, Writer, ref data);
        }

        /// <summary>
        /// serialize an array to a json string
        /// </summary>
        /// <param name="list">array list</param>
        /// <param name="refrencedId">referenceId</param>
        /// <returns>json that serialized</returns>
        internal void SerializeArrayReference(IEnumerable list, ref int refrencedId)
        {
            Writer.Append(JsonSettingInfo.BeforeObject);
            Writer.Append(refrencedId);
            Writer.Append(JsonSettingInfo.AfterArrayObject);
            foreach (object item in list)
            {
                if (item == null)
                    continue;
                var value = item;
                SerializeObject(ref value);
                Writer.Append(JsonSettingInfo.Comma);
            }
            RemoveLastCama();
            Writer.Append(JsonSettingInfo.CloseSquareBracketsWithBrackets);
        }

        /// <summary>
        /// serialize an object to a json string with reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="refrencedId">referenceId</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObjectReference(object data, ref int refrencedId, TypeGoInfo typeGoInfo)
        {
            Writer.Append(JsonSettingInfo.BeforeObject);
            Writer.Append(refrencedId);
            Writer.Append(JsonSettingInfo.CommaQuotes);

            var properties = typeGoInfo.ArrayProperties;
            foreach (var property in properties)
            {
                object propertyValue = property.GetValue(data);
                if (propertyValue == null)
                    continue;
                Writer.Append(JsonSettingInfo.Quotes);
                Writer.Append(property.Name);
                Writer.Append(JsonSettingInfo.QuotesColon);
                property.TypeGoInfo.Serialize(this, Writer, ref propertyValue);
                Writer.Append(JsonSettingInfo.Comma);
            }
            RemoveLastCama();
            Writer.Append(JsonSettingInfo.CloseBracket);
        }
        /// <summary>
        /// serialize an array to a json string
        /// </summary>
        /// <param name="list">array list</param>
        /// <returns>json that serialized</returns>
        internal void SerializeArray(IEnumerable list)
        {
            Writer.Append(JsonSettingInfo.OpenSquareBrackets);
            foreach (object item in list)
            {
                if (item == null)
                    continue;
                var value = item;
                SerializeObject(ref value);
                Writer.Append(JsonSettingInfo.Comma);
            }

            RemoveLastCama();
            Writer.Append(JsonSettingInfo.CloseSquareBrackets);
        }
        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(object data, TypeGoInfo typeGoInfo)
        {
            Writer.Append(JsonSettingInfo.OpenBraket);
            var properties = typeGoInfo.ArrayProperties;
            foreach (var property in properties)
            {
                object propertyValue = property.GetValue(data);
                if (propertyValue == null)
                    continue;
                Writer.Append(JsonSettingInfo.Quotes);
                Writer.Append(property.Name);
                Writer.Append(JsonSettingInfo.QuotesColon);
                property.TypeGoInfo.Serialize(this, Writer, ref propertyValue);
                Writer.Append(JsonSettingInfo.Comma);
            }
            RemoveLastCama();
            Writer.Append(JsonSettingInfo.CloseBracket);
        }
        /// <summary>
        /// serialize an object to a json string with detect reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        void SerializeFunctionWithReference(TypeGoInfo typeGoInfo, ref object data)
        {
            if (GenerateReference(ref data, out int refrencedId))
                return;
            SerializeObjectReference(data, ref refrencedId, typeGoInfo);
        }
        /// <summary>
        /// serialize an array to a json string with detect reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        void SerializeArrayFunctionWithReference(TypeGoInfo typeGoInfo, ref object data)
        {
            if (GenerateReference(ref data, out int refrencedId))
                return;
            SerializeArrayReference((IEnumerable)data, ref refrencedId);
        }
        /// <summary>
        /// serialize an object to a json string without detect reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        void SerializeFunctionWithoutReference(TypeGoInfo typeGoInfo, ref object data)
        {
            SerializeObject(data, typeGoInfo);
        }
        /// <summary>
        /// serialize an object to a json string without detect reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        void SerializeArrayFunctionWithoutReference(TypeGoInfo typeGoInfo, ref object data)
        {
            SerializeArray((IEnumerable)data);
        }
        /// <summary>
        /// generate reference id for object
        /// </summary>
        /// <param name="data">object to detect reference</param>
        /// <param name="refrencedId">refrence id created</param>
        /// <returns>is reference created</returns>
        bool GenerateReference(ref object data, out int refrencedId)
        {
            if (!SerializedObjects.TryGetValue(data, out refrencedId))
            {
                refrencedId = ReferencedIndex;
                SerializedObjects.Add(data, refrencedId);
                ReferencedIndex++;
                return false;
            }
            else
            {
                Writer.Append(JsonSettingInfo.OpenBraketRefColonQuotes);
                Writer.Append(refrencedId);
                Writer.Append(JsonSettingInfo.QuotesCloseBracket);
                return true;
            }
        }
    }
}
