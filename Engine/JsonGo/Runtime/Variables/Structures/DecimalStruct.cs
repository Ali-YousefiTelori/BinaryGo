using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JsonGo.Runtime.Variables.Structures
{
    /// <summary>
    /// struct of decimal to direct access of bytes in memory to make bitconverter faster
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public ref struct DecimalStruct
    {
        /// <summary>
        /// offset of zero in memory
        /// </summary>
        [FieldOffset(0)]
        public byte Byte0;
        /// <summary>
        /// offset of one in memory
        /// </summary>
        [FieldOffset(1)]
        public byte Byte1;
        /// <summary>
        /// offset of two in memory
        /// </summary>
        [FieldOffset(2)]
        public byte Byte2;
        /// <summary>
        /// offset of three in memory
        /// </summary>
        [FieldOffset(3)]
        public byte Byte3;
        /// <summary>
        /// offset of four in memory
        /// </summary>
        [FieldOffset(4)]
        public byte Byte4;
        /// <summary>
        /// offset of five in memory
        /// </summary>
        [FieldOffset(5)]
        public byte Byte5;
        /// <summary>
        /// offset of six in memory
        /// </summary>
        [FieldOffset(6)]
        public byte Byte6;
        /// <summary>
        /// offset of seven in memory
        /// </summary>
        [FieldOffset(7)]
        public byte Byte7;

        /// <summary>
        /// offset of eight in memory
        /// </summary>
        [FieldOffset(8)]
        public byte Byte8;
        /// <summary>
        /// offset of nine in memory
        /// </summary>
        [FieldOffset(9)]
        public byte Byte9;
        /// <summary>
        /// offset of then in memory
        /// </summary>
        [FieldOffset(10)]
        public byte Byte10;
        /// <summary>
        /// offset of eleven in memory
        /// </summary>
        [FieldOffset(11)]
        public byte Byte11;
        /// <summary>
        /// offset of twelve in memory
        /// </summary>
        [FieldOffset(12)]
        public byte Byte12;
        /// <summary>
        /// offset of thirteen in memory
        /// </summary>
        [FieldOffset(13)]
        public byte Byte13;
        /// <summary>
        /// offset of fourteen in memory
        /// </summary>
        [FieldOffset(14)]
        public byte Byte14;
        /// <summary>
        /// offset of fiveteen in memory
        /// </summary>
        [FieldOffset(15)]
        public byte Byte15;

        /// <summary>
        /// value in memory
        /// </summary>
        [FieldOffset(0)]
        public decimal Value;
    }
}
