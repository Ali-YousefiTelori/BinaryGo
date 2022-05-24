using BinaryGo.Runtime.Variables.Structures;
using BinaryGoTest.Models;
using System;
using System.Linq;
using Xunit;

namespace BinaryGoTest.Binary.NullableVariables
{
    public class BinaryNormalNullableVariablesSerializationsTest : BaseTests
    {
        public (byte[] Result, T? Value) NullTestSerialize<T>()
            where T : struct
        {
            T? value = null;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 0 });
            return (result, value);
        }

        [Fact]
        public (byte[] Result, byte? Value) ByteTestSerialize()
        {
            byte? value = 45;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1, value.Value });
            NullTestSerialize<byte>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, sbyte? Value) SByteTestSerialize()
        {
            sbyte? value = -5;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1, (byte)value.Value });
            NullTestSerialize<sbyte>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, short? Value) Int16TestSerialize()
        {
            short? value = -1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<short>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ushort? Value) UInt16TestSerialize()
        {
            ushort? value = 1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<ushort>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, int? Value) Int32TestSerialize()
        {
            int? value = -1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<int>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, uint? Value) UInt32TestSerialize()
        {
            uint? value = 1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<uint>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, long? Value) Int64TestSerialize()
        {
            long? value = -4727327827885;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<long>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ulong? Value) UInt64TestSerialize()
        {
            ulong? value = 4727327827885;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<ulong>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, double? Value) DoubleTestSerialize()
        {
            double? value = -1582.5453;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<double>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, float? Value) FloatTestSerialize()
        {
            float? value = 52.66f;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<float>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, decimal? Value) DecimalTestSerialize()
        {
            decimal? value = 453445.54245m;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            DecimalStruct decimalStruct = new DecimalStruct()
            {
                Byte0 = result[1],
                Byte1 = result[2],
                Byte2 = result[3],
                Byte3 = result[4],
                Byte4 = result[5],
                Byte5 = result[6],
                Byte6 = result[7],
                Byte7 = result[8],
                Byte8 = result[9],
                Byte9 = result[10],
                Byte10 = result[11],
                Byte11 = result[12],
                Byte12 = result[13],
                Byte13 = result[14],
                Byte14 = result[15],
                Byte15 = result[16]
            };

            SequenceEqual(result, new byte[] { 1 }.Concat(new byte[]
            {
                decimalStruct.Byte0,
                decimalStruct.Byte1,
                decimalStruct.Byte2,
                decimalStruct.Byte3,
                decimalStruct.Byte4,
                decimalStruct.Byte5,
                decimalStruct.Byte6,
                decimalStruct.Byte7,
                decimalStruct.Byte8,
                decimalStruct.Byte9,
                decimalStruct.Byte10,
                decimalStruct.Byte11,
                decimalStruct.Byte12,
                decimalStruct.Byte13,
                decimalStruct.Byte14,
                decimalStruct.Byte15
            }).ToArray());
            NullTestSerialize<decimal>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringTestSerialize()
        {
            string value = "ali yousefi";
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            TextEqual(result, value);
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool? Value) BoolTestSerialize()
        {
            bool? value = true;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<bool>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool? Value) BoolTestSerialize2()
        {
            bool? value = false;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value)).ToArray());
            NullTestSerialize<bool>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, DateTime? Value) DateTimeTestSerialize()
        {
            DateTime? value = DateTime.Now;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value.Ticks)).ToArray());
            NullTestSerialize<DateTime>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TimeSpan? Value) TimeSpanTestSerialize()
        {
            TimeSpan? value = TimeSpan.Parse("10:20");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value.Ticks)).ToArray());
            NullTestSerialize<TimeSpan>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum? Value) EnumTestSerialize1()
        {
            TestEnum? value = TestEnum.None;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes((int)value.Value)).ToArray());
            NullTestSerialize<TestEnum>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum? Value) EnumTestSerialize2()
        {
            TestEnum? value = TestEnum.Value10;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes((int)value.Value)).ToArray());
            NullTestSerialize<TestEnum>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum? Value) EnumTestSerialize3()
        {
            TestEnum? value = TestEnum.Value50;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes((int)value.Value)).ToArray());
            NullTestSerialize<TestEnum>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, byte[] Value) ByteArrayTestSerialize()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, BitConverter.GetBytes(value.Length).Concat(value).ToArray());
            return (value, result);
        }

        [Fact]
        public (byte[] Result, int[] Value) IntArrayTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            var bytes = value.SelectMany(x => BitConverter.GetBytes(x)).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            SequenceEqual(result, bytes.ToArray());
            return (result, value);
        }

        [Fact]
        public (byte[] Result, Guid? Value) GuidTestSerialize()
        {
            Guid? value = Guid.NewGuid();
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(value.Value.ToByteArray()).ToArray());
            NullTestSerialize<Guid>();
            return (result, value);
        }
#if (NET6_0)     
        [Fact]
        public (byte[] Result, TimeOnly? Value) TimeOnlyTestSerialize()
        {
            TimeOnly? value = TimeOnly.Parse("10:20");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value.Ticks)).ToArray());
            NullTestSerialize<TimeOnly>();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, DateOnly? Value) DateOnlyTestSerialize()
        {
            DateOnly? value = DateOnly.Parse("2022-05-22");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            SequenceEqual(result, new byte[] { 1 }.Concat(BitConverter.GetBytes(value.Value.ToDateTime(TimeOnly.MinValue).Ticks)).ToArray());
            NullTestSerialize<TimeOnly>();
            return (result, value);
        }
#endif
    }
}