using BinaryGoTest.Models.Complex;
using System;
using System.Collections.Generic;
using Xunit;

namespace BinaryGoTest.Models.StructureChanged.Complex
{
    public class ComplexCompanyOldStructureInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public bool IsClosed { get; set; }
        public List<ComplexCarOldStructureInfo> Cars { get; set; }
        public List<ComplexTypeOldStructureInfo> Types { get; set; }
        public bool IsEquals(ComplexCompanyInfo complexCompany)
        {
            Assert.True(complexCompany.Id == Id
                && complexCompany.Name == Name
                && complexCompany.Key == Key
                && complexCompany.IsClosed == IsClosed);
            Assert.True(Cars.Count == complexCompany.Cars.Count);
            for (int i = 0; i < Cars.Count; i++)
            {
                Assert.True(complexCompany.Cars[i].IsEquals(Cars[i]));
            }
            return true;
        }
    }
}
