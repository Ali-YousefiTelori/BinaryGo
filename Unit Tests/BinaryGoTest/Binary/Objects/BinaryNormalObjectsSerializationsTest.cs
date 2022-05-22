using BinaryGo.Binary;
using BinaryGo.Helpers;
using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using System;
using System.Text;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class BinaryNormalObjectsSerializationsTest : BaseTests
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

        public BinarySerializer GetSerializer
        {
            get
            {
                var result = new BinarySerializer();
                result.Options = new BaseOptionInfo();
                return result;
            }
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) SimpleUserTestSerialize()
        {
            var value = GetSimpleUser();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) SimpleUserTestSerialize2()
        {
            var value = GetSimpleUser2();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) SimpleUserTestSerialize3()
        {
            var value = GetSimpleUser3();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
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
        public (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) SimpleParentUserTestSerialize()
        {
            var value = GetSimpleParentUser();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) SimpleParentUserTestSerialize2()
        {
            var value = GetSimpleParentUser2();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) SimpleParentUserTestSerialize3()
        {
            var value = GetSimpleParentUser3();
            var serializer = GetSerializer;
            var result = serializer.Serialize(value);
            return (result.ToArray(), value, serializer.Options);
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
