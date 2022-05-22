using BinaryGo.Runtime.Variables.Structures;
using System;

namespace BinaryGo.IO
{
    /// <summary>
    /// fast buffer stream 
    /// </summary>
    public ref struct BufferBuilder
    {
        /// <summary>
        /// set your capacity as default size without allocate memory always
        /// </summary>
        /// <param name="capacity"></param>
        public BufferBuilder(int capacity) : this()
        {
            _capacity = capacity;
            _Buffer = new byte[capacity];
        }


        int _capacity;
        byte[] _Buffer;

        /// <summary>
        /// length of buffer
        /// </summary>
        public int Length;

        /// <summary>
        /// check if size of buffer is full make space for it
        /// </summary>
        public void Resize()
        {
            byte[] newBuffer = new byte[_Buffer.Length + _capacity];
            _Buffer.CopyTo(new Span<byte>(newBuffer, 0, _Buffer.Length));
            _Buffer = newBuffer;
        }

        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(Span<byte> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<byte>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ref ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<byte>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// add new bytes to buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length + Length > _Buffer.Length)
            {
                _capacity += buffer.Length;
                Resize();
            }
            buffer.CopyTo(new Span<byte>(_Buffer, Length, buffer.Length));
            Length += buffer.Length;
        }

        /// <summary>
        /// write single data
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref byte data)
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
        public void Write(byte data)
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
            if (Length + 1 > _Buffer.Length)
            {
                _capacity += 1;
                Resize();
            }
            if (data)
                _Buffer[Length] = 1;
            else
                _Buffer[Length] = 0;
            Length++;
        }

        /// <summary>
        /// write a datetime struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref DateTime data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new LongStruct() { Value = data.Ticks };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a datetime struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref TimeSpan data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new LongStruct() { Value = data.Ticks };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a long struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref long data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new LongStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a long struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(long data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new LongStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a ulong struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref ulong data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new ULongStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a short struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref short data)
        {
            if (Length + 2 > _Buffer.Length)
            {
                _capacity += 2;
                Resize();
            }
            var value = new ShortStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            Length += 2;
        }

        /// <summary>
        /// write a ushort struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref ushort data)
        {
            if (Length + 2 > _Buffer.Length)
            {
                _capacity += 2;
                Resize();
            }
            var value = new UShortStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            Length += 2;
        }

        /// <summary>
        /// write a int struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref int data)
        {
            if (Length + 4 > _Buffer.Length)
            {
                _capacity += 4;
                Resize();
            }
            var value = new IntStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            Length += 4;
        }

        /// <summary>
        /// write a uint struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref uint data)
        {
            if (Length + 4 > _Buffer.Length)
            {
                _capacity += 4;
                Resize();
            }
            var value = new UIntStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            Length += 4;
        }

        /// <summary>
        /// write a float struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref float data)
        {
            if (Length + 4 > _Buffer.Length)
            {
                _capacity += 4;
                Resize();
            }
            var value = new FloatStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            Length += 4;
        }

        /// <summary>
        /// write a double struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref double data)
        {
            if (Length + 8 > _Buffer.Length)
            {
                _capacity += 8;
                Resize();
            }
            var value = new DoubleStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            Length += 8;
        }

        /// <summary>
        /// write a decimal struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref decimal data)
        {
            if (Length + 16 > _Buffer.Length)
            {
                _capacity += 16;
                Resize();
            }
            var value = new DecimalStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            _Buffer[Length + 8] = value.Byte8;
            _Buffer[Length + 9] = value.Byte9;
            _Buffer[Length + 10] = value.Byte10;
            _Buffer[Length + 11] = value.Byte11;
            _Buffer[Length + 12] = value.Byte12;
            _Buffer[Length + 13] = value.Byte13;
            _Buffer[Length + 14] = value.Byte14;
            _Buffer[Length + 15] = value.Byte15;
            Length += 16;
        }

        /// <summary>
        /// write a guid struct
        /// </summary>
        /// <param name="data"></param>
        public void Write(ref Guid data)
        {
            if (Length + 16 > _Buffer.Length)
            {
                _capacity += 16;
                Resize();
            }
            var value = new GuidStruct() { Value = data };
            _Buffer[Length] = value.Byte0;
            _Buffer[Length + 1] = value.Byte1;
            _Buffer[Length + 2] = value.Byte2;
            _Buffer[Length + 3] = value.Byte3;
            _Buffer[Length + 4] = value.Byte4;
            _Buffer[Length + 5] = value.Byte5;
            _Buffer[Length + 6] = value.Byte6;
            _Buffer[Length + 7] = value.Byte7;
            _Buffer[Length + 8] = value.Byte8;
            _Buffer[Length + 9] = value.Byte9;
            _Buffer[Length + 10] = value.Byte10;
            _Buffer[Length + 11] = value.Byte11;
            _Buffer[Length + 12] = value.Byte12;
            _Buffer[Length + 13] = value.Byte13;
            _Buffer[Length + 14] = value.Byte14;
            _Buffer[Length + 15] = value.Byte15;
            Length += 16;
        }
        #endregion

        /// <summary>
        /// remove last data if was equal
        /// </summary>
        /// <param name="data"></param>
        public void RemoveLast(byte data)
        {
            if (_Buffer[Length - 1].Equals(data))
                Length--;
        }

        /// <summary>
        /// get span of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public Span<byte> ToSpan()
        {
            return _Buffer;
        }

        /// <summary>
        /// get array of buffer
        /// don't forgot this is not enough the real length is when you get Length of property
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return _Buffer;
        }
    }
}
