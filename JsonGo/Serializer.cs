using System;
using System.Collections;
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
            Type dataType = data.GetType();

            if (dataType == typeof(int) ||
                dataType == typeof(string) ||
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
                dataType == typeof(ushort))
            {
                return string.Concat('\"', data.ToString(), '\"');
            }
            else if (dataType.IsEnum)
                return string.Concat('\"', Convert.ToInt32(data).ToString(), '\"');
            else if (data is IEnumerable list)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append('[');
                foreach (object item in list)
                {
                    stringBuilder.Append(Serialize(item));
                    stringBuilder.Append(',');
                }
                if (stringBuilder[stringBuilder.Length - 1] == ',')
                    stringBuilder.Length--;
                stringBuilder.Append(']');
                return stringBuilder.ToString();
            }
            else
            {

                StringBuilder stringBuilder = new StringBuilder();
                PropertyInfo[] properties = dataType.GetProperties();

                stringBuilder.Append('{');
                for (int i = 0; i < properties.Length; i++)
                {
                    System.Reflection.PropertyInfo property = properties[i];
                    stringBuilder.Append('\"');
                    stringBuilder.Append(property.Name);
                    stringBuilder.Append('\"');
                    stringBuilder.Append(':');
                    stringBuilder.Append(Serialize(property.GetValue(data)));
                    stringBuilder.Append(',');
                }
                if (stringBuilder[stringBuilder.Length - 1] == ',')
                    stringBuilder.Length--;
                stringBuilder.Append('}');
                return stringBuilder.ToString();
            }
        }
    }
}
