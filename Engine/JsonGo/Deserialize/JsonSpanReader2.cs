using JsonGo.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    public ref struct JsonSpanReader2
    {
        public static byte[] UnSupportedValue { get; set; } = " ,\r\n\t".Select(x => (byte)x).ToArray();
        public static byte[] EndsValue { get; set; } = "}]".Select(x => (byte)x).ToArray();
        public static byte[] SkipValues { get; set; } = " \r\n\t".Select(x => (byte)x).ToArray();

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
        public JsonSpanReader2(ReadOnlySpan<byte> buffer) : this()
        {
            _buffer = buffer;
        }

        private int _Index;

        //public byte Read()
        //{
        //    do
        //    {
        //        _Index++;
        //        if (!SkipValues.Contains(_buffer[_Index]))
        //            break;
        //        else
        //            continue;
        //    }
        //    while (true);
        //    return _buffer[_Index];
        //}


        public ReadOnlySpan<byte> ExtractString()
        {
            ReadOnlySpan<byte> readOnlySpan = _buffer.Slice(_Index + 1, _Length - _Index);

            for (int i = 0; i < readOnlySpan.Length; i++)
            {
                if (readOnlySpan[i] == JsonConstantsBytes.Quotes && readOnlySpan[i - 1] != JsonConstantsBytes.BackSlash)
                {
                    _Index += i + 1;
                    return readOnlySpan.Slice(0, i);
                }
            }
            _Index = _Length;
            return readOnlySpan;
        }
        /// <summary>
        /// extract value from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public ReadOnlySpan<byte> ExtractValue()
        {
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
