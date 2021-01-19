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
    public class ProductInfo
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
        public virtual DateTime CreatedDate { get; set; }
    }
}
