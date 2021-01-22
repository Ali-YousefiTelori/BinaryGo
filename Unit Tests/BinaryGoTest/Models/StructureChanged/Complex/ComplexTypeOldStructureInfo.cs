using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Models.StructureChanged.Complex
{
    public class ComplexTypeOldStructureInfo
    {
        public string TypeName { get; set; }
        public bool IsEquals(ComplexTypeOldStructureInfo  complexTypeOldStructureInfo)
        {
            Assert.True(complexTypeOldStructureInfo.TypeName == TypeName);
            return true;
        }
    }
}
