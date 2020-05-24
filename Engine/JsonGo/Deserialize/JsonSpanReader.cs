using JsonGo.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    public ref struct JsonSpanReader
    {
        //SkipValues = ' ', '\r', '\n', '\t'
        //EndsValues = '}', ']'
        //UnSupportedValue = ' ', ',', '\r', '\n', '\t' 
        //static byte[] SupportedValue { get; set; } = "0123456789.truefalsTRUEFALS-n".Select(x => (byte)x).ToArray();

        #region SkipValues
        public const char RSpace = '\r';
        public const char NSpace = '\n';
        public const char TSpace = '\t';
        public const char Space = ' ';
        #endregion

        #region EndsValues
        public const char CloseBracket = '}';
        public const char CloseSquareBrackets = ']';
        #endregion

        #region UnSupportedValues
        public const char Comma = ',';
        #endregion

        public const char Quotes = '"';
        public const char BackSlash = '\\';
        public const char RNewLine = 'r';
        public const char NNewLine = 'n';
        public bool IsFinished
        {
            get
            {
                return _Index >= _Length;
            }
        }
        private int _Length;

        public ReadOnlySpan<char> _buffer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public JsonSpanReader(ReadOnlySpan<char> buffer) : this()
        {
            _buffer = buffer;
            _Index = -1;
            _Length = _buffer.Length - 1;
        }

        private int _Index;

        /// <summary>
        /// Read a character
        /// </summary>
        /// <returns></returns>
        public char Read()
        {
            do
            {
                _Index++;
                if (_buffer[_Index] != Space && _buffer[_Index] != RSpace
                    && _buffer[_Index] != NSpace && _buffer[_Index] != TSpace)
                    return _buffer[_Index];
            }
            while (true);
        }

        private ReadOnlySpan<char> ExtractStringOLD()
        {
            char[] result = new char[10];
            var max = result.Length - 1;
            int writeIndex = 0;
            ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index + 1, _Length - _Index);
            for (int i = 0; i < readOnlySpan.Length - 1; i++)
            {
                if (i >= max)
                {
                    max = result.Length + 10;
                    Array.Resize(ref result, max);
                }
                if (readOnlySpan[i] == JsonConstantsBytes.Quotes && readOnlySpan[i - 1] != JsonConstantsBytes.BackSlash)
                {
                    _Index += i + 1;
                    Array.Resize(ref result, writeIndex);
                    return result.AsSpan();
                }
                else if (readOnlySpan[i] == JsonConstantsBytes.BackSlash && readOnlySpan[i + 1] == JsonConstantsBytes.Quotes)
                    continue;
                else if (readOnlySpan[i] == JsonConstantsBytes.BackSlash && readOnlySpan[i + 1] == JsonConstantsBytes.RNewLine)
                {
                    result[writeIndex] = '\r';
                    i++;
                    writeIndex++;
                }
                else if (readOnlySpan[i] == JsonConstantsBytes.BackSlash && readOnlySpan[i + 1] == JsonConstantsBytes.NNewLine)
                {
                    result[writeIndex] = '\n';
                    i++;
                    writeIndex++;
                }
                else
                {
                    result[writeIndex] = readOnlySpan[i];
                    writeIndex++;
                }

            }
            _Index = _Length;
            Array.Resize(ref result, writeIndex);

            return result.AsSpan();
        }

        public ReadOnlySpan<char> ExtractString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = 0;
            ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index + 1, _Length - _Index);
            for (int i = 0; i < readOnlySpan.Length - 1; i++)
            {
                if (readOnlySpan[i] == Quotes && readOnlySpan[i - 1] != BackSlash)
                {
                    _Index += i + 1;
                    stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                    return stringBuilder.ToString().AsSpan();
                }
                else if (readOnlySpan[i] == BackSlash && readOnlySpan[i + 1] == Quotes)
                {
                    stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                    i++;
                    index = i;
                }
                else if (readOnlySpan[i] == BackSlash && readOnlySpan[i + 1] == RNewLine)
                {
                    stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                    stringBuilder.Append(RSpace);
                    i++;
                    index = i + 1;
                }
                else if (readOnlySpan[i] == BackSlash && readOnlySpan[i + 1] == NNewLine)
                {
                    stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                    stringBuilder.Append(NSpace);
                    i++;
                    index = i + 1;
                }
            }
            _Index = _Length;
            if (readOnlySpan[readOnlySpan.Length - 1] == Quotes)
                stringBuilder.Append(readOnlySpan.Slice(index, 1));
            else
                stringBuilder.Append(readOnlySpan.Slice(index, readOnlySpan.Length));
            return stringBuilder.ToString().AsSpan();
        }
        /// <summary>
        /// Extract string with quotes
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractStringQuotes()
        {
            ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index, _Length - _Index);

            for (int i = 1; i < readOnlySpan.Length; i++)
            {
                if (readOnlySpan[i] == Quotes && readOnlySpan[i - 1] != BackSlash)
                {
                    _Index += i + 1;
                    return readOnlySpan.Slice(0, i + 1);
                }
            }
            _Index = _Length;
            return readOnlySpan.Slice(0, _Length);
        }

        /// <summary>
        /// extract value from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractValue()
        {
            int start = _Index;
            while (_Index < _Length)
            {
                //UnSupportedValue = ' ', ',', '\r', '\n', '\t' 
                if (_buffer[_Index] == Space || _buffer[_Index] == Comma || _buffer[_Index] == RSpace || _buffer[_Index] == NSpace
                    || _buffer[_Index] == TSpace)
                    break;
                //EndsValues = '}', ']'
                else if (_buffer[_Index] == CloseBracket || _buffer[_Index] == CloseSquareBrackets)
                {
                    _Index--;
                    return _buffer.Slice(start, _Index - start + 1);
                }
                _Index++;
            }
            return _buffer.Slice(start, _Index - start + 1);
        }

        /// <summary>
        /// extract json key
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractKey()
        {
            Read();
            int start = _Index;
            while (true)
            {
                _Index++;
                if (_buffer[_Index] == JsonConstantsBytes.Quotes)
                    break;
            }
            return _buffer.Slice(start, _Index - start);
        }
    }
}
