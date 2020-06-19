using JsonGo.Json.Deserialize;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonGo.Binary.Deserialize;
using JsonGo.IO;

namespace JsonGo.Runtime
{
    /// <summary>
    /// get value from json
    /// </summary>
    /// <param name="deserializer"></param>
    /// <param name="typeGo"></param>
    /// <returns></returns>
    public delegate TProperty JsonGetValue<TProperty, TObject>(ref JsonSerializeHandler deserializer, TObject typeGo);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="deserializer"></param>
    ///// <param name="typeGo"></param>
    ///// <param name="_buffer"></param>
    ///// <returns></returns>
    ///public delegate object FastExtractFunction(JsonDeserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader _buffer);
    /// <summary>
    /// Tries to get value from a dictionary
    /// This is a pointer delegate of a method that can be accessed easy from everywhere
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public delegate bool TryGetValue<TKey>(TKey key, out object result);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="readOnlySpan"></param>
    /// <returns></returns>
    public delegate void CharsBufferBuilderFunc(ReadOnlySpan<char> readOnlySpan);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public delegate void CharBufferBuilderFunc(char data);
    /// <summary>
    /// remove char
    /// </summary>
    /// <param name="data"></param>
    public delegate void RemoveCharBufferBuilderFunc(char data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="readOnlySpan"></param>
    /// <returns></returns>
    public delegate ReadOnlySpan<byte> RefFuncByte(ReadOnlySpan<byte> readOnlySpan);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="readOnlySpan"></param>
    /// <returns></returns>
    public delegate ReadOnlySpan<byte> RefFuncChar(ReadOnlySpan<char> readOnlySpan);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="readOnlySpan"></param>
    public delegate void RefActionByte(ReadOnlySpan<byte> readOnlySpan);
    /// <summary>
    /// one byte write
    /// </summary>
    /// <param name="readOnlySpan"></param>
    public delegate void RefOneByte(byte readOnlySpan);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="readOnlySpan"></param>
    //public delegate Memory<byte> RefFuncChar(ReadOnlySpan<byte> readOnlySpan);
    /// <summary>
    /// Action to serialize object to json string
    /// </summary>
    /// <param name="handler">string serializer handler</param>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void JsonActionGo<T>(ref JsonSerializeHandler handler, ref T data);
    /// <summary>
    /// Function to serialize object to json string
    /// </summary>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized string</returns>
    public delegate ReadOnlySpan<char> JsonFunctionGo<T>(ref T data);
    /// <summary>
    /// Function to serialize object as binary
    /// </summary>
    /// <param name="handler">binary serializer handler</param>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>
    public delegate void JsonBinaryFunctionGo<T>(ref JsonBinarySerializeHandler handler, ref T data);

    /// <summary>
    /// Function to serialize object
    /// </summary>
    /// <param name="typeGoInfo"></param>
    /// <param name="handler"></param>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder,
    public delegate void JsonFunctionTypeGo<T>(TypeGoInfo<T> typeGoInfo, JsonSerializeHandler handler, ref T data);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deserializer"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate object DeserializeFunc(JsonDeserializer deserializer, ReadOnlySpan<char> data);
    /// <summary>
    /// Function to serialize object as binary
    /// </summary>
    /// <param name="typeGoInfo"></param>
    /// <param name="stream"></param>
    /// <param name="data"></param>
    public delegate void BinaryFunctionTypeGo<T>(TypeGoInfo<T> typeGoInfo, Stream stream, ref T data);
    /// <summary>
    /// Binary serializer
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="data"></param>
    public delegate void BinaryFunctionGo(Stream stream, ref object data);
    /// <summary>
    /// deserialize span reader to object
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate object BinaryDeserializeFunc(ref BinarySpanReader reader);
}
