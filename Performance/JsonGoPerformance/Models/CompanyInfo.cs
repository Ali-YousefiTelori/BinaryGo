using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace JsonGoPerformance.Models
{
    [ZeroFormattable]
    [MessagePackObject]
    public class CompanyInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual string Name { get; set; }
        [Key(2)]
        [Index(2)]
        public virtual List<UserInfo> Users { get; set; }
        [Key(3)]
        [Index(3)]
        public virtual List<CarInfo> Cars { get; set; }
    }
}
