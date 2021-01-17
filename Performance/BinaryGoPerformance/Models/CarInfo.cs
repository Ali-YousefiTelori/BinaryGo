using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace BinaryGoPerformance.Models
{
    [ZeroFormattable]
    [MessagePackObject]
    public class CarInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual string Name { get; set; }
    }
}
