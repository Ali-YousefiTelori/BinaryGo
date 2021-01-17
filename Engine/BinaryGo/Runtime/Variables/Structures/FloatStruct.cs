using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryGo.Runtime.Variables.Structures
{
    /// <summary>
    /// struct of float to direct access of bytes in memory to make bitconverter faster
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public ref struct FloatStruct
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
        /// value in memory
        /// </summary>
        [FieldOffset(0)]
        public float Value;
    }
}
