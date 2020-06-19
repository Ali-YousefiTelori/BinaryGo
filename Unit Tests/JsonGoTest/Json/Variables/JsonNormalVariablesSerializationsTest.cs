using JsonGo.Runtime;
using JsonGoTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JsonGoTest.Json.Variables
{
    public class JsonNormalVariablesSerializationsTest
    {
        [Fact]
        public (string Result, byte Value) ByteTestSerialize()
        {
            byte value = 45;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, sbyte Value) SByteTestSerialize()
        {
            sbyte value = -5;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, short Value) Int16TestSerialize()
        {
            short value = -1582;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, ushort Value) UInt16TestSerialize()
        {
            ushort value = 1582;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, int Value) Int32TestSerialize()
        {
            int value = -1582;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, uint Value) UInt32TestSerialize()
        {
            uint value = 1582;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, long Value) Int64TestSerialize()
        {
            long value = -4727327827885;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, ulong Value) UInt64TestSerialize()
        {
            ulong value = 4727327827885;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, double Value) DoubleTestSerialize()
        {
            double value = -1582.5453;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, float Value) FloatTestSerialize()
        {
            float value = 52.66f;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, decimal Value) DecimalTestSerialize()
        {
            decimal value = 453445.54245m;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}"); return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringTestSerialize()
        {
            string value = "ali yousefi";
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"{value}\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, bool Value) BoolTestSerialize()
        {
            bool value = true;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value.ToString().ToLower()}", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, bool Value) BoolTestSerialize2()
        {
            bool value = false;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value.ToString().ToLower()}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, DateTime Value) DateTimeTestSerialize()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"{value}\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize1()
        {
            BaseTypeGoInfo.Generate<TestEnum>(JsonGo.Json.Serializer.DefaultOptions);
            TestEnum value = TestEnum.None;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize2()
        {
            BaseTypeGoInfo.Generate<TestEnum>(JsonGo.Json.Serializer.DefaultOptions);
            var value = TestEnum.Value10;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize3()
        {
            var value = TestEnum.Value50;
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, byte[] Value) ByteArrayTestSerialize()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"BQpfIA==\"", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, int[] Value) IntArrayTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "[5,10,95,32]", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, int[] Value) IntArrayValueReferenceTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            JsonGo.Json.Serializer serializer = new JsonGo.Json.Serializer(new JsonGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
            var result = serializer.Serialize(value);
            Assert.True(result == "{\"$id\":1,\"$values\":[5,10,95,32]}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string[] Value) StringArrayTestSerialize()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string[] Value) StringArrayReferenceTestSerialize()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            JsonGo.Json.Serializer serializer = new JsonGo.Json.Serializer(new JsonGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
            var result = serializer.Serialize(value);
            Assert.True(result == "{\"$id\":1,\"$values\":[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringQuatsTestSerialize()
        {
            string value = "salam\"\"ddv sdd {} [] \"";
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "\"salam\\\"\\\"ddv sdd {} [] \\\"\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringWithLineTestSerialize()
        {
            string value = @"test hello: ""my name is
ali
then yousefi"" so we are good now""";
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "\"test hello: \\\"my name is\\r\\nali\\r\\nthen yousefi\\\" so we are good now\\\"\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
    }
}