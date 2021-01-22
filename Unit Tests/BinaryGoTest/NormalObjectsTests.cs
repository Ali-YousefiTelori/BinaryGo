using BinaryGoTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryGoTest
{
    //public class NormalObjectsTests
    //{
    //    [Fact]
    //    public void UserInfoTest()
    //    {
    //        BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
    //        UserInfo userInfo = new UserInfo()
    //        {
    //            Age = 29,
    //            CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
    //            FullName = "Ali Yousefi",
    //            Id = 1,
    //            EMP_NO = 56
    //        };
    //        var result = serializer.Serialize(userInfo);
    //        var equalData = "{\"$id\":1,\"EMP_NO\":56,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\"}";
    //        Assert.True(result == equalData);
    //        var deserialized = BinaryGo.Json.Deserialize.JsonDeserializer.NormalInstance.Deserialize<UserInfo>(result);
    //        Assert.True(deserialized.IsEquals(userInfo));
    //    }
    //    [Fact]
    //    public void UserInfoNullableTest()
    //    {
    //        UserInfo userInfo = new UserInfo()
    //        {
    //            Age = 29,
    //            IsMarried = false,
    //            CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
    //            FullName = "Ali Yousefi",
    //            Id = 1,
    //        };
    //        BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
    //        var result = serializer.Serialize(userInfo);
    //        var equalData = "{\"$id\":1,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"IsMarried\":false,\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\"}";
    //        Assert.True(result == equalData);
    //        BinaryGo.Json.Deserialize.JsonDeserializer deserializer = new BinaryGo.Json.Deserialize.JsonDeserializer()
    //        {
    //            HasGenerateRefrencedTypes = true
    //        };
    //        var deserialized = deserializer.Deserialize<UserInfo>(result);
    //        Assert.True(deserialized.IsEquals(userInfo));
    //    }


    //    [Fact]
    //    public void UserInfoWithRolesTest()
    //    {
    //        return;
    //        BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
    //        UserInfo userInfo = new UserInfo()
    //        {
    //            Age = 29,
    //            CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
    //            FullName = "Ali Yousefi",
    //            Id = 1,
    //        };
    //        userInfo.Roles = new List<RoleInfo>()
    //        {
    //            new RoleInfo() { Id = 1, Type =  RoleType.Viewer, UserInfo = userInfo },
    //            new RoleInfo() { Id = 2, Type =  RoleType.Normal, UserInfo = userInfo }
    //        };

    //        var result = serializer.Serialize(userInfo);
    //        var equalData = "{\"$id\":1,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\",\"Roles\":{\"$id\":2,\"$values\":[{\"$id\":3,\"Id\":1,\"UserInfo\":{\"$ref\":1},\"Type\":3},{\"$id\":4,\"Id\":2,\"UserInfo\":{\"$ref\":1},\"Type\":2}]}}";
    //        Assert.True(result == equalData);
    //        BinaryGo.Json.Deserialize.JsonDeserializer deserializer = new BinaryGo.Json.Deserialize.JsonDeserializer()
    //        {
    //            HasGenerateRefrencedTypes = true
    //        };
    //        var deserialized = deserializer.Deserialize<UserInfo>(result);
    //        Assert.True(deserialized.IsEquals(userInfo));


    //    }

    //    [Fact]
    //    public void UserInfoWithRolesAndCompanyTest()
    //    {
    //        return;
    //        BinaryGo.Json.Serializer serializer = new BinaryGo.Json.Serializer(new BinaryGo.Helpers.BaseOptionInfo() { HasGenerateRefrencedTypes = true });
    //        CompanyInfo companyInfo = new CompanyInfo()
    //        {
    //            Id = 14,
    //            Users = new List<UserInfo>(),
    //            Name = "company test"
    //        };

    //        UserInfo userInfo = new UserInfo()
    //        {
    //            Age = 29,
    //            CreatedDate = DateTime.Parse("6/21/2019 12:53:26 PM"),
    //            FullName = "Ali Yousefi",
    //            Id = 1,
    //            CompanyInfo = companyInfo
    //        };
    //        companyInfo.Users.Add(userInfo);
    //        companyInfo.Users.Add(userInfo);
    //        userInfo.Roles = new List<RoleInfo>()
    //        {
    //            new RoleInfo() { Id = 1, Type =  RoleType.Viewer, UserInfo = userInfo },
    //            new RoleInfo() { Id = 2, Type =  RoleType.Normal, UserInfo = userInfo }
    //        };

    //        var result = serializer.Serialize(companyInfo);
    //        var equalData = "{\"$id\":1,\"Id\":14,\"Name\":\"company test\",\"Users\":{\"$id\":2,\"$values\":[{\"$id\":3,\"Id\":1,\"FullName\":\"Ali Yousefi\",\"Age\":29,\"CreatedDate\":\"6/21/2019 12:53:26 PM\",\"Roles\":{\"$id\":4,\"$values\":[{\"$id\":5,\"Id\":1,\"UserInfo\":{\"$ref\":3},\"Type\":3},{\"$id\":6,\"Id\":2,\"UserInfo\":{\"$ref\":3},\"Type\":2}]},\"CompanyInfo\":{\"$ref\":1}},{\"$ref\":3}]}}";
    //        Assert.True(result == equalData);
    //        BinaryGo.Json.Deserialize.JsonDeserializer deserializer = new BinaryGo.Json.Deserialize.JsonDeserializer()
    //        {
    //            HasGenerateRefrencedTypes = true
    //        };
    //        var deserialized = deserializer.Deserialize<CompanyInfo>(result);
    //        Assert.True(deserialized.IsEquals(companyInfo));
    //    }
    //}
}
