using JsonGo.Binary;
using JsonGo.DataTypes;
using JsonGo.Json.Deserialize;
using JsonGo.Helpers;
using JsonGo.Interfaces;
using JsonGo.Json;
using JsonGo.Runtime.Interfaces;
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
        /// <param name="typeGoInfo"></param>
        /// <param name="options">options or settings of variable serializer</param>
        public static void InitializeVariable<T>(TypeGoInfo typeGoInfo, ITypeGo options) where T : ISerializationVariable, new()
        {
            T variable = new T();
            variable.Initialize(typeGoInfo, options);
        }


        /// <summary>
        /// initialize a typeGo for a runtime type
        /// the typeGo make everything to use faster with near access
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TypeGoInfo Generate(Type type, ITypeGo options)
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
                    InitializeVariable<DateTimeVariable>(typeGoInfo, options);
                else if (baseType == typeof(uint))
                    InitializeVariable<UIntVariable>(typeGoInfo, options);
                else if (baseType == typeof(long))
                    InitializeVariable<LongVariable>(typeGoInfo, options);
                else if (baseType == typeof(short))
                    InitializeVariable<ShortVariable>(typeGoInfo, options);
                else if (baseType == typeof(byte))
                    InitializeVariable<ByteVariable>(typeGoInfo, options);
                else if (baseType == typeof(double))
                    InitializeVariable<DoubleVariable>(typeGoInfo, options);
                else if (baseType == typeof(float))
                    InitializeVariable<FloatVariable>(typeGoInfo, options);
                else if (baseType == typeof(decimal))
                    InitializeVariable<DecimalVariable>(typeGoInfo, options);
                else if (baseType == typeof(sbyte))
                    InitializeVariable<SByteVariable>(typeGoInfo, options);
                else if (baseType == typeof(ulong))
                    InitializeVariable<ULongVariable>(typeGoInfo, options);
                else if (baseType == typeof(bool))
                    InitializeVariable<BoolVariable>(typeGoInfo, options);
                else if (baseType == typeof(ushort))
                    InitializeVariable<UShortVariable>(typeGoInfo, options);
                else if (baseType == typeof(int))
                    InitializeVariable<IntVariable>(typeGoInfo, options);
                else if (baseType == typeof(byte[]))
                    InitializeVariable<ByteArrayVariable>(typeGoInfo, options);
                else if (baseType == typeof(string))
                    InitializeVariable<StringVariable>(typeGoInfo, options);
                else if (baseType.IsEnum)
                    InitializeVariable<EnumVariable>(typeGoInfo, options);
                //array data
                else if (typeof(IEnumerable).IsAssignableFrom(baseType))
                    InitializeVariable<ArrayVariable>(typeGoInfo, options);
                //object daat
                else
                {
                    baseType = GenerateTypeFromInterface(baseType);
                    typeGoInfo.IsNoQuotesValueType = false;
                    if (options.HasGenerateRefrencedTypes)
                    {
                        //add $Id dproperties
                        typeGoInfo.Properties[JsonConstantsString.IdRefrencedTypeNameNoQuotes] = new PropertyGoInfo()
                        {
                            TypeGoInfo = Generate(typeof(int), options),
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
                                handler.AppendChar(JsonConstantsString.OpenBraket);
                                handler.Append(JsonConstantsString.RefRefrencedTypeName);
                                handler.AppendChar(JsonConstantsString.Colon);
                                handler.Append(refrencedId.ToString(CurrentCulture));
                                handler.AppendChar(JsonConstantsString.CloseBracket);
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
        public static void AddToCustomTypes(Type type, Type result)
        {
            CustomTypeChanges[type] = result;
        }

        /// <summary>
        /// get delete of type to make fast way to create instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
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


}
