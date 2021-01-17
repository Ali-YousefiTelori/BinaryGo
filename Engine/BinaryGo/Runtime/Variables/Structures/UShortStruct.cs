using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryGo.Runtime.Variables.Structures
{
    /// <summary>
    /// struct of ushort to direct access of bytes in memory to make bitconverter faster
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public ref struct UShortStruct
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
        /// value in memory
        /// </summary>
        [FieldOffset(0)]
        public ushort Value;
    }

}
