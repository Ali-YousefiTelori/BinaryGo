using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public enum RoleType : byte
    {
        None = 0,
        Admin = 1,
        Normal =2,
        Viewer = 3
    }
    public class RoleInfo
    {
        public int Id { get; set; }
        public UserInfo UserInfo { get; set; }
        public RoleType Type { get; set; }
    }
}
