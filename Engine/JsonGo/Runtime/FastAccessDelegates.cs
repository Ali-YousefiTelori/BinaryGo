using JsonGo.Json.Deserialize;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deserializer"></param>
    /// <param name="typeGo"></param>
    /// <param name="_buffer"></param>
    /// <returns></returns>
    public delegate object FastExtractFunction(JsonDeserializer deserializer, TypeGoInfo typeGo, ref JsonSpanReader _buffer);
    /// <summary>
    /// try get value from a dictionary
    /// this is a pointer delegate of method to access faster from everywhere
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="key"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public delegate bool TryGetValue<TKey, TResult>(TKey key, out TResult result);
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="readOnlySpan"></param>
    /// <returns></returns>
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
    /// <param name="typeGoInfo"></param>
    /// <param name="handler"></param>
    /// <param name="data">any object to serialize</param>
    /// <returns>serialized stringbuilder</returns>TypeGoInfo typeGoInfo, Serializer serializer, StringBuilder stringBuilder,
    public delegate void JsonFunctionTypeGo(TypeGoInfo typeGoInfo, JsonSerializeHandler handler, ref object data);
    /// <summary>
    /// function for serialize object as binary
    /// </summary>
    /// <param name="typeGoInfo"></param>
    /// <param name="stream"></param>
    /// <param name="data"></param>
    public delegate void BinaryFunctionTypeGo(TypeGoInfo typeGoInfo, Stream stream, ref object data);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deserializer"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate object DeserializeFunc(JsonDeserializer deserializer, ReadOnlySpan<char> data);

}
