﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    public ref struct JsonSpanReader
    {
        //static byte[] SupportedValue { get; set; } = "0123456789.truefalsTRUEFALS-n".Select(x => (byte)x).ToArray();
        static byte[] UnSupportedValue { get; set; } = " ,\r\n\t".Select(x => (byte)x).ToArray();
        static byte[] EndsValue { get; set; } = "}]".Select(x => (byte)x).ToArray();
        static byte[] SkipValues { get; set; } = " \r\n\t".Select(x => (byte)x).ToArray();

        #region white space
        //static byte BSpace { get; set; } = (byte)'\b';
        //static byte FSpace { get; set; } = (byte)'\f';
        static byte RSpace { get; set; } = (byte)'\r';
        static byte NSpace { get; set; } = (byte)'\n';
        static byte TSpace { get; set; } = (byte)'\t';
        static byte Space { get; set; } = (byte)' ';
        #endregion

        public bool IsFinished
        {
            get
            {
                return _Index >= _Length;
            }
        }
        private int _Length;

        public ReadOnlySpan<byte> _buffer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public JsonSpanReader(ReadOnlySpan<byte> buffer) : this()
        {
            _buffer = buffer;
            _Index = -1;
            _Length = _buffer.Length - 1;
        }

        private int _Index;

        public byte Read()
        {
            //ReadOnlySpan<char> readOnlySpan = _buffer.Slice(_Index + 1, Length - _Index);
            //for (; _Index < readOnlySpan.Length; _Index++)
            //{
            //    if (readOnlySpan[_Index] == Space || readOnlySpan[_Index] == RSpace
            //        || readOnlySpan[_Index] == NSpace || readOnlySpan[_Index] == TSpace)
            //        continue;
            //    else
            //        break;
            //}
            //Index++;
            //var chr = _buffer.TrimStart()[0];
            //Index += _buffer.IndexOf(chr);
            //return _buffer[Index];
            //foreach (var item in _buffer.Slice(_Index))
            //{
            //    _Index++;
            //    if (item != Space && item != RSpace
            //        && item != NSpace && item != TSpace)
            //        break;
            //}
            do
            {
                _Index++;
                //if (_buffer[_Index] != Space && _buffer[_Index] != RSpace
                //    && _buffer[_Index] != NSpace && _buffer[_Index] != TSpace)
                //    break;
                if (!SkipValues.Contains(_buffer[_Index]))
                    break;
                else
                    continue;
            }
            while (true);
            return _buffer[_Index];
        }


        public ReadOnlySpan<byte> ExtractString()
        {
            //StringBuilder builder = new StringBuilder();
            //byte last = default;
            //foreach (var item in _buffer.Slice(_Index + 1, Length - _Index))
            //{
            //    if (item == JsonSettingInfo.Quotes && last != JsonSettingInfo.BackSlash)
            //        break;
            //    builder.Append(item);
            //    last = item;
            //}
            //_Index += builder.Length;
            //return builder;
            ReadOnlySpan<byte> readOnlySpan = _buffer.Slice(_Index + 1, _Length - _Index);

            for (int i = 0; i < readOnlySpan.Length; i++)
            {
                if (readOnlySpan[i] == JsonConstants.Quotes && readOnlySpan[i - 1] != JsonConstants.BackSlash)
                {
                    _Index += i + 1;
                    return readOnlySpan.Slice(0, i);
                }
            }
            _Index = _Length;
            return readOnlySpan;
            //ReadOnlySpan<char> readOnlySpan = _buffer.Slice(Index + 1, Length - Index);
            //for (int i = 0; i < readOnlySpan.Length; i++)
            //{
            //    if (readOnlySpan[i] == JsonSettingInfo.Quotes && readOnlySpan[i - 1] != JsonSettingInfo.BackSlash)
            //    {
            //        Index += i + 1;
            //        return readOnlySpan.Slice(0, i);
            //    }
            //}
            //Index = Length;
            //return readOnlySpan;
            //int start = Index + 1;
            //for (int i = start; i < _buffer.Length; i++)
            //{
            //    if (_buffer[i] == JsonSettingInfo.Quotes && _buffer[i - 1] != JsonSettingInfo.BackSlash)
            //    {
            //        Index = i;
            //        return _buffer.Slice(start, i - start);
            //    }
            //}
            //Index = Length;
            //return _buffer;
            //while (true)
            //{
            //    Index++;
            //    if (_buffer[Index] == JsonSettingInfo.Quotes && lastChar != JsonSettingInfo.BackSlash)
            //        break;
            //    lastChar = _buffer[Index];
            //}
            //return _buffer.Slice(start, Index - start);
            //int start = Index + 1;
            //char lastChar = default;
            //while (true)
            //{
            //    Index++;
            //    if (_buffer[Index] == JsonSettingInfo.Quotes && lastChar != JsonSettingInfo.BackSlash)
            //        break;
            //    lastChar = _buffer[Index];
            //}
            //return _buffer.Slice(start, Index - start);
        }
        /// <summary>
        /// Extract string with quotes
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<byte> ExtractStringQuotes()
        {
            ReadOnlySpan<byte> readOnlySpan = _buffer.Slice(_Index, _Length - _Index);

            for (int i = 1; i < readOnlySpan.Length; i++)
            {
                if (readOnlySpan[i] == JsonConstants.Quotes && readOnlySpan[i - 1] != JsonConstants.BackSlash)
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
        public ReadOnlySpan<byte> ExtractValue()
        {
            //StringBuilder builder = new StringBuilder();
            //foreach (var item in _buffer.Slice(_Index + 1, Length - _Index))
            //{
            //    if (!SupportedValue.Contains(item))
            //        break;
            //    builder.Append(item);
            //}
            //_Index += builder.Length;
            //return builder;
            int start = _Index;
            while (true)
            {
                if (UnSupportedValue.Contains(_buffer[_Index]))
                    break;
                else if (EndsValue.Contains(_buffer[_Index]))
                {
                    _Index--;
                    return _buffer.Slice(start, _Index - start + 1);
                }
                _Index++;
            }
            return _buffer.Slice(start, _Index - start);
        }
        public ReadOnlySpan<byte> ExtractKey()
        {
            Read();
            //StringBuilder builder = new StringBuilder();
            //foreach (var item in _buffer.Slice(_Index + 1, Length - _Index))
            //{
            //    if (item == JsonSettingInfo.Quotes)
            //        break;
            //    builder.Append(item);
            //}
            //_Index += builder.Length;
            //return builder;
            int start = _Index;
            while (true)
            {
                _Index++;
                if (_buffer[_Index] == JsonConstants.Quotes)
                    break;
            }

            return _buffer.Slice(start, _Index - start);
        }

        //public override string ToString()
        //{
        //    return new string(_buffer.ToArray());
        //}

        //public bool Equals(JsonSpanReader other)
        //{
        //    if (other.Length == Length)
        //    {
        //        for (int i = 0; i < Length; i++)
        //        {
        //            if (_buffer[i] != other._buffer[i])
        //                return false;
        //        }
        //    }
        //    return false;
        //}

        //public static bool operator ==(JsonSpanReader lhs, JsonSpanReader rhs)
        //{
        //    return lhs.Equals(rhs);
        //}
        //public static bool operator !=(JsonSpanReader lhs, JsonSpanReader rhs)
        //{
        //    return !lhs.Equals(rhs);
        //}
    }
}