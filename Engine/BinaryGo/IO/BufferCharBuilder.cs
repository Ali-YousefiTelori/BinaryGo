using BinaryGo.Runtime.Variables.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGo.IO
{
    /// <summary>
    /// fast buffer stream 
    /// </summary>
    public ref struct BufferCharBuilder
    {
        /// <summary>
        /// set your capacity as default size without allocate memory always
        /// </summary>
        /// <param name="capacity"></param>
        public BufferCharBuilder(int capacity) : this()
        {
            _capacity = capacity;
            _Buffer = new char[capacity];
        }


        int _capacity;
        char[] _Buffer;

        /// <summary>
        /// length of buffer
        /// </summary>
        public int Length;

        /// <summary>
        /// check if size of buffer is full make space for it
        /// </summary>
        public void Resize()
        {
            char[] newBuffer = new char[_Buffer.Length + _capacity];
            _Buffer.CopyTo(new Span<char>(newBuffer, 0, _Buffer.Length));
            _Buffer = newBuffer;
        }

        /// <summary>
        /// add new chars to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(Span<char> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<char>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// add new chars to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ref ReadOnlySpan<char> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<char>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// add new chars to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ReadOnlySpan<char> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<char>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// write single data
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref char data)
        {
            if (Length + 1 > _Buffer.Length)
            {
                _capacity += 1;
                Resize();
            }
            _Buffer[Length] = data;
            Length++;
        }

        /// <summary>
        /// write single data
        /// </summary>
        /// <param name="data"></param>
        public void Write(char data)
        {
            if (Length + 1 > _Buffer.Length)
            {
                _capacity += 1;
                Resize();
            }
            _Buffer[Length] = data;
            Length++;
        }

        #region Write Direct Struct

        /// <summary>
        /// write a bool struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref bool data)
        {
            if (Length + 5 > _Buffer.Length)
            {
                _capacity += 5;
                Resize();
            }
            if (data)
            {
                _Buffer[Length] = 'T';
                _Buffer[Length + 1] = 'r';
                _Buffer[Length + 2] = 'u';
                _Buffer[Length + 3] = 'e';
            }
            else
            {
                _Buffer[Length] = 'F';
                _Buffer[Length + 1] = 'a';
                _Buffer[Length + 2] = 'l';
                _Buffer[Length + 3] = 's';
                _Buffer[Length + 3] = 'e';
            }
            Length += 5;
        }

        #endregion

        /// <summary>
        /// remove last data if was equal
        /// </summary>
        /// <param name="data"></param>
        public void RemoveLast(char data)
        {
            if (_Buffer[Length - 1].Equals(data))
                Length--;
        }

        /// <summary>
        /// get span of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public Span<char> ToSpan()
        {
            return _Buffer;
        }

        /// <summary>
        /// get array of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public char[] ToArray()
        {
            return _Buffer;
        }
    }
}
