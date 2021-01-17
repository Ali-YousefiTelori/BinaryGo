using BinaryGo.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryGo.Json
{
    /// <summary>
    /// Json binary (byte array) serializer handler to use serialization and deserialization methods faster
    /// this class will save data to memoryStream
    /// </summary>
    public ref struct JsonBinarySerializeHandler
    {
        /// <summary>
        /// make a fast serializer handler
        /// </summary>
        /// <param name="_EncodingGetBytes"></param>
        /// <param name="_RemoveLastCommaCharacter"></param>
        /// <param name="_Append"></param>
        /// <param name="_AppendByte"></param>
        /// <param name="_Serializer"></param>
        /// <param name="_AddSerializedObjects"></param>
        /// <param name="_TryGetValueOfSerializedObjects"></param>
        public JsonBinarySerializeHandler(RefFuncChar _EncodingGetBytes, Action _RemoveLastCommaCharacter, RefActionByte _Append, RefOneByte _AppendByte
            , Serializer _Serializer, Action<object, int> _AddSerializedObjects, TryGetValue<object> _TryGetValueOfSerializedObjects) : this()
        {
            EncodingGetBytes = _EncodingGetBytes;
            //RemoveLastCommaCharacter = _RemoveLastCommaCharacter;
            Append = _Append;
            AppendByte = _AppendByte;
            Serializer = _Serializer;
            AddSerializedObjects = _AddSerializedObjects;
            TryGetValueOfSerializedObjects = _TryGetValueOfSerializedObjects;
        }

        /// <summary>
        /// default encoding of serializer
        /// </summary>
        public readonly RefFuncChar EncodingGetBytes;
        ///// <summary>
        ///// remove ',' caracter in ends of text
        ///// </summary>
        //public readonly Action RemoveLastCommaCharacter;
        /// <summary>
        /// Appends bytes
        /// </summary>
        public readonly RefActionByte Append;
        /// <summary>
        /// appends a byte
        /// </summary>
        public readonly RefOneByte AppendByte;
        /// <summary>
        /// Serializer
        /// </summary>
        public readonly Serializer Serializer;
        /// <summary>
        /// Adds object to serialized for references
        /// </summary>
        public readonly Action<object, int> AddSerializedObjects;
        /// <summary>
        /// Finds serialization object for reference values
        /// </summary>
        public readonly TryGetValue<object> TryGetValueOfSerializedObjects;
    }
}
