using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<RoleInfo> Roles { get; set; }
        public CompanyInfo CompanyInfo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UserInfo user)
            {
                return user.Age == Age &&
                    user.CreatedDate == CreatedDate &&
                    user.FullName == FullName &&
                    user.Id == Id;
            }
            return false;
        }
    }
}
