using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Helpers
{
    public class TextHelper
    {
        public static readonly UTF8Encoding _UTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
        public static string SpanToString(ReadOnlySpan<byte> utf8Unescaped)
        {
            return _UTF8Encoding.GetString(utf8Unescaped.ToArray());
            //unsafe
            //{
            //    fixed (byte* bytePtr = utf8Unescaped)
            //    {
            //        return _UTF8Encoding.GetString(bytePtr, utf8Unescaped.Length);
            //    }
            //}
        }

        public static ReadOnlySpan<byte> StringToSpanByteArray(ref string text)
        {
            //unsafe
            //{
            //    fixed (char* bytePtr = text)
            //    {
            //        var bytesArray = new byte[_UTF8Encoding.GetByteCount(bytePtr, text.Length)];
            //        fixed (byte* bytes = bytesArray)
            //        {
            //            _UTF8Encoding.GetBytes(bytePtr, text.Length, bytes, 1024);
            //            return bytesArray.AsSpan();
            //        }
            //    }
            //}
            return _UTF8Encoding.GetBytes(text).AsSpan();
        }
    }
}
