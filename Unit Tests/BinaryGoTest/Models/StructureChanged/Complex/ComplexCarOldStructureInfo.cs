﻿using BinaryGoTest.Models.Complex;
using System;
using System.Linq;
using Xunit;

namespace BinaryGoTest.Models.StructureChanged.Complex
{
    public class ComplexCarOldStructureInfo
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public DateTime CreationDateTime { get; set; }
        public byte[] Data { get; set; }
        public int Height { get; set; }
        public bool IsEquals(ComplexCarInfo complexCarInfo)
        {
            Assert.True(complexCarInfo.Name == Name
                && complexCarInfo.Weight == Weight
                && complexCarInfo.CreationDateTime == CreationDateTime);
            Assert.True(complexCarInfo.Data.SequenceEqual(Data));
            return true;
        }
    }
}
