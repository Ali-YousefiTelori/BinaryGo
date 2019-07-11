using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonGo.Deserialize
{
    public ref struct JsonSpanReader
    {
        static byte[] SupportedValue { get; set; } = "0123456789.truefalsTRUEFALS-n".Select(x => (byte)x).ToArray();

        #region white space
        static byte BSpace { get; set; } = (byte)'\b';
        static byte FSpace { get; set; } = (byte)'\f';
        static byte RSpace { get; set; } = (byte)'\r';
        static byte NSpace { get; set; } = (byte)'\n';
        static byte TSpace { get; set; } = (byte)'\t';
        static byte Space { get; set; } = (byte)' ';
        #endregion

        public bool IsFinished
        {
            get
            {
                return Index >= Length;
            }
        }
        public int Length { get; set; }

        private ReadOnlySpan<byte> _buffer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public JsonSpanReader(ReadOnlySpan<byte> buffer) : this()
        {
            _buffer = buffer;
            Index = -1;
            Length = _buffer.Length - 1;
        }

        public int Index { get; set; }

        public byte Read()
        {
            byte character = default;
            while (true)
            {
                Index++;
                character = _buffer[Index];
                if (character == Space || character == BSpace
                    || character == FSpace || character == RSpace
                    || character == NSpace || character == TSpace)
                    continue;
                else
                    break;
            }
            return character;
        }

        public byte ReadWithoutSkip()
        {
            Index++;
            return _buffer[Index];
        }

        public JsonSpanReader ExtractString()
        {
            int start = Index + 1;
            byte lastChar = default;
            while (true)
            {
                var character = ReadWithoutSkip();
                if (character == JsonSettingInfo.Quotes && lastChar != JsonSettingInfo.BackSlash)
                    break;
                lastChar = character;
            }
            return new JsonSpanReader(_buffer.Slice(start, Index - start));
        }
        /// <summary>
        /// extract value from json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public JsonSpanReader ExtractValue()
        {
            int start = Index;
            while (true)
            {
                var character = ReadWithoutSkip();
                if (!SupportedValue.Contains(character))
                    break;
            }

            return new JsonSpanReader(_buffer.Slice(start, Index - start));
        }
        public JsonSpanReader ExtractKey()
        {
            Read();
            int start = Index;
            while (true)
            {
                var character = ReadWithoutSkip();
                if (character == JsonSettingInfo.Quotes)
                    break;
            }

            return new JsonSpanReader(_buffer.Slice(start, Index - start));
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_buffer.ToArray());
        }
    }
}
