using JsonGo.Binary.Deserialize;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// base of property
    /// </summary>
    public abstract class BasePropertyGoInfo<TObject>
    {
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
        /// <summary>
        /// Gets property value
        /// </summary>
        internal abstract object InternalGetValue(ref TObject instance);
        /// <summary>
        /// Set value of property
        /// </summary>
        internal abstract void InternalSetValue(ref TObject instance, ref object value);
        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        internal abstract void JsonSerialize(ref JsonSerializeHandler handler, ref object value);
        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        internal abstract object JsonDeserialize(ref ReadOnlySpan<char> text);

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        internal abstract void BinarySerialize(ref BufferBuilder<byte> stream, ref object value);

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        internal abstract object BinaryDeserialize(ref BinarySpanReader reader);
    }
}
