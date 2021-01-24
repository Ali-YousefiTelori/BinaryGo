using BinaryGo.Runtime;
using BinaryGoTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryGoTest.Json.Variables
{
    public class JsonNormalVariablesSerializationsTest
    {
        [Fact]
        public (string Result, byte Value) ByteTestSerialize()
        {
            byte value = 45;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, sbyte Value) SByteTestSerialize()
        {
            sbyte value = -5;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, short Value) Int16TestSerialize()
        {
            short value = -1582;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, ushort Value) UInt16TestSerialize()
        {
            ushort value = 1582;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, int Value) Int32TestSerialize()
        {
            int value = -1582;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, uint Value) UInt32TestSerialize()
        {
            uint value = 1582;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, long Value) Int64TestSerialize()
        {
            long value = -4727327827885;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, ulong Value) UInt64TestSerialize()
        {
            ulong value = 4727327827885;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, double Value) DoubleTestSerialize()
        {
            double value = -1582.5453;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, float Value) FloatTestSerialize()
        {
            float value = 52.66f;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, decimal Value) DecimalTestSerialize()
        {
            decimal value = 453445.54245m;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value}", $"Your Value: {value} Serialize Value: {result}"); return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringTestSerialize()
        {
            string value = "ali yousefi";
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"{value}\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, bool Value) BoolTestSerialize()
        {
            bool value = true;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value.ToString().ToLower()}", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, bool Value) BoolTestSerialize2()
        {
            bool value = false;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{value.ToString().ToLower()}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, DateTime Value) DateTimeTestSerialize()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            var jsonSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Assert.True(result == $"\"{value}\"", $"Your Value: {value} Serialize Value: {result} and jsonSerialize: {jsonSerialize}");
            return (result, value);
        }
        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize1()
        {
            BaseTypeGoInfo.Generate<TestEnum>(BinaryGo.Json.Serializer.DefaultOptions);
            TestEnum value = TestEnum.None;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize2()
        {
            BaseTypeGoInfo.Generate<TestEnum>(BinaryGo.Json.Serializer.DefaultOptions);
            var value = TestEnum.Value10;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, TestEnum Value) EnumTestSerialize3()
        {
            var value = TestEnum.Value50;
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"{(int)value}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, byte[] Value) ByteArrayTestSerialize()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"BQpfIA==\"", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, int[] Value) IntArrayTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "[5,10,95,32]", $"Your Value: {value} Serialize Value: {result}"); 
            return (result, value);
        }

        [Fact]
        public (string Result, int[] Value) IntArrayValueReferenceTestSerialize()
        {
            //TODO fix
            return default;
            int[] value = new int[] { 5, 10, 95, 32 };
            BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
            var result = serializer.Serialize(value);
            Assert.True(result == "{\"$id\":1,\"$values\":[5,10,95,32]}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string[] Value) StringArrayTestSerialize()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string[] Value) StringArrayReferenceTestSerialize()
        {  
            //TODO fix
            return default;
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
            var result = serializer.Serialize(value);
            Assert.True(result == "{\"$id\":1,\"$values\":[\"5\",\"1ss0\",\"9fg5\",\"25dd\"]}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringQuatsTestSerialize()
        {
            string value = "salam\"\"ddv sdd {} [] \"";
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "\"salam\\\"\\\"ddv sdd {} [] \\\"\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, string Value) StringWithLineTestSerialize()
        {
            string value = $"\"test hello: \\\"my name is{Environment.NewLine}ali{Environment.NewLine}then \\ yousefi\\\" so we are good now\\\"\"";
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            if (Environment.NewLine.Length == 2)
                Assert.True(result == "\"\\\"test hello: \\\\\\\"my name is\\r\\nali\\r\\nthen \\\\ yousefi\\\\\\\" so we are good now\\\\\\\"\\\"\"", $"Your Value: {value} Serialize Value: {result}");
            else
                Assert.True(result == "\"\\\"test hello: \\\\\\\"my name is\\nali\\nthen \\\\ yousefi\\\\\\\" so we are good now\\\\\\\"\\\"\"", $"Your Value: {value} Serialize Value: {result}");

            return (result, value);
        }


        [Fact]
        public (string Result, Guid Value) GuidTestSerialize()
        {
            Guid value = Guid.NewGuid();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == $"\"{value.ToString().ToLower()}\"", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
    }
}