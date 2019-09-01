using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest.Models
{
    public class UserInfo
    {
        public string this[int index]
        {
            get
            {
                return null;
            }
            set
            {

            }
        }
        public short EMP_NO { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool? IsMarried { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<RoleInfo> Roles { get; set; }
        public CompanyInfo CompanyInfo { get; set; }

        public bool IsEquals(UserInfo user)
        {
            var isEqual = user.Age == Age &&
                user.CreatedDate == CreatedDate &&
                user.FullName == FullName &&
                user.EMP_NO == EMP_NO &&
                user.Id == Id && user.IsMarried == IsMarried;
            if (!isEqual)
                return isEqual;
            else if (user.Roles != Roles)
            {
                if (user.Roles == null || Roles == null || user.Roles.Count != Roles.Count)
                    return false;
                else
                {
                    for (int i = 0; i < Roles.Count; i++)
                    {
                        if (!Roles[i].IsEquals(user.Roles[i]))
                            return false;
                    }
                }
            }
            return true;

        }
    }
}
