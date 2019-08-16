using JsonGo.Helpers;
using JsonGo.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Deserialize
{
    delegate IJsonGoModel ExtractFunction(ReadOnlySpan<char> json, ref int indexOf);
    //delegate void FastExtractFunction(TypeGoInfo typeGo, ref object instance, Func<object> createInstance, ref ReadOnlySpan<byte> _buffer);
    delegate ReadOnlySpan<byte> FastExtractFunction(Deserializer deserializer, TypeGoInfo typeGo, ref object instance, Func<object> createInstance, ref JsonSpanReader _buffer);

    /// <summary>
    /// deserializer of json
    /// </summary>
    public class Deserializer
    {
        static ExtractFunction ExtractFunction { get; set; }
        static FastExtractFunction FastExtractFunction { get; set; }

        static Deserializer()
        {
            SingleIntance = new Deserializer();
            //ExtractFunction = DeserializerExtractor.Extract;
            //FastExtractFunction = FastDeserializerExtractor2.Extract;
            FastExtractFunction = FastDeserializerExtractor.Extract;
        }

        /// <summary>
        /// save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<int, object> DeSerializedObjects { get; set; } = new Dictionary<int, object>();
        /// <summary>
        /// single instance of deserialize to access faster
        /// </summary>
        public static Deserializer SingleIntance { get; set; }
        /// <summary>
        /// default setting of serializer
        /// </summary>
        public JsonSettingInfo Setting { get; set; } = new JsonSettingInfo();
        /// <summary>
        /// deserialize a json to a type
        /// </summary>
        /// <typeparam name="T">type of deserialize</typeparam>
        /// <param name="json">json to deserialize</param>
        /// <returns>deserialized type</returns>
        public T Deserialize<T>(string json)
        {
            try
            {
                var dataType = typeof(T);
                if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
                    typeGoInfo = TypeGoInfo.Types[dataType] = TypeGoInfo.Generate(dataType);
                object instance = null;
                var reader = new JsonSpanReader(TextHelper.StringToSpanByteArray(ref json));
                var result = FastExtractFunction(this, typeGoInfo, ref instance, typeGoInfo.CreateInstance, ref reader);
                var text = Encoding.UTF8.GetString(result.ToArray());
                if (instance == null)
                    instance = typeGoInfo.Deserialize(this, result);
                if (typeGoInfo.Cast != null)
                    return (T)typeGoInfo.Cast(instance);
                return (T)instance;
            }
            finally
            {
                DeSerializedObjects.Clear();
            }
           
        }

        /// <summary>
        /// deserialize a json to a type
        /// </summary>
        /// <param name="type">type of deserialize</param>
        /// <param name="json">json to deserialize</param>
        /// <returns>deserialized type</returns>
        public object Deserialize(string json, Type type)
        {
            try
            {
                int indexOf = 0;
                IJsonGoModel jsonModel = ExtractFunction(json.AsSpan(), ref indexOf);
                return jsonModel.Generate(type, this);
            }
            finally
            {
                DeSerializedObjects.Clear();
            }
        }


        /// <summary>
        /// get type of a json parameter name
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="key">json parameter name</param>
        /// <returns>type of json parameter</returns>
        internal Type GetKeyType(object obj, string key)
        {
            if (obj == null)
                return null;
            key = key.Trim('\"');
            Type dataType = obj.GetType();
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
            {
                TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            }
            if (typeGoInfo.Properties.TryGetValue(key, out PropertyGoInfo propertyGo))
            {
                return propertyGo.Type;
            }
            return null;
        }

    }
}
