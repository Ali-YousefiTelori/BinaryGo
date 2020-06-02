using JsonGoTest.Models;
using System;
using System.Linq;
using Xunit;

namespace JsonGoTest.Json.Variables
{
    public class JsonNormalVariablesDeserializationsTest : JsonNormalVariablesSerializationsTest
    {
        [Fact]
        public void ByteTestDeserialize()
        {
            var (Result, Value) = ByteTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<byte>(Result) == Value);
        }

        [Fact]
        public void SByteTestDeserialize()
        {
            var (Result, Value) = SByteTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<sbyte>(Result) == Value);
        }

        [Fact]
        public void Int16TestDeserialize()
        {
            var (Result, Value) = UInt16TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<short>(Result) == Value);
        }

        [Fact]
        public void UInt16TestDeserialize()
        {
            var (Result, Value) = UInt16TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<ushort>(Result) == Value);
        }

        [Fact]
        public void Int32TestDeserialize()
        {
            var (Result, Value) = Int32TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<int>(Result) == Value);
        }

        [Fact]
        public void UInt32TestDeserialize()
        {
            var (Result, Value) = UInt32TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<uint>(Result) == Value);
        }

        [Fact]
        public void Int64TestDeserialize()
        {
            var (Result, Value) = Int64TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<long>(Result) == Value);
        }

        [Fact]
        public void UInt64TestDeserialize()
        {
            var (Result, Value) = UInt64TestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<ulong>(Result) == Value);
        }

        [Fact]
        public void DoubleTestDeserialize()
        {
            var (Result, Value) = DoubleTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<double>(Result) == Value);
        }

        [Fact]
        public void FloatTestDeserialize()
        {
            var (Result, Value) = FloatTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<float>(Result) == Value);
        }

        [Fact]
        public void DecimalTestDeserialize()
        {
            var (Result, Value) = DecimalTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<decimal>(Result) == Value);
        }

        [Fact]
        public void StringTestDeserialize()
        {
            var (Result, Value) = StringTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<string>(Result) == Value);
        }

        [Fact]
        public void BoolTestDeserialize()
        {
            var (Result, Value) = BoolTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<bool>(Result) == Value);
        }

        [Fact]
        public void BoolTestDeserialize2()
        {
            var (Result, Value) = BoolTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<bool>(Result) == Value);
        }

        [Fact]
        public void DateTimeTestDeserialize()
        {
            var (Result, Value) = DateTimeTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<DateTime>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize1()
        {
            var (Result, Value) = EnumTestSerialize1();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize2()
        {
            var (Result, Value) = EnumTestSerialize2();

            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize3()
        {
            var (Result, Value) = EnumTestSerialize3();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void ByteArrayTestDeserialize()
        {
            var (Result, Value) = ByteArrayTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<byte[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void IntArrayTestDeserialize()
        {
            var (Result, Value) = IntArrayTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<int[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void IntArrayValueReferenceTestDeserialize()
        {
            var (Result, Value) = IntArrayValueReferenceTestSerialize();
            JsonGo.Json.Deserialize.JsonDeserializer deserializer = new JsonGo.Json.Deserialize.JsonDeserializer()
            {
                HasGenerateRefrencedTypes = true
            };
            Assert.True(deserializer.Deserialize<int[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void StringArrayTestDeserialize()
        {
            var (Result, Value) = StringArrayTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<string[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void StringArrayReferenceTestDeserialize()
        {
            var (Result, Value) = StringArrayReferenceTestSerialize();
            JsonGo.Json.Deserialize.JsonDeserializer deserializer = new JsonGo.Json.Deserialize.JsonDeserializer()
            {
                HasGenerateRefrencedTypes = true
            };
            Assert.True(deserializer.Deserialize<string[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void StringQuatsTestDeserialize()
        {
            var (Result, Value) = StringQuatsTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<string>(Result) == Value);
        }

        [Fact]
        public void StringWithLineTestDeserialize()
        {
            var (Result, Value) = StringWithLineTestSerialize();
            Assert.True(JsonGo.Json.Deserialize.JsonDeserializer.SingleInstance.Deserialize<string>(Result) == Value);
        }
    }
}
