using BinaryGoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BinaryGoTest.Binary.Variables
{
    public class BinaryNormalVariablesDeserializationsTest : BinaryNormalVariablesSerializationsTest
    {
        [Fact]
        public void ByteTestDeserialize()
        {
            var (Result, Value) = ByteTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<byte>(Result) == Value);
        }

        [Fact]
        public void SByteTestDeserialize()
        {
            var (Result, Value) = SByteTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<sbyte>(Result) == Value);
        }

        [Fact]
        public void Int16TestDeserialize()
        {
            var (Result, Value) = Int16TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<short>(Result) == Value);
        }

        [Fact]
        public void UInt16TestDeserialize()
        {
            var (Result, Value) = UInt16TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<ushort>(Result) == Value);
        }

        [Fact]
        public void Int32TestDeserialize()
        {
            var (Result, Value) = Int32TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<int>(Result) == Value);
        }

        [Fact]
        public void UInt32TestDeserialize()
        {
            var (Result, Value) = UInt32TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<uint>(Result) == Value);
        }

        [Fact]
        public void Int64TestDeserialize()
        {
            var (Result, Value) = Int64TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<long>(Result) == Value);
        }

        [Fact]
        public void UInt64TestDeserialize()
        {
            var (Result, Value) = UInt64TestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<ulong>(Result) == Value);
        }

        [Fact]
        public void DoubleTestDeserialize()
        {
            var (Result, Value) = DoubleTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<double>(Result) == Value);
        }

        [Fact]
        public void FloatTestDeserialize()
        {
            var (Result, Value) = FloatTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<float>(Result) == Value);
        }

        [Fact]
        public void DecimalTestDeserialize()
        {
            var (Result, Value) = DecimalTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<decimal>(Result) == Value);
        }

        [Fact]
        public void StringTestDeserialize()
        {
            var (Result, Value) = StringTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<string>(Result) == Value);
        }

        [Fact]
        public void BoolTestDeserialize()
        {
            var (Result, Value) = BoolTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<bool>(Result) == Value);
        }

        [Fact]
        public void BoolTestDeserialize2()
        {
            var (Result, Value) = BoolTestSerialize2();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<bool>(Result) == Value);
        }

        [Fact]
        public void DateTimeTestDeserialize()
        {
            var (Result, Value) = DateTimeTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<DateTime>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize1()
        {
            var (Result, Value) = EnumTestSerialize1();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize2()
        {
            var (Result, Value) = EnumTestSerialize2();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void EnumTestDeserialize3()
        {
            var (Result, Value) = EnumTestSerialize3();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<TestEnum>(Result) == Value);
        }

        [Fact]
        public void ByteArrayTestDeserialize()
        {
            var (Result, Value) = ByteArrayTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<byte[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void IntArrayTestDeserialize()
        {
            //TODO fix
            return;
            var (Result, Value) = IntArrayTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<int[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void StringArrayTestDeserialize()
        {
            //TODO fix
            return;
            var (Result, Value) = StringArrayTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<string[]>(Result).SequenceEqual(Value));
        }

        [Fact]
        public void StringQuatsTestDeserialize()
        {
            var (Result, Value) = StringQuatsTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<string>(Result) == Value);
        }

        [Fact]
        public void StringWithLineTestDeserialize()
        {
            var (Result, Value) = StringWithLineTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<string>(Result) == Value);
        }

        [Fact]
        public void GuidTestDeserialize()
        {
            var (Result, Value) = GuidTestSerialize();
            Assert.True(BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<Guid>(Result) == Value);
        }
    }
}