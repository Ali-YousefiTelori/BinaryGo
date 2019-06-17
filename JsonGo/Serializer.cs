using JsonGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// string builder of json serialization
        /// </summary>
        public StringBuilder Builder { get; set; } = new StringBuilder();
        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        public string Serialize(object data)
        {
            ReferencedIndex = 1;
            Builder.Clear();
            SerializedObjects.Clear();
            return SerializeObject(Builder, ref data, this).ToString();
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <param name="serializer"></param>
        /// <returns>json that serialized from you object</returns>
        internal static StringBuilder SerializeObject(StringBuilder stringBuilder, ref object data, Serializer serializer)
        {
            Type dataType = data.GetType();
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
            {
                TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            }
            return typeGoInfo.Serialize(serializer, stringBuilder, ref data);
        }

        //internal static string SerializeArray(IEnumerable list, Serializer serializer)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append('[');
        //    foreach (object item in list)
        //    {
        //        if (item != null)
        //        {
        //            string serialized = SerializeObject(item, serializer);
        //            if (serialized != null)
        //            {
        //                stringBuilder.Append(serialized);
        //                stringBuilder.Append(',');
        //            }
        //        }
        //    }

        //    if (stringBuilder[stringBuilder.Length - 1] == ',')
        //        stringBuilder.Length--;
        //    stringBuilder.Append(']');
        //    return stringBuilder.ToString();
        //}

        internal static StringBuilder SerializeArrayReference(StringBuilder stringBuilder, IEnumerable list, ref object refrencedId, Serializer serializer)
        {
            stringBuilder.Append(JsonSettingInfo.BeforeObject);
            stringBuilder.Append(refrencedId);
            stringBuilder.Append(JsonSettingInfo.AfterArrayObject);
            foreach (object item in list)
            {
                var value = item;
                SerializeObject(stringBuilder, ref value, serializer);
                stringBuilder.Append(JsonSettingInfo.Comma);
            }

            if (stringBuilder[stringBuilder.Length - 1] == JsonSettingInfo.Comma)
                stringBuilder.Length--;
            stringBuilder.Append(JsonSettingInfo.CloseSquareBrackets);
            stringBuilder.Append(JsonSettingInfo.CloseBracket);
            return stringBuilder;
        }

        internal static StringBuilder SerializeObjectReference(StringBuilder stringBuilder, object data, ref object refrencedId, TypeGoInfo typeGoInfo, Serializer serializer)
        {
            stringBuilder.Append(JsonSettingInfo.BeforeObject);
            stringBuilder.Append(refrencedId);
            stringBuilder.Append(JsonSettingInfo.CommaQuotes);

            var array = typeGoInfo.ArrayProperties;
            for (int i = 0; i < array.Length; i++)
            {
                var property = array[i];
                object propertyValue = property.GetValue(data);
                if (propertyValue != null)
                {
                    stringBuilder.Append(JsonSettingInfo.Quotes);
                    stringBuilder.Append(property.Name);
                    stringBuilder.Append(JsonSettingInfo.QuotesColon);
                    SerializeObject(stringBuilder, ref propertyValue, serializer);
                    stringBuilder.Append(JsonSettingInfo.Comma);
                }
            }
            if (stringBuilder[stringBuilder.Length - 1] == JsonSettingInfo.Comma)
                stringBuilder.Length--;
            stringBuilder.Append(JsonSettingInfo.CloseBracket);
            return stringBuilder;
        }

        //internal static string SerializeObject(object data, TypeGoInfo typeGoInfo, Serializer serializer)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append('{');
        //    foreach (var item in typeGoInfo.Properties)
        //    {
        //        var property = item.Value;
        //        object propertyValue = property.GetValue(data);
        //        if (propertyValue != null)
        //        {
        //            string serializedValue = SerializeObject(propertyValue, serializer);
        //            if (serializedValue != null)
        //            {
        //                stringBuilder.Append('\"');
        //                stringBuilder.Append(property.Name);
        //                stringBuilder.Append('\"');
        //                stringBuilder.Append(':');
        //                stringBuilder.Append(serializedValue);
        //                stringBuilder.Append(',');
        //            }
        //        }
        //    }
        //    if (stringBuilder[stringBuilder.Length - 1] == ',')
        //        stringBuilder.Length--;
        //    stringBuilder.Append('}');
        //    return stringBuilder.ToString();
        //}
    }
}
