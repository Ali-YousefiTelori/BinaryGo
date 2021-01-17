using BinaryGoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Json.NullableVariables
{
    public class JsonNormalNullableVariablesDeserializationsTest : JsonNormalNullableVariablesSerializationsTest
    {
        [Fact]
        public void ByteTestDeserialize()
        {
            var (Result, Value) = ByteTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<byte?>(Result) == Value);

            (Result, Value) = ByteNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<byte?>(Result) == Value);
        }

        [Fact]
        public void SByteTestDeserialize()
        {
            var (Result, Value) = SByteTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<sbyte?>(Result) == Value);

            (Result, Value) = SByteNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<sbyte?>(Result) == Value);
        }

        [Fact]
        public void Int16TestDeserialize()
        {
            var (Result, Value) = Int16TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<short?>(Result) == Value);

            (Result, Value) = Int16NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<short?>(Result) == Value);
        }

        [Fact]
        public void UInt16TestDeserialize()
        {
            var (Result, Value) = UInt16TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<ushort?>(Result) == Value);

            (Result, Value) = UInt16NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<ushort?>(Result) == Value);
        }

        [Fact]
        public void Int32TestDeserialize()
        {
            var (Result, Value) = Int32TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<int?>(Result) == Value);

            (Result, Value) = Int32NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<int?>(Result) == Value);
        }

        [Fact]
        public void UInt32TestDeserialize()
        {
            var (Result, Value) = UInt32TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<uint?>(Result) == Value);

            (Result, Value) = UInt32NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<uint?>(Result) == Value);
        }

        [Fact]
        public void Int64TestDeserialize()
        {
            var (Result, Value) = Int64TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<long?>(Result) == Value);

            (Result, Value) = Int64NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<long?>(Result) == Value);
        }

        [Fact]
        public void UInt64TestDeserialize()
        {
            var (Result, Value) = UInt64TestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<ulong?>(Result) == Value);

            (Result, Value) = UInt64NullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<ulong?>(Result) == Value);
        }

        [Fact]
        public void DoubleTestDeserialize()
        {
            var (Result, Value) = DoubleTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<double?>(Result) == Value);

            (Result, Value) = DoubleNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<double?>(Result) == Value);
        }

        [Fact]
        public void FloatTestDeserialize()
        {
            var (Result, Value) = FloatTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<float?>(Result) == Value);

            (Result, Value) = FloatNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<float?>(Result) == Value);
        }

        [Fact]
        public void DecimalTestDeserialize()
        {
            var (Result, Value) = DecimalTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<decimal?>(Result) == Value);

            (Result, Value) = DecimalNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<decimal?>(Result) == Value);
        }

        [Fact]
        public void BoolTestDeserialize()
        {
            var (Result, Value) = BoolTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<bool?>(Result) == Value);

            (Result, Value) = BoolNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<bool?>(Result) == Value);
        }

        [Fact]
        public void DateTimeTestDeserialize()
        {
            var (Result, Value) = DateTimeTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<DateTime?>(Result) == Value);

            (Result, Value) = DateTimeNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<DateTime?>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize1()
        {
            var (Result, Value) = EnumTestSerialize1();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<TestEnum?>(Result) == Value);

            (Result, Value) = EnumNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<TestEnum?>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize2()
        {
            var (Result, Value) = EnumTestSerialize2();

            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<TestEnum?>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize3()
        {
            var (Result, Value) = EnumTestSerialize3();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<TestEnum?>(Result) == Value);
        }

        [Fact]
        public void GuidTestDeserialize()
        {
            var (Result, Value) = GuidTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<Guid?>(Result) == Value);

            (Result, Value) = GuidNullTestSerialize();
            Assert.True(BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<Guid?>(Result) == Value);
        }
    }
}
