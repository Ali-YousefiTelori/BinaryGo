using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.IO
{
    /// <summary>
    /// fast buffer stream 
    /// </summary>
    public ref struct BufferBuilder<T>
    {
        /// <summary>
        /// set your capacity as default size without allocate memory always
        /// </summary>
        /// <param name="capacity"></param>
        public BufferBuilder(int capacity) : this()
        {
            _capacity = capacity;
            _Buffer = new T[capacity];
        }


        int _capacity;
        T[] _Buffer;

        /// <summary>
        /// length of buffer
        /// </summary>
        public int Length;
        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(Span<T> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                T[] newBuffer = new T[_Buffer.Length + _capacity];
                _Buffer.CopyTo(new Span<T>(newBuffer, 0, _Buffer.Length));
                _Buffer = newBuffer;
            }
            buffer.CopyTo(new Span<T>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }
        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ref ReadOnlySpan<T> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                T[] newBuffer = new T[_Buffer.Length + _capacity];
                _Buffer.CopyTo(new Span<T>(newBuffer, 0, _Buffer.Length));
                _Buffer = newBuffer;
            }
            buffer.CopyTo(new Span<T>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ReadOnlySpan<T> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                T[] newBuffer = new T[_Buffer.Length + _capacity];
                _Buffer.CopyTo(new Span<T>(newBuffer, 0, _Buffer.Length));
                _Buffer = newBuffer;
            }
            buffer.CopyTo(new Span<T>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// write single data
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref T data)
        {
            if (Length + 1 > _Buffer.Length)
            {
                _capacity++;
                T[] newBuffer = new T[_Buffer.Length + _capacity];
                _Buffer.CopyTo(new Span<T>(newBuffer, 0, _Buffer.Length));
                _Buffer = newBuffer;
            }
            _Buffer[Length] = data;
            Length++;
        }

        /// <summary>
        /// write single data
        /// </summary>
        /// <param name="data"></param>
        public void Write(T data)
        {
            if (Length + 1 > _Buffer.Length)
            {
                _capacity++;
                T[] newBuffer = new T[_Buffer.Length + _capacity];
                _Buffer.CopyTo(new Span<T>(newBuffer, 0, _Buffer.Length));
                _Buffer = newBuffer;
            }
            _Buffer[Length] = data;
            Length++;
        }

        /// <summary>
        /// remove last data if was equal
        /// </summary>
        /// <param name="data"></param>
        public void RemoveLast(T data)
        {
            if (_Buffer[Length - 1].Equals(data))
                Length--;
        }

        /// <summary>
        /// get span of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public Span<T> ToSpan()
        {
            return _Buffer;
        }

        /// <summary>
        /// get array of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _Buffer;
        }
    }
}
