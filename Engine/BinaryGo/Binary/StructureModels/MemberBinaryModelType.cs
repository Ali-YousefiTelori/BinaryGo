namespace BinaryGo.Binary.StructureModels
{
    /// <summary>
    /// Type of member of binary mmeber
    /// </summary>
    public enum MemberBinaryModelType : byte
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,
        /// <summary>
        /// Property type
        /// </summary>
        Property = 1,
        /// <summary>
        /// Field type
        /// </summary>
        Field = 2
    }
}
