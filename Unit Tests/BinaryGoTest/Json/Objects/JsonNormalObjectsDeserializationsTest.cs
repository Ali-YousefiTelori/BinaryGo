using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryGoTest.Json.Objects
{
    public class JsonNormalObjectsDeserializationsTest : JsonNormalObjectsSerializationsTest
    {
        #region SimpleUser

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            (string Result, SimpleUserInfo Value) = SimpleUserTestSerialize();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            (string Result, SimpleUserInfo Value) = SimpleUserTestSerialize2();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            (string Result, SimpleUserInfo Value) = SimpleUserTestSerialize3();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        #endregion

        #region SimpleUserInheritance

        [Fact]
        public void SimpleParentUserTestDeserialize()
        {
            (string Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleParentUserTestDeserialize2()
        {
            (string Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize2();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleParentTestDeserialize3()
        {
            (string Result, SimpleParentUserInfo Value) = SimpleParentUserTestSerialize3();
            var result = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<SimpleParentUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }
        #endregion
    }
}
