using MessagePack;
using ProtoBuf;
using System;
using ZeroFormatter;

namespace BinaryGoPerformance.Models
{
    public enum RoleType : byte
    {
        None = 0,
        Admin = 1,
        Normal = 2,
        Viewer = 3
    }
    [ZeroFormattable]
    [MessagePackObject]
    [ProtoContract]
    public class RoleInfo
    {
        [Key(0)]
        [Index(0)]
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        [ProtoMember(2)]
        public virtual RoleType Type { get; set; }
    }
}
