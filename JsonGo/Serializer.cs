using JsonGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// save serialized objects to skip stackoverflow exception and for referenced type
        /// </summary>
        internal Dictionary<object, object> SerializedObjects { get; set; } = new Dictionary<object, object>();
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonSettingInfo Setting { get; set; } = new JsonSettingInfo();

        /// <summary>
        /// start of referenced index
        /// </summary>
        internal int ReferencedIndex { get; set; } = 1;
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
        public StringBuilder Builder { get; set; } = new StringBuilder(256);

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        public string Serialize(object data)
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
            ReferencedIndex = 1;
            Builder.Clear();
            SerializedObjects.Clear();
            SerializeObject(ref data, null);
            return Builder.ToString();
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <param name="typeGoInfo"></param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(ref object data, TypeGoInfo typeGoInfo)
        {
            if (data == null)
            {
                Builder.Append(JsonSettingInfo.Null);
                return;
            }
            Type dataType = data.GetType();
            if (typeGoInfo == null && !TypeGoInfo.Types.TryGetValue(dataType, out typeGoInfo))
            {
                TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            }
            typeGoInfo.Serialize(this, Builder, ref data);
        }

        /// <summary>
        /// serialize an array to a json string
        /// </summary>
        /// <param name="list">array list</param>
        /// <param name="refrencedId">referenceId</param>
        /// <returns>json that serialized</returns>
        internal void SerializeArrayReference(IEnumerable list, ref object refrencedId)
        {
            Builder.Append(JsonSettingInfo.BeforeObject);
            Builder.Append(refrencedId);
            Builder.Append(JsonSettingInfo.AfterArrayObject);
            foreach (object item in list)
            {
                var value = item;
                SerializeObject(ref value, null);
                Builder.Append(JsonSettingInfo.Comma);
            }
            if (Builder[Builder.Length - 1] == JsonSettingInfo.Comma)
                Builder.Length--;
            Builder.Append(JsonSettingInfo.CloseSquareBracketsWithBrackets);
        }

        /// <summary>
        /// serialize an object to a json string with reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="refrencedId">referenceId</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObjectReference(object data, ref object refrencedId, TypeGoInfo typeGoInfo)
        {
            Builder.Append(JsonSettingInfo.BeforeObject);
            Builder.Append(refrencedId);
            Builder.Append(JsonSettingInfo.CommaQuotes);

            var properties = typeGoInfo.ArrayProperties;
            foreach (var property in properties)
            {
                object propertyValue = property.GetValue(data);
                Builder.Append(JsonSettingInfo.Quotes);
                Builder.Append(property.Name);
                Builder.Append(JsonSettingInfo.QuotesColon);
                SerializeObject(ref propertyValue, property.TypeGoInfo);
                Builder.Append(JsonSettingInfo.Comma);
            }
            if (Builder[Builder.Length - 1] == JsonSettingInfo.Comma)
                Builder.Length--;
            Builder.Append(JsonSettingInfo.CloseBracket);
        }
        /// <summary>
        /// serialize an array to a json string
        /// </summary>
        /// <param name="list">array list</param>
        /// <returns>json that serialized</returns>
        internal void SerializeArray(IEnumerable list)
        {
            Builder.Append(JsonSettingInfo.OpenSquareBrackets);
            foreach (object item in list)
            {
                var value = item;
                SerializeObject(ref value, null);
                Builder.Append(JsonSettingInfo.Comma);
            }

            if (Builder[Builder.Length - 1] == JsonSettingInfo.Comma)
                Builder.Length--;
            Builder.Append(JsonSettingInfo.CloseSquareBrackets);
        }
        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        internal void SerializeObject(object data, TypeGoInfo typeGoInfo)
        {
            Builder.Append(JsonSettingInfo.OpenBraket);
            var properties = typeGoInfo.ArrayProperties;
            foreach (var property in properties)
            {
                object propertyValue = property.GetValue(data);
                if (propertyValue != null)
                {
                    Builder.Append(JsonSettingInfo.Quotes);
                    Builder.Append(property.Name);
                    Builder.Append(JsonSettingInfo.QuotesColon);
                    SerializeObject(ref propertyValue, property.TypeGoInfo);
                    Builder.Append(JsonSettingInfo.Comma);
                }
            }
            if (Builder[Builder.Length - 1] == JsonSettingInfo.Comma)
                Builder.Length--;
            Builder.Append(JsonSettingInfo.CloseBracket);
        }
        /// <summary>
        /// serialize an object to a json string with detect reference
        /// </summary>
        /// <param name="data">object to serialize</param>
        /// <param name="typeGoInfo">typego of jsongo</param>
        /// <returns>json that serialized</returns>
        void SerializeFunctionWithReference(TypeGoInfo typeGoInfo, ref object data)
        {
            if (GenerateReference(ref data, out object refrencedId))
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
            if (GenerateReference(ref data, out object refrencedId))
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
        bool GenerateReference(ref object data, out object refrencedId)
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
                Builder.Append(JsonSettingInfo.OpenBraketRefColonQuotes + refrencedId + JsonSettingInfo.QuotesCloseBracket);
                return true;
            }
        }
    }
}
