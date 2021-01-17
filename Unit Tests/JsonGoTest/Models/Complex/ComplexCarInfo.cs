using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Models.Normal
{
    public class ComplexCarInfo
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public double Weight { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsEquals(ComplexCarInfo  complexCarInfo)
        {
            Assert.True(complexCarInfo.Name == Name && complexCarInfo.Weight == Weight && complexCarInfo.CreationDateTime == CreationDateTime);
            Assert.True(complexCarInfo.Data.SequenceEqual(Data));
            return true;
        }
    }
}
