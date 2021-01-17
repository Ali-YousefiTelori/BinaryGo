using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace BinaryGoPerformance.Models
{
    [ZeroFormattable]
    [MessagePackObject]
    public class ProductInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual string Name { get; set; }
        [Key(2)]
        [Index(2)]
        public virtual DateTime CreatedDate { get; set; }
    }
}
