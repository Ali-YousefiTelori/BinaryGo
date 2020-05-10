using JsonGo.DataTypes;
using JsonGo.Deserialize;
using JsonGo.Helpers;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void FunctionGo(SerializeHandler handler, ref object data);
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder,
    public delegate void FunctionTypeGo(TypeGoInfo typeGoInfo, SerializeHandler handler, ref object data);
    public delegate object DeserializeFunc(Deserializer deserializer, ReadOnlySpan<char> data);

    /// <summary>
    /// generate type details on memory
    /// </summary>
    public class TypeGoInfo
    {
        /// <summary>
        /// current calture
        /// </summary>
        public static CultureInfo CurrentCulture { get; set; }
        static TypeGoInfo()
        {
            CurrentCulture = new CultureInfo("en-US");
            AddToCustomTypes<ICollection, Array>();
            AddToCustomTypes<IEnumerable, Array>();
            AddToCustomTypes<IList, Array>();
            AddToCustomTypes(typeof(IEnumerable<>), typeof(List<>));
            AddToCustomTypes(typeof(ICollection<>), typeof(List<>));
            AddToCustomTypes(typeof(IList<>), typeof(List<>));
        }
        /// <summary>
        /// default value of object
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// if the type is simple like int,byte,bool,enum they can serialize without quots
        /// </summary>
        public bool IsNoQuotesValueType { get; set; } = true;
        /// <summary>
        /// type of data
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// serialize action
        /// </summary>
        public FunctionGo Serialize { get; set; }
        /// <summary>
        /// deserialize string to object
        /// </summary>
        public DeserializeFunc Deserialize { get; set; }
        /// <summary>
        /// create instance of type
        /// </summary>
        public Func<object> CreateInstance { get; set; }
        /// <summary>
        /// cast to real object
        /// </summary>
        public Func<object, object> Cast { get; set; }
        /// <summary>
        /// add array value to array type go
        /// </summary>
        public Action<object, object> AddArrayValue { get; set; }

        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> Properties { get; set; }
        /// <summary>
        /// properties of type
        /// </summary>
        public Dictionary<string, PropertyGoInfo> DirectProperties { get; set; }
        /// <summary>
        /// array of all properties for serialize
        /// </summary>
        public PropertyGoInfo[] SerializeProperties { get; set; }
        /// <summary>
        /// array of all properties for deserialize
        /// </summary>
        public PropertyGoInfo[] DeserializeProperties { get; set; }
        /// <summary>
        /// generic types
        /// </summary>
        public List<TypeGoInfo> Generics { get; set; } = new List<TypeGoInfo>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeGoInfo Generate(Type type, IJson options)
        {
            if (options.TryGetValueOfTypeGo(type, out TypeGoInfo find))
                return find;
            var baseType = Nullable.GetUnderlyingType(type);
            bool isNullable = false;
            if (baseType != null)
            {
                isNullable = true;
            }
            else
                baseType = type;
            TypeGoInfo typeGoInfo = new TypeGoInfo
            {
                Properties = new Dictionary<string, PropertyGoInfo>(),
                Type = type,
            };
            options.AddTypes(type, typeGoInfo);
            if (baseType == typeof(DateTime))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.AppendChar(JsonConstantsString.Quotes);
                    handler.Append(((DateTime)data).ToString(CurrentCulture));
                    handler.AppendChar(JsonConstantsString.Quotes);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    var text = new string(x.ToArray());
                    if (DateTime.TryParse(text, out DateTime value))
                        return value;
                    return default(DateTime);
                };
                typeGoInfo.DefaultValue = default(DateTime);
            }
            else if (baseType == typeof(uint))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((uint)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (uint.TryParse(new string(x.ToArray()), out uint value))
                        return value;
                    return default(uint);
                };
                typeGoInfo.DefaultValue = default(uint);
            }
            else if (baseType == typeof(long))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((long)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (long.TryParse(new string(x.ToArray()), out long value))
                        return value;
                    return default(long);
                };
                typeGoInfo.DefaultValue = default(long);
            }
            else if (baseType == typeof(short))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((short)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (short.TryParse(new string(x.ToArray()), out short value))
                        return value;
                    return default(short);
                };
                typeGoInfo.DefaultValue = default(short);
            }
            else if (baseType == typeof(byte))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((byte)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (byte.TryParse(new string(x.ToArray()), out byte value))
                        return value;
                    return default(byte);
                };
                typeGoInfo.DefaultValue = default(byte);
            }
            else if (baseType == typeof(double))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((double)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (double.TryParse(new string(x.ToArray()), out double value))
                        return value;
                    return default(double);
                };
                typeGoInfo.DefaultValue = default(double);
            }
            else if (baseType == typeof(float))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((float)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (float.TryParse(new string(x.ToArray()), out float value))
                        return value;
                    return default(float);
                };
                typeGoInfo.DefaultValue = default(float);
            }
            else if (baseType == typeof(decimal))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((decimal)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (decimal.TryParse(new string(x.ToArray()), out decimal value))
                        return value;
                    return default(decimal);
                };
                typeGoInfo.DefaultValue = default(decimal);
            }
            else if (baseType == typeof(sbyte))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((sbyte)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (sbyte.TryParse(new string(x.ToArray()), out sbyte value))
                        return value;
                    return default(sbyte);
                };
                typeGoInfo.DefaultValue = default(sbyte);
            }
            else if (baseType == typeof(ulong))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((ulong)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (ulong.TryParse(new string(x.ToArray()), out ulong value))
                        return value;
                    return default(ulong);
                };
                typeGoInfo.DefaultValue = default(ulong);
            }
            else if (baseType == typeof(bool))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    if ((bool)data)
                        handler.Append("true");
                    else
                        handler.Append("false");
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (bool.TryParse(new string(x.ToArray()), out bool value))
                        return value;
                    return default(bool);
                };
                typeGoInfo.DefaultValue = default(bool);
            }
            else if (baseType == typeof(ushort))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((ushort)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (ushort.TryParse(new string(x.ToArray()), out ushort value))
                        return value;
                    return default(ushort);
                };
                typeGoInfo.DefaultValue = default(ushort);
            }
            else if (baseType == typeof(int))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(((int)data).ToString(CurrentCulture));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    if (int.TryParse(new string(x.ToArray()), out int value))
                        return value;
                    return default(int);
                };
                typeGoInfo.DefaultValue = default(int);
            }
            else if (baseType == typeof(byte[]))
            {
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.Append(Convert.ToBase64String((byte[])data));
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return Convert.FromBase64String(new string(x.ToArray()));
                };
                typeGoInfo.DefaultValue = default(byte[]);
            }
            else if (baseType == typeof(string))
            {
                typeGoInfo.IsNoQuotesValueType = false;
                typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                {
                    handler.AppendChar(JsonConstantsString.Quotes);
                    handler.Append(((string)data).Replace("\"", "\\\"").Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
                    handler.AppendChar(JsonConstantsString.Quotes);
                };
                typeGoInfo.Deserialize = (deserializer, x) =>
                {
                    return new string(x.ToArray());
                };
                typeGoInfo.DefaultValue = default(string);
            }
            else if (baseType.IsEnum)
            {
                var enumType = Enum.GetUnderlyingType(baseType);
                if (enumType == typeof(uint))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToUInt32(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (uint.TryParse(new string(x.ToArray()), out uint value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(long))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToInt64(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (long.TryParse(new string(x.ToArray()), out long value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(short))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToInt16(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (short.TryParse(new string(x.ToArray()), out short value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(byte))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToByte(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (byte.TryParse(new string(x.ToArray()), out byte value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(double))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToDouble(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (double.TryParse(new string(x.ToArray()), out double value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(float))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToSingle(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (float.TryParse(new string(x.ToArray()), out float value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(decimal))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToDecimal(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (decimal.TryParse(new string(x.ToArray()), out decimal value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(sbyte))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToSByte(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (sbyte.TryParse(new string(x.ToArray()), out sbyte value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(ulong))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToUInt64(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (ulong.TryParse(new string(x.ToArray()), out ulong value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(ushort))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToUInt16(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (ushort.TryParse(new string(x.ToArray()), out ushort value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }
                else if (enumType == typeof(int))
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Append(Convert.ToInt32(data).ToString(CurrentCulture));
                    };
                    typeGoInfo.Deserialize = (deserializer, x) =>
                    {
                        if (int.TryParse(new string(x.ToArray()), out int value))
                            return Enum.ToObject(baseType, value);
                        return Enum.ToObject(baseType, 0);
                    };
                }

                typeGoInfo.DefaultValue = Activator.CreateInstance(baseType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(baseType))
            {
                baseType = GenerateTypeFromInterface(baseType);
                typeGoInfo.IsNoQuotesValueType = false;
                if (options.HasGenerateRefrencedTypes)
                {
                    //add $Id dproperties
                    typeGoInfo.Properties[JsonConstantsBytes.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                    {
                        TypeGoInfo = Generate(typeof(int), options),
                        Type = typeof(int),
                        Name = JsonConstantsBytes.IdRefrencedTypeNameNoQuotes,
                        SetValue = (serializer, instance, value) =>
                        {
                            serializer.DeSerializedObjects.Add((int)value, instance);
                        },
                        GetValue = (handler, data) =>
                        {
                            if (!handler.TryGetValueOfSerializedObjects(data, out int refrencedId))
                            {
                                var serializer = handler.Serializer;
                                serializer.ReferencedIndex++;
                                handler.AddSerializedObjects(data, serializer.ReferencedIndex);
                                return serializer.ReferencedIndex;
                            }
                            else
                            {
                                return refrencedId;
                            }
                        }
                    };

                    typeGoInfo.Properties[JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                    {
                        TypeGoInfo = Generate(type, options),
                        Type = type,
                        Name = JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes,
                        SetValue = (serializer, instance, value) =>
                        {
                            if (Generate(instance.GetType(), options) is TypeGoInfo typeGo)
                            {
                                foreach (var item in (IEnumerable)value)
                                {
                                    typeGo.AddArrayValue(instance, item);
                                }
                            }
                        },
                        GetValue = (handler, data) =>
                        {
                            if (data == null)
                                return null;
                            handler.AppendChar(JsonConstantsString.Quotes);
                            handler.Append(JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes);
                            handler.Append(JsonConstantsString.QuotesColon);
                            handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
                            var generic = typeGoInfo.Generics[0];
                            foreach (var item in (IEnumerable)data)
                            {
                                var obj = item;
                                generic.Serialize(handler, ref obj);
                                handler.AppendChar(JsonConstantsBytes.Comma);
                            }
                            handler.Serializer.RemoveLastCama();
                            handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
                            return null;
                        }
                    };
                }
                foreach (var item in baseType.GetGenericArguments())
                {
                    if (!options.TryGetValueOfTypeGo(item, out TypeGoInfo typeGoInfoProperty))
                    {
                        typeGoInfoProperty = Generate(item, options);
                    }
                    typeGoInfo.Generics.Add(typeGoInfoProperty);
                }

                if (baseType.IsArray)
                {
                    var elementType = baseType.GetElementType();
                    var newType = typeof(List<>).MakeGenericType(elementType);
                    if (!options.TryGetValueOfTypeGo(elementType, out TypeGoInfo typeGoInfoProperty))
                    {
                        typeGoInfoProperty = Generate(elementType, options);
                    }
                    typeGoInfo.Generics.Add(typeGoInfoProperty);
                    typeGoInfo.CreateInstance = () => Activator.CreateInstance(newType);
                    var castMethod = typeof(TypeGoInfo).GetMethod("GetArray", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                    typeGoInfo.Cast = (obj) => castMethod.Invoke(null, new object[] { obj });
                    var method = newType.GetMethod("Add");
                    typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                }
                else
                {
                    typeGoInfo.CreateInstance = () => Activator.CreateInstance(baseType);
                    var method = baseType.GetMethod("Add");
                    typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                }
                if (options.HasGenerateRefrencedTypes)
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
                    };
                }
                else
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        if (data != null)
                        {
                            handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
                            var generic = typeGoInfo.Generics[0];
                            foreach (var item in (IEnumerable)data)
                            {
                                var obj = item;
                                generic.Serialize(handler, ref obj);
                                handler.AppendChar(JsonConstantsBytes.Comma);
                            }
                            handler.Serializer.RemoveLastCama();
                            handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
                        }
                        else
                        {
                            handler.Append("null");
                        }
                    };
                }
                typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.GetValue != null).ToArray();
                typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.SetValue != null).ToArray();
                typeGoInfo.DefaultValue = null;
            }
            else
            {
                baseType = GenerateTypeFromInterface(baseType);
                typeGoInfo.IsNoQuotesValueType = false;
                if (options.HasGenerateRefrencedTypes)
                {
                    //add $Id dproperties
                    typeGoInfo.Properties[JsonConstantsBytes.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                    {
                        TypeGoInfo = Generate(typeof(int), options),
                        Type = typeof(int),
                        Name = JsonConstantsBytes.IdRefrencedTypeNameNoQuotes,
                        SetValue = (serializer, instance, value) =>
                        {
                            serializer.DeSerializedObjects.Add((int)value, instance);
                        },
                        GetValue = (handler, data) =>
                        {
                            if (!handler.TryGetValueOfSerializedObjects(data, out int refrencedId))
                            {
                                var serializer = handler.Serializer;
                                serializer.ReferencedIndex++;
                                handler.AddSerializedObjects(data, serializer.ReferencedIndex);
                                return serializer.ReferencedIndex;
                            }
                            else
                            {
                                return refrencedId;
                            }
                        }
                    };
                }
                
                foreach (var property in baseType.GetProperties())
                {
                    if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0 || !property.CanRead || !property.CanWrite || property.GetIndexParameters().Length > 0)
                        continue;
                    IPropertyCallerInfo del = null;
                    try
                    {
                        del = GetDelegateInstance(baseType, property);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Cannot create delegate for property {property.Name} in type {type.FullName}", ex);
                    }
                    if (!options.TryGetValueOfTypeGo(property.PropertyType, out TypeGoInfo typeGoInfoProperty))
                    {
                        typeGoInfoProperty = Generate(property.PropertyType, options);
                    }
                    typeGoInfo.Properties[property.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = property.PropertyType,
                        Name = property.Name,
                        GetValue = (handler, x) => del.GetPropertyValue(x),
                        SetValue = del.SetPropertyValue,
                        //SetValue = (x, val) => property.SetValue(x, val),
                    };
                }



                foreach (var item in baseType.GetFields())
                {
                    if (item.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        continue;

                    if (!options.TryGetValueOfTypeGo(item.FieldType, out TypeGoInfo typeGoInfoProperty))
                    {
                        typeGoInfoProperty = Generate(item.FieldType, options);
                    }
                    typeGoInfo.Properties[item.Name] = new PropertyGoInfo()
                    {
                        TypeGoInfo = typeGoInfoProperty,
                        Type = item.FieldType,
                        Name = item.Name,
                        //GetValue = item.GetValue,
                        //SetValue = item.SetValue
                    };
                }
                typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.GetValue != null).ToArray();
                typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.SetValue != null).ToArray();
                if (options.HasGenerateRefrencedTypes)
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        if (handler.TryGetValueOfSerializedObjects(data, out int refrencedId))
                        {
                            handler.AppendChar(JsonConstantsBytes.OpenBraket);
                            handler.Append(JsonConstantsBytes.RefRefrencedTypeName);
                            handler.AppendChar(JsonConstantsString.Colon);
                            handler.Append(refrencedId.ToString(CurrentCulture));
                            handler.AppendChar(JsonConstantsBytes.CloseBracket);
                        }
                        else
                            handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
                    };
                }
                else
                {
                    typeGoInfo.Serialize = (SerializeHandler handler, ref object data) =>
                    {
                        handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
                    };
                }

                typeGoInfo.CreateInstance = () => Activator.CreateInstance(baseType);
                typeGoInfo.DefaultValue = null;
            }
            if (isNullable)
                typeGoInfo.DefaultValue = null;
            return typeGoInfo;
        }

        internal static Dictionary<Type, Type> CustomTypeChanges { get; set; } = new Dictionary<Type, Type>();
        /// <summary>
        /// generate interface ot types to new types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GenerateTypeFromInterface(Type type)
        {
            Type[] genericTypes = null;
            if (type.GenericTypeArguments.Length > 0)
            {
                genericTypes = type.GenericTypeArguments;
                type = type.GetGenericTypeDefinition();
            }

            if (CustomTypeChanges.TryGetValue(type, out Type newType))
            {
                type = newType;
            }

            if (genericTypes != null)
            {
                for (int i = 0; i < genericTypes.Length; i++)
                {
                    genericTypes[i] = GenerateTypeFromInterface(genericTypes[i]);
                }
                type = type.MakeGenericType(genericTypes);
            }
            return type;
        }

        /// <summary>
        /// add your types or interfaces to automatic custom type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static void AddToCustomTypes<TType, TResult>()
        {
            AddToCustomTypes(typeof(TType), typeof(TResult));
        }
        /// <summary>
        /// add your types or interfaces to automatic custom type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static void AddToCustomTypes(Type type, Type result)
        {
            CustomTypeChanges[type] = result;
        }
        public static IPropertyCallerInfo GetDelegateInstance(Type type, PropertyInfo propertyInfo)
        {
            var openGetterType = typeof(Func<,>);
            var concreteGetterType = openGetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            var openSetterType = typeof(Action<,>);
            var concreteSetterType = openSetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            Delegate getterInvocation = Delegate.CreateDelegate(concreteGetterType, null, propertyInfo.GetGetMethod());
            Delegate setterInvocation = Delegate.CreateDelegate(concreteSetterType, null, propertyInfo.GetSetMethod());

            var callerType = typeof(PropertyCallerInfo<,>);
            var callerGenericType = callerType
                .MakeGenericType(type, propertyInfo.PropertyType);

            return (IPropertyCallerInfo)Activator.CreateInstance(callerGenericType, getterInvocation, setterInvocation);
        }

        static T[] GetArray<T>(IEnumerable<T> iList)
        {
            var result = new T[iList.Count()];

            Array.Copy(iList.ToArray(), 0, result, 0, result.Length);
            return result;
        }
    }

    public interface IPropertyCallerInfo
    {
        object GetPropertyValue(object instance);
        void SetPropertyValue(Deserializer deserializer, object instance, object value);
    }
    public class PropertyCallerInfo<TType, TPropertyType> : IPropertyCallerInfo
    {
        public PropertyCallerInfo(Func<TType, TPropertyType> funcGetValue, Action<TType, TPropertyType> funcSetValue)
        {
            GetValue = funcGetValue;
            SetValue = funcSetValue;
        }
        public Func<TType, TPropertyType> GetValue { get; set; }
        public Action<TType, TPropertyType> SetValue { get; set; }

        public object GetPropertyValue(object instance)
        {
            return GetValue((TType)instance);
        }

        public void SetPropertyValue(Deserializer deserializer, object instance, object value)
        {
            SetValue((TType)instance, (TPropertyType)value);
        }
    }
}
