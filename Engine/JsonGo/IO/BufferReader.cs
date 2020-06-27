using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.IO
{
    /// <summary>
    /// fast way to read buffer from memory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public ref struct BufferReader<T> 
    {
        /// <summary>
        /// set your capacity as default size without allocate memory always
        /// </summary>
        /// <param name="buffer"></param>
        public BufferReader(Span<T> buffer) : this()
        {
            _Buffer = buffer;
        }

        /// <summary>
        /// position of read buffer
        /// </summary>
        int _position;

        Span<T> _Buffer;

        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="length"></param>
        public Span<T> Read(int length)
        {
            var result = _Buffer.Slice(_position)
            _position += length;
        }

        /// <summary>
        /// get span of buffer
        /// </summary>
        /// <returns></returns>
        public Span<T> ToSpan()
        {
            return _Buffer;
        }
    }
}
