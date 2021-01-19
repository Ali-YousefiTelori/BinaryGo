using MessagePack;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace BinaryGoPerformance.Models
{
    [ZeroFormattable]
    [MessagePackObject]
    [ProtoContract]
    public class UserInfo
    {
        [Key(0)]
        [Index(0)]
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        [ProtoMember(2)]
        public virtual string FullName { get; set; }
        [Key(2)]
        [Index(2)]
        [ProtoMember(3)]
        public virtual int Age { get; set; }
        [Key(3)]
        [Index(3)]
        [ProtoMember(4)]
        public virtual DateTime CreatedDate { get; set; }
        [Key(4)]
        [Index(4)]
        [ProtoMember(5)]
        public virtual List<ProductInfo> Products { get; set; }
        [Key(5)]
        [Index(5)]
        [ProtoMember(6)]
        public virtual List<RoleInfo> Roles { get; set; }
    }

    [ZeroFormattable]
    [MessagePackObject]
    [ProtoContract]
    public class SimpleUserInfo
    {
        [Key(0)]
        [Index(0)]
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        [ProtoMember(2)]
        public virtual string FullName { get; set; }
        [Key(2)]
        [Index(2)]
        [ProtoMember(3)]
        public virtual int Age { get; set; }
        [Key(3)]
        [Index(3)]
        [ProtoMember(4)]
        public virtual DateTime CreatedDate { get; set; }
    }
}
