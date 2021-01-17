using BinaryGo.Binary.Deserialize;
using BinaryGo.IO;
using BinaryGo.Json;
using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryGo.Interfaces
{
    /// <summary>
    /// This interface is needed to initialize method for TypeGo
    /// </summary>
    public interface ISerializationVariable<TType>
    {
        /// <summary>
        /// json serialize as string text
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        void JsonSerialize(ref JsonSerializeHandler handler, ref TType value);
        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        TType JsonDeserialize(ref ReadOnlySpan<char> text);
        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        void BinarySerialize(ref BufferBuilder stream, ref TType value);
        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        TType BinaryDeserialize(ref BinarySpanReader reader);
    }
}
