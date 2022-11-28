using BinaryGoTest.Models.StructureChanged.Complex;
using System;
using System.Linq;
using Xunit;

namespace BinaryGoTest.Models.Complex
{
    public class ComplexCarInfo
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public double Weight { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Age { get; set; } = "A600";
        public string Type { get; set; }
        public int AgeInt { get; set; } = 600;
        public int ObjectType { get; set; } = 700;
        public bool IsEquals(ComplexCarInfo complexCarInfo)
        {
            Assert.True(complexCarInfo.Name == Name && complexCarInfo.Weight == Weight && complexCarInfo.CreationDateTime == CreationDateTime);
            Assert.True(complexCarInfo.Data.SequenceEqual(Data));
            return true;
        }
        public bool IsEquals(ComplexCarOldStructureInfo complexCarInfo)
        {
            Assert.True(complexCarInfo.Name == Name && complexCarInfo.Weight == Weight && complexCarInfo.CreationDateTime == CreationDateTime);
            Assert.True(complexCarInfo.Data.SequenceEqual(Data));
            return true;
        }
    }
}
