using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace BinaryGoPerformance.Models
{
    [ZeroFormattable]
    [MessagePackObject]
    public class UserInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual string FullName { get; set; }
        [Key(2)]
        [Index(2)]
        public virtual int Age { get; set; }
        [Key(3)]
        [Index(3)]
        public virtual DateTime CreatedDate { get; set; }
        [Key(4)]
        [Index(4)]
        public virtual List<ProductInfo> Products { get; set; }
        [Key(5)]
        [Index(5)]
        public virtual List<RoleInfo> Roles { get; set; }
    }

    [ZeroFormattable]
    [MessagePackObject]
    public class SimpleUserInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual string FullName { get; set; }
        [Key(2)]
        [Index(2)]
        public virtual int Age { get; set; }
        [Key(3)]
        [Index(3)]
        public virtual DateTime CreatedDate { get; set; }
    }
}
