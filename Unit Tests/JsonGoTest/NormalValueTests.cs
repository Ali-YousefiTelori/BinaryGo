using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<byte>(result) == value);
        }
        [Test]
        public void UByteTest()
        {
            sbyte value = -5;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<sbyte>(result) == value);
        }
        [Test]
        public void Int16Test()
        {
            short value = -1582;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<short>(result) == value);
        }
        [Test]
        public void UInt16Test()
        {
            ushort value = 1582;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<ushort>(result) == value);
        }
        [Test]
        public void Int32Test()
        {
            int value = -1582;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<int>(result) == value);
        }
        [Test]
        public void UInt32Test()
        {
            uint value = 1582;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
        }
        [Test]
        public void Int64Test()
        {
            long value = -4727327827885;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<long>(result) == value);
        }
        [Test]
        public void UInt64Test()
        {
            ulong value = 4727327827885;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<ulong>(result) == value);
        }
        [Test]
        public void DoubleTest()
        {
            double value = -1582.5453;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<double>(result) == value);
        }
        [Test]
        public void FloatTest()
        {
            float value = 52.66f;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<float>(result) == value);
        }
        [Test]
        public void DecimalTest()
        {
            decimal value = 453445.54245m;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<decimal>(result) == value);
        }
        [Test]
        public void StringTest()
        {
            string value = "ali yousefi";
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<string>(result) == value);
        }

        [Test]
        public void BoolTest()
        {
            bool value = true;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{value.ToString().ToLower()}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<bool>(result) == value);
        }
        [Test]
        public void DateTimeTest()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"{value}\"");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<DateTime>(result) == value);
        }
        [Test]
        public void EnumTest()
        {
            TestEnum value = TestEnum.None;
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{(int)value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
            value = TestEnum.Value10;
            result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{(int)value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
            value = TestEnum.Value50;
            result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"{(int)value}");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<TestEnum>(result) == value);
        }

        [Test]
        public void ByteArrayTest()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == $"\"BQpfIA==\"");
            Assert.IsTrue(JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<byte[]>(result).SequenceEqual(value));

        }

        [Test]
        public void IntArrayTest()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == "[5,10,95,32]");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<int[]>(result);
            Assert.IsTrue(data.SequenceEqual(value));
        }
        [Test]
        public void IntArrayValueReferenceTest()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            JsonGo.Json.Serializer serializer = new JsonGo.Json.Serializer(new JsonGo.Json.JsonOptionInfo() { IsGenerateLoopReference = true });
            var result = serializer.Serialize(value);
            Assert.IsTrue(result == "{\"$id\":1,\"$values\":[5,10,95,32]}");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<int[]>(result);
            Assert.IsTrue(data.SequenceEqual(value));
        }

        [Test]
        public void StringArrayTest()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == "[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<string[]>(result);
            Assert.IsTrue(data.SequenceEqual(value));
        }

        [Test]
        public void StringArrayReferenceTest()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            JsonGo.Json.Serializer serializer = new JsonGo.Json.Serializer(new JsonGo.Json.JsonOptionInfo() { IsGenerateLoopReference = true });
            var result = serializer.Serialize(value);
            Assert.IsTrue(result == "{\"$id\":1,\"$values\":[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]}");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<string[]>(result);
            Assert.IsTrue(data.SequenceEqual(value));
        }

        [Test]
        public void StringQuatsTest()
        {
            string value = "salam\"\"ddv sdd {} [] \"";
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(value);
            Assert.IsTrue(result == "\"salam\\\"\\\"ddv sdd {} [] \\\"\"");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<string>(result);
            Assert.IsTrue(value == data);
        }

        [Test]
        public void StringWithLineTest()
        {
            string line = @"test hello: ""my name is
ali
then yousefi"" so we are good now""";
            var result = JsonGo.Json.Serializer.SingleIntance.Serialize(line);
            Assert.IsTrue(result == "\"test hello: \\\"my name is\\r\\nali\\r\\nthen yousefi\\\" so we are good now\\\"\"");
            var data = JsonGo.Deserialize.JsonDeserializer.SingleIntance.Deserialize<string>(result);
            Assert.IsTrue(data == line);
        }
        #endregion
    }
}
