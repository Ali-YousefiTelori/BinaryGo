using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest
{
    public enum TestEnum
    {
        None = 0,
        Value10 = 10,
        Value50 = 50
    }
    public class NormalValueTests
    {
        [SetUp]
        public void Setup()
        {

        }

        #region Serialize and Deserialize
        [Test]
        public void ByteTest()
        {
            byte value = 45;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<byte>(result) == value);
        }
        [Test]
        public void UByteTest()
        {
            sbyte value = -5;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<sbyte>(result) == value);
        }
        [Test]
        public void Int16Test()
        {
            short value = -1582;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<short>(result) == value);
        }
        [Test]
        public void UInt16Test()
        {
            ushort value = 1582;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<ushort>(result) == value);
        }
        [Test]
        public void Int32Test()
        {
            int value = -1582;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<int>(result) == value);
        }
        [Test]
        public void UInt32Test()
        {
            uint value = 1582;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
        }
        [Test]
        public void Int64Test()
        {
            long value = -4727327827885;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<long>(result) == value);
        }
        [Test]
        public void UInt64Test()
        {
            ulong value = 4727327827885;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<ulong>(result) == value);
        }
        [Test]
        public void DoubleTest()
        {
            double value = -1582.5453;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<double>(result) == value);
        }
        [Test]
        public void FloatTest()
        {
            float value = 52.66f;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<float>(result) == value);
        }
        [Test]
        public void DecimalTest()
        {
            decimal value = 453445.54245m;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<decimal>(result) == value);
        }
        [Test]
        public void StringTest()
        {
            string value = "ali yousefi";
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<string>(result) == value);
        }

        [Test]
        public void BoolTest()
        {
            bool value = true;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<bool>(result) == value);
        }
        [Test]
        public void DateTimeTest()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<DateTime>(result) == value);
        }
        [Test]
        public void EnumTest()
        {
            TestEnum value = TestEnum.None;
            var result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{(int)value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
            value = TestEnum.Value10;
            result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{(int)value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
            value = TestEnum.Value50;
            result = JsonGo.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{(int)value}\"");
            Assert.IsTrue(JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
        }
        #endregion
    }
}
