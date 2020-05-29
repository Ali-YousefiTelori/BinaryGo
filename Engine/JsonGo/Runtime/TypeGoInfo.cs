using JsonGo.Binary;
using JsonGo.DataTypes;
using JsonGo.Deserialize;
using JsonGo.Helpers;
using JsonGo.Interfaces;
using JsonGo.Json;
using JsonGo.Runtime.Variables;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JsonGo.Runtime
{
    public delegate T RefFunc<T>(ReadOnlySpan<char> readOnlySpan);
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void JsonFunctionGo(JsonSerializeHandler handler, ref object data);
    /// <summary>
    /// binary serializer
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="data"></param>
    public delegate void BinaryFunctionGo(Stream stream, ref object data);
    /// <summary>
    /// function for serialize object
    /// </summary>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder,
    public delegate void JsonFunctionTypeGo(TypeGoInfo typeGoInfo, JsonSerializeHandler handler, ref object data);
    public delegate void BinaryFunctionTypeGo(TypeGoInfo typeGoInfo, Stream stream, ref object data);
    public delegate object DeserializeFunc(JsonDeserializer deserializer, ReadOnlySpan<char> data);

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
        public JsonFunctionGo JsonSerialize { get; set; }
        /// <summary>
        /// binary serialize
        /// </summary>
        public BinaryFunctionGo BinarySerialize { get; set; }
        /// <summary>
        /// deserialize string to object
        /// </summary>
        public DeserializeFunc JsonDeserialize { get; set; }
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
        /// initialize a variable to a typeGo
        /// </summary>
        /// <param name="serializationVariable"></param>
        /// <param name="typeGoInfo"></param>
        public static void InitializeVariable<T>(TypeGoInfo typeGoInfo) where T : ISerializationVariable, new()
        {
            T variable = new T();
            variable.Initialize(typeGoInfo);
        }


        /// <summary>
        /// initialize a typeGo for a runtime type
        /// the typeGo make everything to use faster with near access
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TypeGoInfo Generate(Type type, IJson options)
        {
            lock (options)
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
                    InitializeVariable<DateTimeVariable>(typeGoInfo);
                else if (baseType == typeof(uint))
                    InitializeVariable<UIntVariable>(typeGoInfo);
                else if (baseType == typeof(long))
                    InitializeVariable<LongVariable>(typeGoInfo);
                else if (baseType == typeof(short))
                    InitializeVariable<ShortVariable>(typeGoInfo);
                else if (baseType == typeof(byte))
                    InitializeVariable<ByteVariable>(typeGoInfo);
                else if (baseType == typeof(double))
                    InitializeVariable<DoubleVariable>(typeGoInfo);
                else if (baseType == typeof(float))
                    InitializeVariable<FloatVariable>(typeGoInfo);
                else if (baseType == typeof(decimal))
                    InitializeVariable<DecimalVariable>(typeGoInfo);
                else if (baseType == typeof(sbyte))
                    InitializeVariable<SByteVariable>(typeGoInfo);
                else if (baseType == typeof(ulong))
                    InitializeVariable<ULongVariable>(typeGoInfo);
                else if (baseType == typeof(bool))
                    InitializeVariable<BoolVariable>(typeGoInfo);
                else if (baseType == typeof(ushort))
                    InitializeVariable<UShortVariable>(typeGoInfo);
                else if (baseType == typeof(int))
                    InitializeVariable<IntVariable>(typeGoInfo);
                else if (baseType == typeof(byte[]))
                    InitializeVariable<ByteArrayVariable>(typeGoInfo);
                else if (baseType == typeof(string))
                    InitializeVariable<StringVariable>(typeGoInfo);
                else if (baseType.IsEnum)
                    InitializeVariable<EnumVariable>(typeGoInfo);
                //array data
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

                        typeGoInfo.Properties[JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                        {
                            TypeGoInfo = Generate(type, options),
                            Type = type,
                            Name = JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes,
                            JsonSetValue = (serializer, instance, value) =>
                            {
                                if (Generate(instance.GetType(), options) is TypeGoInfo typeGo)
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
                                handler.Append(JsonConstantsBytes.ValuesRefrencedTypeNameNoQuotes);
                                handler.Append(JsonConstantsString.QuotesColon);
                                handler.AppendChar(JsonConstantsString.OpenSquareBrackets);
                                var generic = typeGoInfo.Generics[0];
                                foreach (var item in (IEnumerable)data)
                                {
                                    var obj = item;
                                    generic.JsonSerialize(handler, ref obj);
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
                        typeGoInfo.CreateInstance = GetActivator(newType);
                        var castMethod = typeof(TypeGoInfo).GetMethod("GetArray", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(elementType);
                        typeGoInfo.Cast = (obj) => castMethod.Invoke(null, new object[] { obj });
                        var method = newType.GetMethod("Add");
                        typeGoInfo.AddArrayValue = (obj, value) => method.Invoke(obj, new object[] { value });
                    }
                    else
                    {
                        typeGoInfo.CreateInstance = GetActivator(baseType);
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
                        typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                        {
                            var generic = typeGoInfo.Generics[0];
                            if (data != null)
                            {
                                foreach (var item in (IEnumerable)data)
                                {
                                    var obj = item;
                                    generic.BinarySerialize(stream, ref obj);
                                }
                            }
                            else
                            {
                                //await generic.BinarySerialize(stream, (byte)0);
                            }
                        };
                    }
                    typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonGetValue != null).ToArray();
                    typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonSetValue != null).ToArray();
                    typeGoInfo.DefaultValue = null;
                }
                //object daat
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
                            JsonGetValue = (handler, x) => del.GetPropertyValue(x),
                            JsonSetValue = del.SetPropertyValue,
                            GetValue = del.GetPropertyValue,
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
                    typeGoInfo.SerializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonGetValue != null).ToArray();
                    typeGoInfo.DeserializeProperties = typeGoInfo.Properties.Values.Where(x => x.JsonSetValue != null).ToArray();
                    if (options.HasGenerateRefrencedTypes)
                    {
                        typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
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
                        typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
                        {
                            handler.Serializer.SerializeFunction(typeGoInfo, handler, ref data);
                        };

                        typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
                        {
                            var properties = typeGoInfo.SerializeProperties;
                            var len = properties.Length;
                            for (int i = 0; i < len; i++)
                            {
                                var property = properties[i];
                                var value = property.GetValue(data);
                                if (value == null || value == property.TypeGoInfo.DefaultValue)
                                {

                                }
                                else
                                    property.TypeGoInfo.BinarySerialize(stream, ref value);
                            }
                        };
                    }

                    typeGoInfo.CreateInstance = GetActivator(baseType);
                    typeGoInfo.DefaultValue = null;
                }
                if (isNullable)
                    typeGoInfo.DefaultValue = null;
                return typeGoInfo;
            }
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

        /// <summary>
        /// object create instance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<object> GetActivator(Type type)
        {
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            if (emptyConstructor == null)
            {
                return new Func<object>(() => null);
            }
            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(emptyConstructor);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(Func<object>), newExp);

            //compile it
            Func<object> compiled = (Func<object>)lambda.Compile();
            return compiled;
        }

        //public static Func<object> GetActivator(Type type)
        //{
        //    ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
        //    if (emptyConstructor == null)
        //    {
        //        return new Func<object>(() => null);
        //    }
        //    var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
        //    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
        //    ilGenerator.Emit(OpCodes.Nop);
        //    ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
        //    ilGenerator.Emit(OpCodes.Ret);
        //    return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        //}
    }

    public interface IPropertyCallerInfo
    {
        object GetPropertyValue(object instance);
        void SetPropertyValue(JsonDeserializer deserializer, object instance, object value);
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
            if (instance == null)
            {

            }
            return GetValue((TType)instance);
        }

        public void SetPropertyValue(JsonDeserializer deserializer, object instance, object value)
        {
            SetValue((TType)instance, (TPropertyType)value);
        }
    }
}
