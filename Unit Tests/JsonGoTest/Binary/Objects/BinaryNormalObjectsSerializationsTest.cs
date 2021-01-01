using JsonGoTest.Models.Inheritance;
using JsonGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JsonGoTest.Binary.Objects
{
    public class BinaryNormalObjectsSerializationsTest
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
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize()
        {
            var value = GetSimpleUser();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize2()
        {
            var value = GetSimpleUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize3()
        {
            var value = GetSimpleUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
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
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize()
        {
            var value = GetSimpleParentUser();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize2()
        {
            var value = GetSimpleParentUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize3()
        {
            var value = GetSimpleParentUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }
        #endregion

        public static string GetText(Span<byte> bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append($"result[{i}] == {bytes[i]} && ");
            }
            return builder.ToString();
        }
    }
}
