using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Enum serializer and deserializer
    /// </summary>
    public class EnumVariable : ISerializationVariable
    {
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo typeGoInfo, ITypeGo options)
        {
            var baseType = Nullable.GetUnderlyingType(typeGoInfo.Type);
            if (baseType == null)
                baseType = typeGoInfo.Type;
            var currentCulture = TypeGoInfo.CurrentCulture;
            typeGoInfo.IsNoQuotesValueType = false;
            var enumType = Enum.GetUnderlyingType(typeGoInfo.Type);
            if (enumType == typeof(uint))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToUInt32(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (uint.TryParse(x, out uint value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((uint)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToUInt32(reader.Read(sizeof(uint))));
                };
            }
            else if (enumType == typeof(long))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToInt64(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (long.TryParse(new string(x), out long value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((long)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToInt64(reader.Read(sizeof(long))));
                };
            }
            else if (enumType == typeof(short))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToInt16(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (short.TryParse(new string(x), out short value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((short)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToInt16(reader.Read(sizeof(short))));
                };
            }
            else if (enumType == typeof(byte))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToByte(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (byte.TryParse(new string(x), out byte value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((byte)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, reader.Read(sizeof(byte))[0]);
                };
            }
            else if (enumType == typeof(double))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToDouble(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (double.TryParse(new string(x), out double value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((double)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToDouble(reader.Read(sizeof(double))));
                };
            }
            else if (enumType == typeof(float))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToSingle(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (float.TryParse(new string(x), out float value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((float)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToSingle(reader.Read(sizeof(float))));
                };
            }
            else if (enumType == typeof(decimal))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToDecimal(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (decimal.TryParse(new string(x), out decimal value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((double)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, (decimal)BitConverter.ToDouble(reader.Read(sizeof(decimal))));
                };
            }
            else if (enumType == typeof(sbyte))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToSByte(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (sbyte.TryParse(new string(x), out sbyte value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((sbyte)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, (sbyte)reader.Read(sizeof(sbyte))[0]);
                };
            }
            else if (enumType == typeof(ulong))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToUInt64(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (ulong.TryParse(new string(x), out ulong value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((ulong)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToUInt64(reader.Read(sizeof(ulong))));
                };
            }
            else if (enumType == typeof(ushort))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToUInt16(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (ushort.TryParse(new string(x), out ushort value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((ushort)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToUInt16(reader.Read(sizeof(ushort))));
                };
            }
            else if (enumType == typeof(int))
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToInt32(data).ToString(currentCulture));
                };
                typeGoInfo.JsonDeserialize = (deserializer, x) =>
                {
                    if (int.TryParse(new string(x), out int value))
                        return Enum.ToObject(baseType, value);
                    return Enum.ToObject(baseType, 0);
                };

                //binary serialization
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    stream.Write(BitConverter.GetBytes((int)data).AsSpan());
                };

                //binary deserialization
                typeGoInfo.BinaryDeserialize = (ref BinarySpanReader reader) =>
                {
                    return Enum.ToObject(baseType, BitConverter.ToInt32(reader.Read(sizeof(int))));
                };
            }
            else
            {
                throw new Exception($"enum of type {enumType.FullName} not support yet, please contact programmer!");
            }
            typeGoInfo.DefaultValue = TypeGoInfo.GetActivator(baseType);
        }
    }
}
