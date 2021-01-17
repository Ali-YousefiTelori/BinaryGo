using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGoTest.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserInfo> Users { get; set; }
        public bool IsEquals(CompanyInfo company)
        {
            var isEqual = company.Id == Id &&
                company.Name == Name;
            if (!isEqual)
                return isEqual;
            else if (company.Users != Users)
            {
                if (company.Users == null || Users == null || company.Users.Count != Users.Count)
                    return false;
                else
                {
                    for (int i = 0; i < Users.Count; i++)
                    {
                        if (!Users[i].IsEquals(company.Users[i]))
                            return false;
                    }
                }
            }
            return true;

        }
    }
}
