using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JsonGo.Runtime.Variables.Structures
{
    /// <summary>
    /// struct of double to direct access of bytes in memory to make bitconverter faster
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public ref struct DoubleStruct
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
        /// value in memory
        /// </summary>
        [FieldOffset(0)]
        public double Value;
    }
}
