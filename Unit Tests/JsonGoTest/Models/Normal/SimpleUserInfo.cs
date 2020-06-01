using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest.Models.Normal
{
    public class SimpleUserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public bool IsEquals(SimpleUserInfo user)
        {
            var isEqual = user.Id == Id
                && user.Name == Name
                && user.Family == Family;
            return isEqual;
        }
    }
}
