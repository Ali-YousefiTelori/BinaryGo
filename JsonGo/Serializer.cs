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
        internal Dictionary<object, string> SerializedObjects { get; set; } = new Dictionary<object, string>();
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
            return SerializeObject(Builder, data, this).ToString();
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <param name="serializer"></param>
        /// <returns>json that serialized from you object</returns>
        internal static StringBuilder SerializeObject(StringBuilder stringBuilder, object data, Serializer serializer)
        {
            if (data == null)
            {
                throw new Exception("data is null to serialize");
            }
            Type dataType = data.GetType();

            //else if (Setting.HasGenerateRefrencedTypes)
            //{

            //}
            //else
            //    return null;
            if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
            {
                TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            }
            return typeGoInfo.Serialize(serializer, stringBuilder, data);
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

        internal static StringBuilder SerializeArrayReference(StringBuilder stringBuilder, IEnumerable list, ref string refrencedId, Serializer serializer)
        {
            stringBuilder.Append($"{{{JsonSettingInfo.IdRefrencedTypeName}:\"{refrencedId}\",{JsonSettingInfo.ValuesRefrencedTypeName}:");

            stringBuilder.Append('[');
            foreach (object item in list)
            {
                if (item != null)
                {
                    SerializeObject(stringBuilder, item, serializer);
                    stringBuilder.Append(',');
                }
            }

            if (stringBuilder[stringBuilder.Length - 1] == ',')
                stringBuilder.Length--;
            stringBuilder.Append(']');
            stringBuilder.Append('}');
            return stringBuilder;
        }

        internal static StringBuilder SerializeObjectReference(StringBuilder stringBuilder, object data, ref string refrencedId, TypeGoInfo typeGoInfo, Serializer serializer)
        {
            stringBuilder.Append('{');
            stringBuilder.Append($"{JsonSettingInfo.IdRefrencedTypeName}:\"{refrencedId}\",");

            foreach (var item in typeGoInfo.Properties)
            {
                var property = item.Value;
                object propertyValue = property.GetValue(data);
                if (propertyValue != null)
                {
                    stringBuilder.Append('\"');
                    stringBuilder.Append(property.Name);
                    stringBuilder.Append('\"');
                    stringBuilder.Append(':');
                    SerializeObject(stringBuilder, propertyValue, serializer);
                    stringBuilder.Append(',');
                }
            }
            if (stringBuilder[stringBuilder.Length - 1] == ',')
                stringBuilder.Length--;
            stringBuilder.Append('}');
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

        internal static StringBuilder SerializeString(StringBuilder stringBuilder, string value)
        {
            foreach (char ch in value)
            {
                if (ch == '"')
                    stringBuilder.Append('\\');
                stringBuilder.Append(ch);
            }
            return stringBuilder;
        }
    }
}
