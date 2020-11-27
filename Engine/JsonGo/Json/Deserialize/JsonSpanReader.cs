using JsonGo.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Json.Deserialize
{
    /// <summary>
    /// Fast struct to read json data
    /// </summary>
    public ref struct JsonSpanReader
    {
        /// <summary>
        /// is struct read finisnhed or not
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return _Index >= _Length;
            }
        }
        private int _Length;

        private ReadOnlySpan<char> _buffer;
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
        /// Reads a character
        /// </summary>
        /// <returns></returns>
        public char Read()
        {
            do
            {
                _Index++;
                if (_buffer[_Index] != JsonConstantsString.Space && _buffer[_Index] != JsonConstantsString.RSpace
                    && _buffer[_Index] != JsonConstantsString.NSpace && _buffer[_Index] != JsonConstantsString.TSpace)
                    return _buffer[_Index];
            }
            while (true);
        }

        /// <summary>
        /// Moves back _Index of 1 position
        /// </summary>
        public void BackIndex()
        {
            _Index--;
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
                if (readOnlySpan[i] == JsonConstantsString.Quotes && readOnlySpan[i - 1] != JsonConstantsString.BackSlash)
                {
                    _Index += i + 1;
                    Array.Resize(ref result, writeIndex);
                    return result.AsSpan();
                }
                else if (readOnlySpan[i] == JsonConstantsString.BackSlash && readOnlySpan[i + 1] == JsonConstantsString.Quotes)
                    continue;
                else if (readOnlySpan[i] == JsonConstantsString.BackSlash && readOnlySpan[i + 1] == JsonConstantsString.RNewLine)
                {
                    result[writeIndex] = '\r';
                    i++;
                    writeIndex++;
                }
                else if (readOnlySpan[i] == JsonConstantsString.BackSlash && readOnlySpan[i + 1] == JsonConstantsString.NNewLine)
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

        /// <summary>
        /// Extract json string surronded by quotes
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = 0;
            ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index + 1, _Length - _Index);
            for (int i = 0; i < readOnlySpan.Length - 1; i++)
            {
                if (readOnlySpan[i] == JsonConstantsString.Quotes && readOnlySpan[i - 1] != JsonConstantsString.BackSlash)
                {
                    _Index += i + 1;
                    stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                    return stringBuilder.ToString().AsSpan();
                }
                else if (readOnlySpan[i] == JsonConstantsString.BackSlash)
                {
                    if (readOnlySpan[i + 1] == JsonConstantsString.Quotes)
                    {
                        stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                        i++;
                        index = i;
                    }
                    else if (readOnlySpan[i + 1] == JsonConstantsString.RNewLine)
                    {
                        stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                        stringBuilder.Append(JsonConstantsString.RSpace);
                        i++;
                        index = i + 1;
                    }
                    else if (readOnlySpan[i + 1] == JsonConstantsString.NNewLine)
                    {
                        stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                        stringBuilder.Append(JsonConstantsString.NSpace);
                        i++;
                        index = i + 1;
                    }
                    else if (readOnlySpan[i + 1] == JsonConstantsString.TTabLine)
                    {
                        stringBuilder.Append(readOnlySpan.Slice(index, i - index));
                        stringBuilder.Append(JsonConstantsString.TSpace);
                        i++;
                        index = i + 1;
                    }
                }
            }
            _Index = _Length;
            if (readOnlySpan[readOnlySpan.Length - 1] == JsonConstantsString.Quotes)
                stringBuilder.Append(readOnlySpan.Slice(index, readOnlySpan.Length - index - 1));
            else
                stringBuilder.Append(readOnlySpan.Slice(index, readOnlySpan.Length));
            return stringBuilder.ToString().AsSpan();
        }
        /// <summary>
        /// Extracts string with quotes
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractStringQuotes()
        {
            ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index, _Length - _Index);

            for (int i = 1; i < readOnlySpan.Length; i++)
            {
                if (readOnlySpan[i] == JsonConstantsString.Quotes && readOnlySpan[i - 1] != JsonConstantsString.BackSlash)
                {
                    _Index += i + 1;
                    return readOnlySpan.Slice(0, i + 1);
                }
            }
            _Index = _Length;
            return readOnlySpan.Slice(0, _Length);
        }

        /// <summary>
        /// Extracts value from json
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractValue()
        {
            int start = _Index;
            while (_Index < _Length)
            {
                //UnSupportedValue = ' ', ',', '\r', '\n', '\t' 
                if (_buffer[_Index] == JsonConstantsString.Space || _buffer[_Index] == JsonConstantsString.Comma || _buffer[_Index] == JsonConstantsString.RSpace || _buffer[_Index] == JsonConstantsString.NSpace
                    || _buffer[_Index] == JsonConstantsString.TSpace)
                {
                    _Index--;
                    return _buffer.Slice(start, _Index - start + 1);
                }
                //EndsValues = '}', ']'
                else if (_buffer[_Index] == JsonConstantsString.CloseBracket || _buffer[_Index] == JsonConstantsString.CloseSquareBrackets)
                {
                    _Index--;
                    return _buffer.Slice(start, _Index - start + 1);
                }
                _Index++;
            }
            return _buffer.Slice(start, _Index - start + 1);
        }

        /// <summary>
        /// Extract json key
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> ExtractKey()
        {
            Read();
            int start = _Index;
            while (true)
            {
                _Index++;
                if (_buffer[_Index] == JsonConstantsString.Quotes)
                    break;
            }
            return _buffer.Slice(start, _Index - start);
        }
    }
}
