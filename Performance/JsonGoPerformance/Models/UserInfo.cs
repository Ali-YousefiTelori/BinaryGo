using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<ProductInfo> Products { get; set; }
        public List<RoleInfo> Roles { get; set; }

        public CompanyInfo CompanyInfo { get; set; }
    }

    [MessagePackObject]
    public class SimpleUserInfo
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string FullName { get; set; }
        [Key(2)]
        public int Age { get; set; }
        [Key(3)]
        public DateTime CreatedDate { get; set; }
    }
}
