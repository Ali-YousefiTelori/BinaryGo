using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.StructureChanged.Inheritance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryGoTest.Models.StructureChanged
{
    public class SimpleParentUserOldStructureInfo : SimpleBaseParentUserOldStructureInfo
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