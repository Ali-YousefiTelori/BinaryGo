﻿using BinaryGo.Binary.Deserialize;
using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Json.Deserialize;
using System;
using System.Collections.Generic;

namespace BinaryGo.Runtime
{
    /// <summary>
    /// base of property
    /// </summary>
    public abstract class BasePropertyGoInfo<TObject>
    {
        /// <summary>
        /// index of property to serialize
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Property type
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// base of typego of property
        /// </summary>
        public BaseTypeGoInfo BaseTypeGoInfo { get; set; }
        /// <summary>
        /// index of cache data property
        /// </summary>
        public int IndexToWrite;
        /// <summary>
        /// Property Name
        /// </summary>
        public string Name;
        /// <summary>
        /// serialized name of json property
        /// </summary>
        public string NameSerialized;
        /// <summary>
        /// byte array of name
        /// </summary>
        public byte[] NameBytes;
        /// <summary>
        /// default value of object
        /// </summary>
        public object DefaultValue;
        ///// <summary>
        ///// Gets property value
        ///// </summary>
        //internal abstract object InternalGetValue(ref TObject instance);
        ///// <summary>
        ///// Set value of property
        ///// </summary>
        //internal abstract void InternalSetValue(ref TObject instance, ref object value);
        ///// <summary>
        ///// json serialize
        ///// </summary>
        ///// <param name="handler"></param>
        ///// <param name="value"></param>
        //internal abstract void JsonSerialize(ref JsonSerializeHandler handler, ref object value);
        /// <summary>
        /// type safe json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        internal abstract void TypedJsonSerialize(ref JsonSerializeHandler handler, ref TObject value);
        /// <summary>
        /// deserialize json object
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="deserializer"></param>
        /// <param name="reader"></param>
        internal abstract void JsonDeserializeObject(ref TObject instance, ref JsonDeserializer deserializer, ref JsonSpanReader reader);
        /// <summary>
        /// deserialize json array
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="deserializer"></param>
        /// <param name="reader"></param>
        internal abstract void JsonDeserializeArray(ref TObject instance, ref JsonDeserializer deserializer, ref JsonSpanReader reader);

        /// <summary>
        /// json deserialize text string inside of double quats
        /// </summary>
        /// <param name="instance">instance of property to set value</param>
        /// <param name="reader">json text reader</param>
        internal abstract void JsonDeserializeString(ref TObject instance, ref JsonSpanReader reader);
        /// <summary>
        /// json deserialize values of number or bool
        /// </summary>
        /// <param name="instance">instance of property to set value</param>
        /// <param name="reader">json text reader</param>
        internal abstract void JsonDeserializeValue(ref TObject instance, ref JsonSpanReader reader);

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        internal abstract void BinarySerialize(ref BufferBuilder stream, ref TObject value);

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        /// <param name="value"></param>
        internal abstract void BinaryDeserialize(ref BinarySpanReader reader, ref TObject value);

        /// <summary>
        /// Get Binary member by property
        /// </summary>
        /// <returns></returns>
        internal abstract MemberBinaryModelInfo GetBinaryMember(BaseOptionInfo option, Dictionary<Type, BinaryModelInfo> generatedModels);
    }
}
