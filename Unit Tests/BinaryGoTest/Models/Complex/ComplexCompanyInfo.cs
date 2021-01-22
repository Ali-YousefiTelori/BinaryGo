using BinaryGoTest.Models.StructureChanged.Complex;
using System;
using System.Collections.Generic;
using Xunit;

namespace BinaryGoTest.Models.Complex
{
    public enum CompanyType : int
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
        public CompanyType Type { get; set; }
        public bool IsClosed { get; set; }
        public List<ComplexCarInfo> Cars { get; set; }
        public bool IsEquals(ComplexCompanyInfo complexCompany)
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
        public bool IsEquals(ComplexCompanyOldStructureInfo complexCompany)
        {
            Assert.True(complexCompany.Id == Id && complexCompany.Name == Name && complexCompany.Key == Key);
            Assert.True(complexCompany.IsClosed == IsClosed);
            Assert.True(Cars.Count == complexCompany.Cars.Count);
            for (int i = 0; i < Cars.Count; i++)
            {
                Assert.True(complexCompany.Cars[i].IsEquals(Cars[i]));
            }
            return true;
        }
    }
}
