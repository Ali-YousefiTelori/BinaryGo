using JsonGoTest.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoTest
{
    public class NormalObjectsTests
    {
        [Test]
        public void UserInfoTest()
        {
            UserInfo userInfo = new UserInfo()
            {
                Age = 29,
                CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
                FullName = "Ali Yousefi",
                Id = 1,
            };
            var result = JsonGo.Serializer.SingleIntance.Serialize(userInfo);
            Assert.IsTrue(result == "{\"$id\":\"1\",\"Id\":\"1\",\"FullName\":\"Ali Yousefi\",\"Age\":\"29\",\"CreatedDate\":\"6/21/2019 12:53:26 PM\"}");
            var deserialized = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(result);
            Assert.IsTrue(deserialized.Equals(userInfo));
        }
    }
}
