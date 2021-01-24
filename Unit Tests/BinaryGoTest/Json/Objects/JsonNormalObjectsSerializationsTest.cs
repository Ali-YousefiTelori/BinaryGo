using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryGoTest.Json.Objects
{
    public class JsonNormalObjectsSerializationsTest
    {

        #region SimpleUser
        public SimpleUserInfo GetSimpleUser()
        {
            return new SimpleUserInfo()
            {
                Id = 2751,
                Name = "Ali",
                Family = "Yousefi Telori"
            };
        }


        public SimpleUserInfo GetSimpleUser2()
        {
            return new SimpleUserInfo()
            {
                Id = 2751,
                Name = "Ali \" \r \n new line \r\n \t end",
                Family = "Yousefi \"Telori\""
            };
        }

        public SimpleUserInfo GetSimpleUser3()
        {
            return new SimpleUserInfo()
            {
                Id = -9874,
                Name = "Ali \" \r \n new line \r\n \t end",
                Family = "Yousefi \"Telori\" {\"name\": \"value\"}"
            };
        }

        [Fact]
        public (string Result, SimpleUserInfo Value) SimpleUserTestSerialize()
        {
            var value = GetSimpleUser();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "{\"Id\":2751,\"Name\":\"Ali\",\"Family\":\"Yousefi Telori\"}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, SimpleUserInfo Value) SimpleUserTestSerialize2()
        {
            var value = GetSimpleUser2();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            var jsonSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Assert.True(result == "{\"Id\":2751,\"Name\":\"Ali \\\" \\r \\n new line \\r\\n \\t end\",\"Family\":\"Yousefi \\\"Telori\\\"\"}", $"Your Value: {value} Serialize Value: {result} and jsonSerialize: {jsonSerialize}");
            return (result, value);
        }

        [Fact]
        public (string Result, SimpleUserInfo Value) SimpleUserTestSerialize3()
        {
            var value = GetSimpleUser3();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "{\"Id\":-9874,\"Name\":\"Ali \\\" \\r \\n new line \\r\\n \\t end\",\"Family\":\"Yousefi \\\"Telori\\\" {\\\"name\\\": \\\"value\\\"}\"}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        #endregion

        #region SimpleUserInheritance
        public SimpleParentUserInfo GetSimpleParentUser()
        {
            return new SimpleParentUserInfo()
            {
                Id = 2751,
                Name = "Ali",
                Family = "Yousefi Telori"
            };
        }


        public SimpleParentUserInfo GetSimpleParentUser2()
        {
            return new SimpleParentUserInfo()
            {
                Id = 2751,
                Name = "Ali \" \r \n new line \r\n \t end",
                Family = "Yousefi \"Telori\""
            };
        }

        public SimpleParentUserInfo GetSimpleParentUser3()
        {
            return new SimpleParentUserInfo()
            {
                Id = -9874,
                Name = "Ali \" \r \n new line \r\n \t end",
                Family = "Yousefi \"Telori\" {\"name\": \"value\"}"
            };
        }

        [Fact]
        public (string Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize()
        {
            var value = GetSimpleParentUser();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "{\"Id\":2751,\"Family\":\"Yousefi Telori\",\"Weight\":0,\"Name\":\"Ali\"}", $"Your Value: {value} Serialize Value: {result}");
            var result2 = BinaryGo.Json.Serializer.NormalInstance.SerializeToBytes(value);
            var valueaa = Encoding.UTF8.GetString(result2.ToArray());
            var iseq = valueaa == result;
            return (result, value);
        }

        [Fact]
        public (string Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize2()
        {
            var value = GetSimpleParentUser2();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "{\"Id\":2751,\"Family\":\"Yousefi \\\"Telori\\\"\",\"Weight\":0,\"Name\":\"Ali \\\" \\r \\n new line \\r\\n \\t end\"}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }

        [Fact]
        public (string Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize3()
        {
            var value = GetSimpleParentUser3();
            var result = BinaryGo.Json.Serializer.NormalInstance.Serialize(value);
            Assert.True(result == "{\"Id\":-9874,\"Family\":\"Yousefi \\\"Telori\\\" {\\\"name\\\": \\\"value\\\"}\",\"Weight\":0,\"Name\":\"Ali \\\" \\r \\n new line \\r\\n \\t end\"}", $"Your Value: {value} Serialize Value: {result}");
            return (result, value);
        }
        #endregion
    }
}
