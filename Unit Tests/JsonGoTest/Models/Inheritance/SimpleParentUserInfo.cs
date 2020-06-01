using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest.Models.Inheritance
{
    public class SimpleParentUserInfo : SimpleBaseUserInfo
    {
        public int Id { get; set; }
        public string Family { get; set; }
        public bool IsEquals(SimpleParentUserInfo user)
        {
            var isEqual = user.Id == Id
                && user.Name == Name
                && user.Family == Family;
            return isEqual;
        }
    }
}
