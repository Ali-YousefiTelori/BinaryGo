using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Models.Complex
{
    public class ComplexUser
    {
        public ulong Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ComplexCompanyInfo[] Companies { get; set; }

        public bool IsEquals(ComplexUser complexUser)
        {
            Assert.True(complexUser.Id == Id && complexUser.UserName == UserName && complexUser.Password == Password);
            Assert.True(Companies.Length == complexUser.Companies.Length);
            for (int i = 0; i < complexUser.Companies.Length; i++)
            {
                Assert.True(complexUser.Companies[i].IsEquals(Companies[i]));
            }
            return true;
        }
    }
}
