using JsonGo.Runtime.Variables;
using JsonGo.Runtime.Variables.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// base of typeGo
    /// </summary>
    public abstract class BaseTypeGoInfo
    {
        /// <summary>
        /// maximum size of this type taked from memory used for bufferbuilder 
        /// </summary>
        public int Capacity = 2;

        /// <summary>
        /// default value of bytes
        /// </summary>
        public byte[] DefaultBinaryValue;

        /// <summary>
        /// Initializes a variable to a TypeGo
        /// </summary>
        public static TVariable InitializeVariable<TVariable>(object typeGoInfo, ITypeGo options)
            where TVariable : BaseVariable, new()
        {
            TVariable variable = new TVariable();
            variable.InitializeBase(typeGoInfo, options);
            return variable;
        }

        static readonly object _lockobj = new object();
        /// <summary>
        /// Initializes a TypeGo for a runtime type
        /// the typeGo makes use of everything faster with easy access
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TypeGoInfo<T> Generate<T>(ITypeGo options)
        {
            lock (_lockobj)
            {
                Type type = typeof(T);
                if (options.TryGetValueOfTypeGo(type, out object find))
                    return (TypeGoInfo<T>)find;
                var baseType = Nullable.GetUnderlyingType(type);

                //when type is not nullable
                if (baseType == null)
                {
                    baseType = type;
                }

                TypeGoInfo<T> typeGoInfo = new TypeGoInfo<T>
                {
                    Properties = new Dictionary<string, BasePropertyGoInfo<T>>(),
                    Type = type,
                };

                options.AddTypes(type, typeGoInfo);


                if (baseType == typeof(bool))
                    InitializeVariable<BoolVariable>(typeGoInfo, options);
                else if (baseType == typeof(DateTime))
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
                else if (baseType == typeof(ushort))
                    InitializeVariable<UShortVariable>(typeGoInfo, options);
                else if (baseType == typeof(int))
                    InitializeVariable<IntVariable>(typeGoInfo, options);
                else if (baseType == typeof(byte[]))
                    InitializeVariable<ByteArrayVariable>(typeGoInfo, options);
                else if (baseType == typeof(string))
                    InitializeVariable<StringVariable>(typeGoInfo, options);
                else if (baseType.IsEnum)
                {
                    BaseVariable variable = (BaseVariable)typeof(EnumVariable<>).MakeGenericType(typeof(T))
                        .GetMethod("Initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                        .Invoke(null, new object[] { });
                    variable.InitializeBase(typeGoInfo, options);
                }
                //array data
                else if (baseType.IsArray)
                {
                    var elementType = baseType.GetElementType();
                    var method = typeof(BaseTypeGoInfo)
                        .GetMethod("InitializeVariable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                        .MakeGenericMethod(typeof(ArrayVariable<>).MakeGenericType(elementType));
                    method.Invoke(null, new object[] { typeGoInfo, options });
                }
                //object daat
                else
                    InitializeVariable<ObjectVariable<T>>(typeGoInfo, options);
                return typeGoInfo;
            }
        }

        /// <summary>
        /// Generate default variables to an option
        /// </summary>
        /// <param name="options"></param>
        public static void GenerateDefaultVariables(ITypeGo options)
        {
            Generate<DateTime>(options);
            Generate<uint>(options);
            Generate<long>(options);
            Generate<short>(options);
            Generate<byte>(options);
            Generate<double>(options);
            Generate<float>(options);
            Generate<decimal>(options);
            Generate<sbyte>(options);
            Generate<ulong>(options);
            Generate<bool>(options);
            Generate<ushort>(options);
            Generate<int>(options);
            Generate<byte[]>(options);
            Generate<int[]>(options);
            Generate<string>(options);
            Generate<string[]>(options);
        }
    }
}
