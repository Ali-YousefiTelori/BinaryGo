using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.Binary.Deserialize
{
    /// <summary>
    /// Fast struct to read binary data
    /// </summary>
    public ref struct BinarySpanReader
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

        private ReadOnlySpan<byte> _buffer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public BinarySpanReader(ReadOnlySpan<byte> buffer) : this()
        {
            _buffer = buffer;
            _Length = _buffer.Length;
        }

        private int _Index;

        /// <summary>
        /// Reads binary range
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<byte> Read(int length)
        {
            var result = _buffer.Slice(_Index, length);
            _Index += length;
            return result;
        }

        /// <summary>
        /// read one byte
        /// </summary>
        /// <returns></returns>
        public byte Read()
        {
            var result = _buffer.Slice(_Index, 1);
            _Index++;
            return result[0];
        }
    }
}
