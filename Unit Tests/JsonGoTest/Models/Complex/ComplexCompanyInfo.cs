using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Models.Normal
{
    public enum ComapnyType : int
    {
        None = 0,
        Creator = 1,
        Visitor = 2,
        Goverment = 3
    }

    public class ComplexCompanyInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public ComapnyType Type { get; set; }
        public bool IsClosed { get; set; }
        public List<ComplexCarInfo> Cars { get; set; }
        public bool IsEquals(ComplexCompanyInfo  complexCompany)
        {
            Assert.True(complexCompany.Id == Id && complexCompany.Name == Name && complexCompany.Key == Key);
            Assert.True(complexCompany.Type == Type && complexCompany.IsClosed == IsClosed);
            Assert.True(Cars.Count == complexCompany.Cars.Count);
            for (int i = 0; i < Cars.Count; i++)
            {
                Assert.True(complexCompany.Cars[i].IsEquals(Cars[i]));
            }
            return true;
        }
    }
}
