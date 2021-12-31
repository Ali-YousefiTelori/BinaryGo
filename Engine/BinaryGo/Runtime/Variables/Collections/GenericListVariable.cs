using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGo.Runtime.Variables.Collections
{
    /// <summary>
    /// variable of list or IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericListVariable<T> : BaseVariable, ISerializationVariable<List<T>>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public GenericListVariable() : base(typeof(List<T>))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="arrayTypeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<List<T>> arrayTypeGoInfo, ITypeOptions options)
        {
            arrayTypeGoInfo.IsNoQuotesValueType = false;
            //set the default value of variable
            arrayTypeGoInfo.DefaultValue = default;

            if (TryGetValueOfTypeGo(typeof(T), out object result))
                typeGoInfo = (TypeGoInfo<T>)result;
            else
                typeGoInfo = BaseTypeGoInfo.Generate<T>(Options);

            arrayTypeGoInfo.JsonSerialize = JsonSerialize;

            //set delegates to access faster and make it pointer directly usage for json deserializer
            arrayTypeGoInfo.JsonDeserialize = JsonDeserialize;

            //set delegates to access faster and make it pointer directly usage for binary serializer
            arrayTypeGoInfo.BinarySerialize = BinarySerialize;

            //set delegates to access faster and make it pointer directly usage for binary deserializer
            arrayTypeGoInfo.BinaryDeserialize = BinaryDeserialize;

            //create instance of object
            //arrayTypeGoInfo.CreateInstance = ReflectionHelper.GetActivator<TObject>(baseType);

            CastToArray = arrayTypeGoInfo.Cast;
        }

        Func<T[], object> CastToArray;
        //type of one of element
        TypeGoInfo<T> typeGoInfo = null;

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref List<T> value)
        {
            if (value != null)
            {
                handler.TextWriter.Write(JsonConstantsString.OpenSquareBrackets);
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    T obj = value[i];
                    typeGoInfo.JsonSerialize(ref handler, ref obj);
                    handler.TextWriter.Write(JsonConstantsString.Comma);
                }

                handler.TextWriter.RemoveLast(JsonConstantsString.Comma);
                handler.TextWriter.Write(JsonConstantsString.CloseSquareBrackets);
            }
            else
            {
                handler.TextWriter.Write(JsonConstantsString.Null);
            }
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public List<T> JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            throw new NotSupportedException();
            //List<T> array = new List<T>();
            //while (true)
            //{
            //    var character = text.Read();
            //    if (character == JsonConstantsString.OpenBraket)
            //    {
            //        var obj = ExtractOject(deserializer, generic, ref json);
            //        typeGo.AddArrayValue(arrayInstance, obj);
            //    }
            //    else if (character == JsonConstantsString.OpenSquareBrackets)
            //    {
            //        var obj = ExtractArray(deserializer, generic, ref json);
            //        typeGo.AddArrayValue(arrayInstance, obj);
            //    }
            //    else if (character == JsonConstantsString.Comma)
            //    {
            //        continue;
            //    }
            //    else if (character == JsonConstantsString.CloseSquareBrackets)
            //    {
            //        break;
            //    }
            //    else if (character == JsonConstantsString.Quotes)
            //    {
            //        var value = json.ExtractString();
            //        typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
            //    }
            //    else
            //    {
            //        bool isClosed = false;
            //        var value = json.ExtractValue();
            //        if (value[value.Length - 1] == JsonConstantsString.Comma)
            //            value = value.Slice(0, value.Length - 1);
            //        if (value[value.Length - 1] == JsonConstantsString.CloseSquareBrackets)
            //        {
            //            value = value.Slice(0, value.Length - 1);
            //            isClosed = true;
            //        }
            //        if (generic.JsonDeserialize != null)
            //            typeGo.AddArrayValue(arrayInstance, generic.JsonDeserialize(deserializer, value));
            //        if (isClosed)
            //            break;
            //    }
            //}
            //return typeGo.Cast == null ? arrayInstance : typeGo.Cast(arrayInstance);
            //return array.ToArray();
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref List<T> value)
        {
            if (value.Count > 0)
            {
                stream.Write(BitConverter.GetBytes(value.Count));
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    T obj = value[i];
                    typeGoInfo.BinarySerialize(ref stream, ref obj);
                }
            }
            else
            {
                stream.Write(BitConverter.GetBytes(0));
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public List<T> BinaryDeserialize(ref BinarySpanReader reader)
        {
            int length = BitConverter.ToInt32(reader.Read(sizeof(int)));
            if (length == 0)
                return null;
            List<T> instance = new List<T>();
            for (int i = 0; i < length; i++)
            {
                instance.Add(typeGoInfo.BinaryDeserialize(ref reader));
            }
            return instance;
        }
    }
}
