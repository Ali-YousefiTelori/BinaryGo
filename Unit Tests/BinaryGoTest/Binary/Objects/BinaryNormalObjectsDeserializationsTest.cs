using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class BinaryNormalObjectsDeserializationsTest : BinaryNormalObjectsSerializationsTest
    {
        #region SimpleUser

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            (byte[] Result, SimpleUserInfo Value) = SimpleUserTestSerialize();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            (byte[] Result, SimpleUserInfo Value) = SimpleUserTestSerialize2();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            (byte[] Result, SimpleUserInfo Value) = SimpleUserTestSerialize3();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        #endregion

        #region SimpleUserInheritance

        [Fact]
        public void SimpleParentUserTestDeserialize()
        {
            (byte[] Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleParentUserTestDeserialize2()
        {
            (byte[] Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize2();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleParentTestDeserialize3()
        {
            (byte[] Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize3();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }
        #endregion
    }
}
