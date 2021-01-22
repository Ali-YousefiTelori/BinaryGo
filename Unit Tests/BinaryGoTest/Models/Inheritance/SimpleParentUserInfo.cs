using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryGoTest.Models.Inheritance
{
    public class SimpleParentUserInfo : SimpleBaseUserInfo
    {
        public int Id { get; set; }
        public string Family { get; set; }
        public string Mail { get; set; }
        public bool IsEquals(SimpleParentUserInfo user)
        {
            var isEqual = user.Id == Id
                && user.Name == Name
                && user.Family == Family
                && user.Weight == Weight
                && user.Phone == Phone
                && user.Mail == Mail;
            return isEqual;
        }
    }
}
