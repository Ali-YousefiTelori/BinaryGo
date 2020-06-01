using JsonGoTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace JsonGoTest.Binary.Variables
{
    public class BinaryNormalVariablesSerializationsTest
    {
        [Fact]
        public (byte[] Result, byte Value) ByteTestSerialize()
        {
            byte value = 45;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(new byte[] { value }), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, sbyte Value) SByteTestSerialize()
        {
            sbyte value = -5;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.Select(x => (sbyte)x).SequenceEqual(new sbyte[] { value }), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, short Value) Int16TestSerialize()
        {
            short value = -1582;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ushort Value) UInt16TestSerialize()
        {
            ushort value = 1582;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, int Value) Int32TestSerialize()
        {
            int value = -1582;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, uint Value) UInt32TestSerialize()
        {
            uint value = 1582;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, long Value) Int64TestSerialize()
        {
            long value = -4727327827885;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, ulong Value) UInt64TestSerialize()
        {
            ulong value = 4727327827885;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, double Value) DoubleTestSerialize()
        {
            double value = -1582.5453;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, float Value) FloatTestSerialize()
        {
            float value = 52.66f;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, decimal Value) DecimalTestSerialize()
        {
            decimal value = 453445.54245m;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((double)value)), $"Your Value: {value} Serialize Value: {result}"); return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringTestSerialize()
        {
            string value = "ali yousefi";
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = Encoding.UTF8.GetBytes(value).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool Value) BoolTestSerialize()
        {
            bool value = true;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, bool Value) BoolTestSerialize2()
        {
            bool value = false;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, DateTime Value) DateTimeTestSerialize()
        {
            DateTime value = DateTime.Now;
            value = value.AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes(value.Ticks)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize1()
        {
            TestEnum value = TestEnum.None;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize2()
        {
            var value = TestEnum.Value10;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, TestEnum Value) EnumTestSerialize3()
        {
            var value = TestEnum.Value50;
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            Assert.True(result.SequenceEqual(BitConverter.GetBytes((int)value)), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, byte[] Value) ByteArrayTestSerialize()
        {
            byte[] value = new byte[] { 5, 10, 95, 32 };
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = value.ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, int[] Value) IntArrayTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = value.SelectMany(x => BitConverter.GetBytes(x)).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");

            return (result, value);
        }

        [Fact]
        public (byte[] Result, int[] Value) IntArrayValueReferenceTestSerialize()
        {
            int[] value = new int[] { 5, 10, 95, 32 };
            JsonGo.Binary.BinarySerializer serializer = new JsonGo.Binary.BinarySerializer(new JsonGo.Json.JsonOptionInfo());
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
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = value.SelectMany(x =>
            {
                var bytes = Encoding.UTF8.GetBytes(x).ToList();
                bytes.InsertRange(0, BitConverter.GetBytes(x.Length));
                return bytes;
            }).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringQuatsTestSerialize()
        {
            string value = "salam\"\"ddv sdd {} [] \"";
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = Encoding.UTF8.GetBytes(value).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (byte[] Result, string Value) StringWithLineTestSerialize()
        {
            string value = @"test hello: ""my name is
ali
then yousefi"" so we are good now""";
            var result = JsonGo.Binary.BinarySerializer.NormalIntance.Serialize(value).ToArray();
            var bytes = Encoding.UTF8.GetBytes(value).ToList();
            bytes.InsertRange(0, BitConverter.GetBytes(value.Length));
            Assert.True(result.SequenceEqual(bytes), $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
    }
}