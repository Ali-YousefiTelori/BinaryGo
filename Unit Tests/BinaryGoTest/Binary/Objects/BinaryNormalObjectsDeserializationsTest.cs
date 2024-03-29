﻿using BinaryGo.Helpers;
using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class BinaryNormalObjectsDeserializationsTest : BinaryNormalObjectsSerializationsTest
    {
        #region SimpleUser

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            ObjectEqual(result, Value);
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize2();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            ObjectEqual(result, Value);
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize3();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            ObjectEqual(result, Value);
        }

        #endregion

        #region SimpleUserInheritance

        [Fact]
        public void SimpleParentUserTestDeserialize()
        {
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            ObjectEqual(result, Value);
        }

        [Fact]
        public void SimpleParentUserTestDeserialize2()
        {
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize2();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            ObjectEqual(result, Value);
        }

        [Fact]
        public void SimpleParentTestDeserialize3()
        {
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize3();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            ObjectEqual(result, Value);
        }
        #endregion
    }
}
