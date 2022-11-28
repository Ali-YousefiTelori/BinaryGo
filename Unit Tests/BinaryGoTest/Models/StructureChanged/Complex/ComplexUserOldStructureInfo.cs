using BinaryGoTest.Models.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Models.StructureChanged.Complex
{
    public class ComplexUserOldStructureInfo
    {
        public ulong Id { get; set; }
        public ComplexCompanyOldStructureInfo[] Companies { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }

        public bool IsEquals(ComplexUser complexUser)
        {
            Assert.True(complexUser.Id == Id && complexUser.UserName == UserName);
            Assert.True(Companies.Length == complexUser.Companies.Length);
            for (int i = 0; i < complexUser.Companies.Length; i++)
            {
                Assert.True(complexUser.Companies[i].IsEquals(Companies[i]));
            }
            return true;
        }

        public bool IsEquals(ComplexUserOldStructureInfo complexUser)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this) == Newtonsoft.Json.JsonConvert.SerializeObject(complexUser) ;
        }
    }
}
