using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryGoTest.Models.StructureChanged
{
    public class SimpleUserOldStructureInfo
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string Name { get; set; }
        public bool IsEquals(SimpleUserOldStructureInfo user)
        {
            var isEqual = user.Id == Id
                && user.Name == Name
                && user.Age == Age
                && user.BirthDate == BirthDate;
            return isEqual;
        }

        public bool IsEquals(SimpleUserInfo user)
        {
            var isEqual = user.Id == Id
                && user.Name == Name;
            return isEqual;
        }
    }
}
