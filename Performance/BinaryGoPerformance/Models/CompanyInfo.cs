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
    public class CompanyInfo
    {
        [Key(0)]
        [Index(0)]
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        [ProtoMember(2)]
        public virtual string Name { get; set; }
        [Key(2)]
        [Index(2)]
        [ProtoMember(3)]
        public virtual List<UserInfo> Users { get; set; }
        [Key(3)]
        [Index(3)]
        [ProtoMember(4)]
        public virtual List<CarInfo> Cars { get; set; }
    }
}
