using BinaryGo.Runtime;
using System;

namespace BinaryGo.Binary.StructureModels
{
    /// <summary>
    /// structure of property of models of binaryGo structure models
    /// do not change the property position please that will be a big brak changes for updates
    /// </summary>
    public class MemberBinaryModelInfo
    {
        /// <summary>
        /// index of member
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// name of property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// has read access of property
        /// </summary>
        public bool CanRead { get; set; }
        /// <summary>
        /// has write access of property
        /// </summary>
        public bool CanWrite { get; set; }
        /// <summary>
        /// type of member
        /// </summary>
        public MemberBinaryModelType Type { get; set; }
        /// <summary>
        /// Resturn type of member
        /// </summary>
        public BinaryModelInfo ResultType { get; set; }
    }
}
