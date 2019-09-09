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
                EMP_NO = 56
            };
            var result = JsonGo.Serializer.SingleIntance.Serialize(userInfo);
            var equalData = "{\"$id\":1,\"EMP_NO\":56,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\"}";
            Assert.IsTrue(result == equalData);
            var deserialized = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(result);
            Assert.IsTrue(deserialized.IsEquals(userInfo));
        }
        [Test]
        public void UserInfoNullableTest()
        {
            UserInfo userInfo = new UserInfo()
            {
                Age = 29,
                IsMarried = false,
                CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
                FullName = "Ali Yousefi",
                Id = 1,
            };
            var result = JsonGo.Serializer.SingleIntance.Serialize(userInfo);
            var equalData = "{\"$id\":1,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"IsMarried\":false,\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\"}";
            Assert.IsTrue(result == equalData);
            var deserialized = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(result);
            Assert.IsTrue(deserialized.IsEquals(userInfo));
        }


        [Test]
        public void UserInfoWithRolesTest()
        {
            UserInfo userInfo = new UserInfo()
            {
                Age = 29,
                CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
                FullName = "Ali Yousefi",
                Id = 1,
            };
            userInfo.Roles = new List<RoleInfo>()
            {
                new RoleInfo() { Id = 1, Type =  RoleType.Viewer, UserInfo = userInfo },
                new RoleInfo() { Id = 2, Type =  RoleType.Normal, UserInfo = userInfo }
            };
            try
            {

                var result = JsonGo.Serializer.SingleIntance.Serialize(userInfo);
                var equalData = "{\"$id\":1,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\",\"Roles\":{\"$id\":2,\"$values\":[{\"$id\":3,\"Id\":1,\"UserInfo\":{\"$ref\":1},\"Type\":3},{\"$id\":4,\"Id\":2,\"UserInfo\":{\"$ref\":1},\"Type\":2}]}}";
                Assert.IsTrue(result == equalData);
                var deserialized = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<UserInfo>(result);
                Assert.IsTrue(deserialized.IsEquals(userInfo));
            }
            catch (Exception ex)
            {

            }


        }

        [Test]
        public void UserInfoWithRolesAndCompanyTest()
        {
            CompanyInfo companyInfo = new CompanyInfo()
            {
                Id = 14,
                Users = new List<UserInfo>(),
                Name = "company test"
            };

            UserInfo userInfo = new UserInfo()
            {
                Age = 29,
                CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
                FullName = "Ali Yousefi",
                Id = 1,
                CompanyInfo = companyInfo
            };
            companyInfo.Users.Add(userInfo);
            companyInfo.Users.Add(userInfo);
            userInfo.Roles = new List<RoleInfo>()
            {
                new RoleInfo() { Id = 1, Type =  RoleType.Viewer, UserInfo = userInfo },
                new RoleInfo() { Id = 2, Type =  RoleType.Normal, UserInfo = userInfo }
            };

            var result = JsonGo.Serializer.SingleIntance.Serialize(companyInfo);
            var equalData = "{\"$id\":1,\"Id\":14,\"Name\":\"company test\",\"Users\":{\"$id\":2,\"$values\":[{\"$id\":3,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\",\"Roles\":{\"$id\":4,\"$values\":[{\"$id\":5,\"Id\":1,\"UserInfo\":{\"$ref\":3},\"Type\":3},{\"$id\":6,\"Id\":2,\"UserInfo\":{\"$ref\":3},\"Type\":2}]},\"CompanyInfo\":{\"$ref\":1}},{\"$ref\":3}]}}";
            Assert.IsTrue(result == equalData);

            var deserialized = JsonGo.Deserialize.Deserializer.SingleIntance.Deserialize<CompanyInfo>(result);
            Assert.IsTrue(deserialized.IsEquals(companyInfo));

        }
    }
}
