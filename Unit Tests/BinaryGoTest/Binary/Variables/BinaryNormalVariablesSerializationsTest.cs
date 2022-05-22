using BinaryGo.Runtime.Variables.Structures;
using BinaryGoTest.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace BinaryGoTest.Binary.Variables
{
    public class BinaryNormalVariablesSerializationsTest
    {
        [Fact]
        public (byte[] Result, byte Value) ByteTestSerialize()
        {
            byte value = 45;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(new byte[] { value }), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, sbyte Value) SByteTestSerialize()
        {
            sbyte value = -5;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.Select(x => (sbyte)x).SequenceEqual(new sbyte[] { value }), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, short Value) Int16TestSerialize()
        {
            short value = -1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ushort Value) UInt16TestSerialize()
        {
            ushort value = 1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, int Value) Int32TestSerialize()
        {
            int value = -1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, uint Value) UInt32TestSerialize()
        {
            uint value = 1582;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, long Value) Int64TestSerialize()
        {
            long value = -4727327827885;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ulong Value) UInt64TestSerialize()
        {
            ulong value = 4727327827885;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, double Value) DoubleTestSerialize()
        {
            double value = -1582.5453;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, float Value) FloatTestSerialize()
        {
            float value = 52.66f;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, decimal Value) DecimalTestSerialize()
        {
            decimal value = 453445.54245m;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            DecimalStruct decimalStruct = new DecimalStruct()
            {
                Byte0 = result[0],
                Byte1 = result[1],
                Byte2 = result[2],
                Byte3 = result[3],
                Byte4 = result[4],
                Byte5 = result[5],
                Byte6 = result[6],
                Byte7 = result[7],
                Byte8 = result[8],
                Byte9 = result[9],
                Byte10 = result[10],
                Byte11 = result[11],
                Byte12 = result[12],
                Byte13 = result[13],
                Byte14 = result[14],
                Byte15 = result[15]
            };
            Assert.True(value == decimalStruct.Value, $"Your Value: {value} Serialize Value: {decimalStruct.Value}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringTestSerialize()
        {
            string value = "ali yousefi";
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool Value) BoolTestSerialize()
        {
            bool value = true;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool Value) BoolTestSerialize2()
        {
            bool value = false;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, DateTime Value) DateTimeTestSerialize()
        {
            DateTime value = DateTime.Now;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value.Ticks)), $"Your Value: {value} Serialize Value: {new DateTime(BitConverter.ToInt64(result))}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TimeSpan Value) TimeSpanTestSerialize()
        {
            TimeSpan value = TimeSpan.Parse("10:20");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value.Ticks)), $"Your Value: {value} Serialize Value: {new TimeSpan(BitConverter.ToInt64(result))}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize1()
        {
            TestEnum value = TestEnum.None;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize2()
        {
            var value = TestEnum.Value10;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize3()
        {
            var value = TestEnum.Value50;
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, byte[] Value) ByteArrayTestSerialize()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            var bytes = value.ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, int[] Value) IntArrayTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            var bytes = value.SelectMany(x => BitConverter.GetBytes(x)).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");

            return (result, value);
        }

        [Fact]
        public (byte[] Result, int[] Value) IntArrayValueReferenceTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            BinaryGo.Binary.BinarySerializer serializer = new BinaryGo.Binary.BinarySerializer(new BinaryGo.Helpers.BaseOptionInfo());
            var result = serializer.Serialize(value).ToArray();
            var bytes = value.SelectMany(x => BitConverter.GetBytes(x)).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string[] Value) StringArrayTestSerialize()
        {
            string[] value = new string[] { "5", "1ss0", "9fg5", "25dd" };
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringQuatsTestSerialize()
        {
            string value = "salam\"\"ddv sdd {} [] \"";
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringWithLineTestSerialize()
        {
            string value = $"test hello: \"my name is{Environment.NewLine}ali{Environment.NewLine}then yousefi\" so we are good now\"";
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            return (result, value);
        }

        [Fact]
        public (byte[] Result, Guid Value) GuidTestSerialize()
        {
            Guid value = Guid.NewGuid();
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(value.ToByteArray()), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TimeOnly Value) TimeOnlyTestSerialize()
        {
            TimeOnly value = TimeOnly.Parse("10:20");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value.Ticks)), $"Your Value: {value} Serialize Value: {new TimeOnly(BitConverter.ToInt64(result))}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, DateOnly Value) DateOnlyTestSerialize()
        {
            DateOnly value = DateOnly.Parse("2022-05-22");
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value.ToDateTime(TimeOnly.MinValue).Ticks)), $"Your Value: {value} Serialize Value: {DateOnly.FromDateTime(new DateTime(BitConverter.ToInt64(result)))}");
            return (result, value);
        }
    }
}