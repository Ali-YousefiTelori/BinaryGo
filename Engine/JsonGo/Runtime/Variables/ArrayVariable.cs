using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// Array serializer and deserializer
    /// </summary>
    public class ArrayVariable : ISerializationVariable
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
            baseType = TypeGoInfo.GenerateTypeFromInterface(baseType);
            var currentCulture = TypeGoInfo.CurrentCulture;
            typeGoInfo.IsNoQuotesValueType = false;
            if (options.HasGenerateRefrencedTypes)
            {
                //add $Id dproperties
                typeGoInfo.Properties[JsonConstantsString.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = TypeGoInfo.Generate(typeof(int), options),
                    Type = typeof(int),
                    Name = JsonConstantsString.IdRefrencedTypeNameNoQuotes,
                    JsonSetValue = (serializer, instance, value) =>
                    {
                        serializer.DeSerializedObjects.Add((int)value, instance);
                    },
                    JsonGetValue = (handler, data) =>
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

                typeGoInfo.Properties[JsonConstantsString.ValuesRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                {
                    TypeGoInfo = TypeGoInfo.Generate(typeGoInfo.Type, options),
                    Type = typeGoInfo.Type,
                    Name = JsonConstantsString.ValuesRefrencedTypeNameNoQuotes,
                    JsonSetValue = (serializer, instance, value) =>
                    {
                        if (TypeGoInfo.Generate(instance.GetType(), options) is TypeGoInfo typeGo)
                        {
                            foreach (var item in (IEnumerable)value)
                            {
                                typeGo.AddArrayValue(instance, item);
                            }
                        }
                    },
                    JsonGetValue = (handler, data) =>
                    {
                        if (data == null)
                            return null;
                        handler.AppendChar(JsonConstantsString.Quotes);
                        handler.Append(JsonConstantsString.ValuesRefrencedTypeNameNoQuotes);
                        handler.Append(JsonConstantsString.QuotesColon);
                        handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
                        var generic = typeGoInfo.Generics[0];
                        foreach (var item in (IEnumerable)data)
                        {
                            var obj = item;
                            generic.JsonSerialize(handler, ref obj);
                            handler.AppendChar(JsonConstantsString.Comma);
                        }
                        handler.Serializer.RemoveLastComma();
                        handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
                        return null;
                    }
                };
            }
            foreach (var item in baseType.GetGenericArguments())
            {
                if (!options.TryGetValueOfTypeGo(item, out TypeGoInfo typeGoInfoProperty))
                {
                    typeGoInfoProperty = TypeGoInfo.Generate(item, options);
                }
                typeGoInfo.Generics.Add(typeGoInfoProperty);
            }

            if (baseType.IsArray)
            {
                var elementType = baseType.GetElementType();
                var newType = typeof(List<>).MakeGenericType(elementType);
                if (!options.TryGetValueOfTypeGo(elementType, out TypeGoInfo typeGoInfoProperty))
                {
                    typeGoInfoProperty = TypeGoInfo.Generate(elementType, options);
                }
                typeGoInfo.Generics.Add(typeGoInfoProperty);
                typeGoInfo.CreateInstance = TypeGoInfo.GetActivator(newType);
                var castMethod = typeof(TypeGoInfo).GetMethod("GetArray", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                typeGoInfo.Cast = (obj) => castMethod.Invoke(null, new object[] { obj });
                var method = newType.GetMethod("Add");
                typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
            }
            else
            {
                typeGoInfo.CreateInstance = TypeGoInfo.GetActivator(baseType);
                //use add method for deserialization
                var method = baseType.GetMethod("Add");
                typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
            }

            if (options.HasGenerateRefrencedTypes)
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
                };
            }
            else
            {
                typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                {
                    if (data != null)
                    {
                        handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
                        var generic = typeGoInfo.Generics[0];
                        foreach (var item in (IEnumerable)data)
                        {
                            var obj = item;
                            generic.JsonSerialize(handler, ref obj);
                            handler.AppendChar(JsonConstantsString.Comma);
                        }
                        handler.Serializer.RemoveLastComma();
                        handler.AppendChar(JsonConstantsString.CloseSquareBrackets);
                    }
                    else
                    {
                        handler.Append("null");
                    }
                };
                typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                {
                    var generic = typeGoInfo.Generics[0];
                    if (data != null)
                    {
                        if (data is ICollection collection)
                        {
                            stream.Write(BitConverter.GetBytes(collection.Count));
                            foreach (var item in collection)
                            {
                                var obj = item;
                                generic.BinarySerialize(stream, ref obj);
                            }
                        }
                        else if (data is IEnumerable enumerable)
                        {
                            int count = 0;
                            IEnumerator enumerator = enumerable.GetEnumerator();
                            while (enumerator.MoveNext())
                                count++;
                            stream.Write(BitConverter.GetBytes(count));
                            foreach (var item in enumerable)
                            {
                                var obj = item;
                                generic.BinarySerialize(stream, ref obj);
                            }
                        }

                    }
                    else
                    {
                        stream.Write(BitConverter.GetBytes(0));
                    }
                };
            }
            typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonGetValue != null).ToArray();
            typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonSetValue != null).ToArray();
            typeGoInfo.DefaultValue = null;
        }
    }
}