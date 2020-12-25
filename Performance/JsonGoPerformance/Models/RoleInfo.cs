using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace JsonGoPerformance.Models
{
    public enum RoleType : byte
    {
        None = 0,
        Admin = 1,
        Normal =2,
        Viewer = 3
    }
    [ZeroFormattable]
    [MessagePackObject]
    public class RoleInfo
    {
        [Key(0)]
        [Index(0)]
        public virtual int Id { get; set; }
        [Key(1)]
        [Index(1)]
        public virtual RoleType Type { get; set; }
    }
}
