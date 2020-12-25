using JsonGo.Runtime.Variables.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo.IO
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

        ///// <summary>
        ///// write a long struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref long data)
        //{
        //    if (Length + 8 > _Buffer.Length)
        //    {
        //        _capacity += 8;
        //        Resize();
        //    }
        //    var value = new LongStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    _Buffer[Length + 4] = value.Byte4;
        //    _Buffer[Length + 5] = value.Byte5;
        //    _Buffer[Length + 6] = value.Byte6;
        //    _Buffer[Length + 7] = value.Byte7;
        //    Length += 8;
        //}

        ///// <summary>
        ///// write a ulong struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref ulong data)
        //{
        //    if (Length + 8 > _Buffer.Length)
        //    {
        //        _capacity += 8;
        //        Resize();
        //    }
        //    var value = new ULongStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    _Buffer[Length + 4] = value.Byte4;
        //    _Buffer[Length + 5] = value.Byte5;
        //    _Buffer[Length + 6] = value.Byte6;
        //    _Buffer[Length + 7] = value.Byte7;
        //    Length += 8;
        //}

        ///// <summary>
        ///// write a short struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref short data)
        //{
        //    if (Length + 2 > _Buffer.Length)
        //    {
        //        _capacity += 2;
        //        Resize();
        //    }
        //    var value = new ShortStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    Length += 2;
        //}

        ///// <summary>
        ///// write a ushort struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref ushort data)
        //{
        //    if (Length + 2 > _Buffer.Length)
        //    {
        //        _capacity += 2;
        //        Resize();
        //    }
        //    var value = new UShortStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    Length += 2;
        //}

        ///// <summary>
        ///// write a int struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref int data)
        //{
        //    if (Length + 4 > _Buffer.Length)
        //    {
        //        _capacity += 4;
        //        Resize();
        //    }
        //    var value = new IntStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    Length += 4;
        //}

        ///// <summary>
        ///// write a uint struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref uint data)
        //{
        //    if (Length + 4 > _Buffer.Length)
        //    {
        //        _capacity += 4;
        //        Resize();
        //    }
        //    var value = new UIntStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    Length += 4;
        //}

        ///// <summary>
        ///// write a float struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref float data)
        //{
        //    if (Length + 4 > _Buffer.Length)
        //    {
        //        _capacity += 4;
        //        Resize();
        //    }
        //    var value = new FloatStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    Length += 4;
        //}

        ///// <summary>
        ///// write a double struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref double data)
        //{
        //    if (Length + 8 > _Buffer.Length)
        //    {
        //        _capacity += 8;
        //        Resize();
        //    }
        //    var value = new DoubleStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    _Buffer[Length + 4] = value.Byte4;
        //    _Buffer[Length + 5] = value.Byte5;
        //    _Buffer[Length + 6] = value.Byte6;
        //    _Buffer[Length + 7] = value.Byte7;
        //    Length += 8;
        //}

        ///// <summary>
        ///// write a decimal struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref decimal data)
        //{
        //    if (Length + 16 > _Buffer.Length)
        //    {
        //        _capacity += 16;
        //        Resize();
        //    }
        //    var value = new DecimalStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    _Buffer[Length + 4] = value.Byte4;
        //    _Buffer[Length + 5] = value.Byte5;
        //    _Buffer[Length + 6] = value.Byte6;
        //    _Buffer[Length + 7] = value.Byte7;
        //    _Buffer[Length + 8] = value.Byte8;
        //    _Buffer[Length + 9] = value.Byte9;
        //    _Buffer[Length + 10] = value.Byte10;
        //    _Buffer[Length + 11] = value.Byte11;
        //    _Buffer[Length + 12] = value.Byte12;
        //    _Buffer[Length + 13] = value.Byte13;
        //    _Buffer[Length + 14] = value.Byte14;
        //    _Buffer[Length + 15] = value.Byte15;
        //    Length += 16;
        //}

        ///// <summary>
        ///// write a guid struct
        ///// </summary>
        ///// <param name="data"></param>
        //public void Write(ref Guid data)
        //{
        //    if (Length + 16 > _Buffer.Length)
        //    {
        //        _capacity += 16;
        //        Resize();
        //    }
        //    var value = new GuidStruct() { Value = data };
        //    _Buffer[Length] = value.Byte0;
        //    _Buffer[Length + 1] = value.Byte1;
        //    _Buffer[Length + 2] = value.Byte2;
        //    _Buffer[Length + 3] = value.Byte3;
        //    _Buffer[Length + 4] = value.Byte4;
        //    _Buffer[Length + 5] = value.Byte5;
        //    _Buffer[Length + 6] = value.Byte6;
        //    _Buffer[Length + 7] = value.Byte7;
        //    _Buffer[Length + 8] = value.Byte8;
        //    _Buffer[Length + 9] = value.Byte9;
        //    _Buffer[Length + 10] = value.Byte10;
        //    _Buffer[Length + 11] = value.Byte11;
        //    _Buffer[Length + 12] = value.Byte12;
        //    _Buffer[Length + 13] = value.Byte13;
        //    _Buffer[Length + 14] = value.Byte14;
        //    _Buffer[Length + 15] = value.Byte15;
        //    Length += 16;
        //}
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
