using JsonGo.Runtime;
using JsonGoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonGoTest.Json.NullableVariables
{
    public class JsonNormalNullableVariablesSerializationsTest
    {

        public (string Result, T? Value) NullTestSerialize<T>(T? value, string valueExpected = null, bool hasQuats = false)
            where T : struct
        {
            var result = JsonGo.Json.Serializer.NormalInstance.Serialize(value);
            if (value.HasValue)
            {
                var stringValue = value.ToString();
                if (hasQuats)
                    stringValue = $"\"{stringValue}\"";
                if (valueExpected == null)
                    Assert.True(result == $"{stringValue}", $"Your Value: {stringValue} Serialize Value: {result}");
                else
                    Assert.True(result == $"{valueExpected}", $"Your Value: {valueExpected} Serialize Value: {result}");
                return (result, value.Value);
            }
            else
            {
                Assert.True(result == $"null", $"Your Value: {value} Serialize Value: {result}");
                return (result, default);
            }
        }

        [Fact]
        public (string Result, byte? Value) ByteTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<byte>(45);
            return (Result, Value);
        }

        [Fact]
        public (string Result, byte? Value) ByteNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<byte>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, sbyte? Value) SByteTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<sbyte>(-5);
            return (Result, Value);
        }

        [Fact]
        public (string Result, sbyte? Value) SByteNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<sbyte>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, short? Value) Int16TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<short>(-1582);
            return (Result, Value);
        }

        [Fact]
        public (string Result, short? Value) Int16NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<short>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, ushort? Value) UInt16TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<ushort>(1582);
            return (Result, Value);
        }

        [Fact]
        public (string Result, ushort? Value) UInt16NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<ushort>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, int? Value) Int32TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<int>(-1582);
            return (Result, Value);
        }

        [Fact]
        public (string Result, int? Value) Int32NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<int>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, uint? Value) UInt32TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<uint>(1582);
            return (Result, Value);
        }

        [Fact]
        public (string Result, uint? Value) UInt32NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<uint>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, long? Value) Int64TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<long>(-4727327827885);
            return (Result, Value);
        }

        [Fact]
        public (string Result, long? Value) Int64NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<long>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, ulong? Value) UInt64TestSerialize()
        {
            var (Result, Value) = NullTestSerialize<ulong>(4727327827885);
            return (Result, Value);
        }

        [Fact]
        public (string Result, ulong? Value) UInt64NullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<ulong>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, double? Value) DoubleTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<double>(-1582.5453);
            return (Result, Value);
        }

        [Fact]
        public (string Result, double? Value) DoubleNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<double>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, float? Value) FloatTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<float>(52.66f);
            return (Result, Value);
        }

        [Fact]
        public (string Result, float? Value) FloatNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<float>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, decimal? Value) DecimalTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<decimal>(453445.54245m);
            return (Result, Value);
        }

        [Fact]
        public (string Result, decimal? Value) DecimalNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<decimal>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, bool? Value) BoolTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<bool>(true, "true");
            NullTestSerialize<bool>(false, "false");

            return (Result, Value);
        }

        [Fact]
        public (string Result, bool? Value) BoolNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<bool>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, DateTime? Value) DateTimeTestSerialize()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var (Result, Value) = NullTestSerialize<DateTime>(value, null, true);
            return (Result, Value);
        }

        [Fact]
        public (string Result, DateTime? Value) DateTimeNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<DateTime>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, TestEnum? Value) EnumTestSerialize1()
        {
            BaseTypeGoInfo.Generate<TestEnum>(JsonGo.Json.Serializer.DefaultOptions);
            TestEnum value = TestEnum.None;
            var (Result, Value) = NullTestSerialize<TestEnum>(value, ((int)value).ToString());
            return (Result, Value);
        }

        [Fact]
        public (string Result, TestEnum? Value) EnumNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<TestEnum>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, TestEnum? Value) EnumTestSerialize2()
        {
            BaseTypeGoInfo.Generate<TestEnum>(JsonGo.Json.Serializer.DefaultOptions);
            TestEnum value = TestEnum.Value10;
            var (Result, Value) = NullTestSerialize<TestEnum>(value, ((int)value).ToString());
            NullTestSerialize<TestEnum>(null);
            return (Result, Value);
        }

        [Fact]
        public (string Result, TestEnum? Value) EnumTestSerialize3()
        {
            BaseTypeGoInfo.Generate<TestEnum>(JsonGo.Json.Serializer.DefaultOptions);
            TestEnum value = TestEnum.Value50;
            var (Result, Value) = NullTestSerialize<TestEnum>(value, ((int)value).ToString());
            return (Result, Value);
        }

        [Fact]
        public (string Result, Guid? Value) GuidTestSerialize()
        {
            Guid value = Guid.NewGuid();
            var (Result, Value) = NullTestSerialize<Guid>(value, hasQuats: true);
            return (Result, Value);
        }
        [Fact]
        public (string Result, Guid? Value) GuidNullTestSerialize()
        {
            var (Result, Value) = NullTestSerialize<Guid>(null);
            return (Result, Value);
        }
    }
}
