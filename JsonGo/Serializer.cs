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
        private int ReferencedIndex { get; set; } = 1;
        /// <summary>
        /// single instance of serializer to accesss faster
        /// </summary>
        public static Serializer SingleIntance { get; set; }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        public string Serialize(object data)
        {
            ReferencedIndex = 1;
            SerializedObjects.Clear();
            return SerializeObject(data);
        }

        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="data">any object to serialize</param>
        /// <returns>json that serialized from you object</returns>
        internal string SerializeObject(object data)
        {
            if (data == null)
            {
                throw new Exception("data is null to serialize");
            }
            Type dataType = data.GetType();

            if (dataType == typeof(int) ||
                dataType == typeof(DateTime) ||
                dataType == typeof(uint) ||
                dataType == typeof(long) ||
                dataType == typeof(short) ||
                dataType == typeof(byte) ||
                dataType == typeof(double) ||
                dataType == typeof(float) ||
                dataType == typeof(decimal) ||
                dataType == typeof(sbyte) ||
                dataType == typeof(ulong) ||
                dataType == typeof(bool) ||
                dataType == typeof(ushort))
            {
                return string.Concat('\"', data.ToString(), '\"');
            }
            else if (dataType == typeof(string))
            {
                return string.Concat('\"', SerializeString(data as string), '\"');
            }
            else if (dataType.IsEnum)
                return string.Concat('\"', Convert.ToInt32(data).ToString(), '\"');

            if (!SerializedObjects.TryGetValue(data, out string refrencedId))
            {
                refrencedId = ReferencedIndex.ToString();
                SerializedObjects.Add(data, refrencedId);
                ReferencedIndex++;
            }
            else if (Setting.HasGenerateRefrencedTypes)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("{");
                stringBuilder.Append($"{Setting.RefRefrencedTypeName}:\"{refrencedId}\"");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
            else
                return null;

            if (data is IEnumerable list)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (Setting.HasGenerateRefrencedTypes)
                    stringBuilder.Append($"{{{Setting.IdRefrencedTypeName}:\"{refrencedId}\",{Setting.ValuesRefrencedTypeName}:");

                stringBuilder.Append('[');
                foreach (object item in list)
                {
                    if (item != null)
                    {
                        string serialized = SerializeObject(item);
                        if (serialized != null)
                        {
                            stringBuilder.Append(serialized);
                            stringBuilder.Append(',');
                        }
                    }
                }
                if (stringBuilder[stringBuilder.Length - 1] == ',')
                    stringBuilder.Length--;
                stringBuilder.Append(']');
                if (Setting.HasGenerateRefrencedTypes)
                    stringBuilder.Append('}');
                return stringBuilder.ToString();
            }
            else
            {

                StringBuilder stringBuilder = new StringBuilder();
                PropertyInfo[] properties = dataType.GetProperties();

                stringBuilder.Append('{');
                if (Setting.HasGenerateRefrencedTypes)
                    stringBuilder.Append($"{Setting.IdRefrencedTypeName}:\"{refrencedId}\",");

                for (int i = 0; i < properties.Length; i++)
                {
                    System.Reflection.PropertyInfo property = properties[i];
                    object propertyValue = property.GetValue(data);
                    if (propertyValue != null)
                    {
                        string serializedValue = SerializeObject(propertyValue);
                        if (serializedValue != null)
                        {
                            stringBuilder.Append('\"');
                            stringBuilder.Append(property.Name);
                            stringBuilder.Append('\"');
                            stringBuilder.Append(':');
                            stringBuilder.Append(serializedValue);
                            stringBuilder.Append(',');
                        }
                    }
                }
                if (stringBuilder[stringBuilder.Length - 1] == ',')
                    stringBuilder.Length--;
                stringBuilder.Append('}');
                return stringBuilder.ToString();
            }
        }

        internal string SerializeString(string value)
        {
            StringBuilder result = new StringBuilder();
            foreach (char ch in value)
            {
                if (ch == '"')
                    result.Append('\\');
                result.Append(ch);
            }
            return result.ToString();
        }
    }
}
