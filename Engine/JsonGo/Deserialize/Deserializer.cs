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
    delegate JsonSpanReader FastExtractFunction(TypeGoInfo typeGo, object instance, ref JsonSpanReader json);

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
            ExtractFunction = DeserializerExtractor.Extract;
            FastExtractFunction = FastDeserializerExtractor.Extract;
        }

        /// <summary>
        /// save deserialized objects for referenced type
        /// </summary>
        internal Dictionary<string, object> DeSerializedObjects { get; set; } = new Dictionary<string, object>();
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
            //int indexOf = 0;
            DeSerializedObjects.Clear();
            var dataType = typeof(T);
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
                typeGoInfo = TypeGoInfo.Types[dataType] = TypeGoInfo.Generate(dataType);
            var instance = typeGoInfo.CreateInstance();
            var reader = new JsonSpanReader(Encoding.UTF8.GetBytes(json).AsSpan());
            FastExtractFunction(typeGoInfo, instance, ref reader);
            return (T)instance;
            //IJsonGoModel jsonModel = ExtractFunction(json.AsSpan(), ref indexOf);
            //return default(T);
            //return (T)jsonModel.Generate(typeof(T), this);
        }

        /// <summary>
        /// deserialize a json to a type
        /// </summary>
        /// <param name="type">type of deserialize</param>
        /// <param name="json">json to deserialize</param>
        /// <returns>deserialized type</returns>
        public object Deserialize(string json, Type type)
        {
            int indexOf = 0;
            DeSerializedObjects.Clear();
            IJsonGoModel jsonModel = ExtractFunction(json.AsSpan(), ref indexOf);
            return jsonModel.Generate(type, this);
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

        /// <summary>
        /// set value of json parameter key to an instance of object
        /// </summary>
        /// <param name="obj">object to change parameter</param>
        /// <param name="value">value to set</param>
        /// <param name="key">parameter name of object</param>
        internal void SetValue(object obj, object value, string key)
        {
            if (obj == null)
                return;
            key = key.Trim('\"');
            Type dataType = obj.GetType();
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
            {
                TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            }
            if (typeGoInfo.Properties.TryGetValue(key, out PropertyGoInfo propertyGo))
            {
                propertyGo.SetValue(obj, value);
            }
        }

        internal object GetValue(Type type, object value)
        {
            if (value == null)
                return null;
            if (type.IsEnum)
            {
                value = Convert.ChangeType(value, typeof(int));
                value = Enum.ToObject(type, (int)value);
            }
            else
                value = Convert.ChangeType(value, type);
            return value;
        }
    }
}
