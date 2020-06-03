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
            Assert.True(result[0] == 191 && result[1] == 10 && result[2] == 0 && result[3] == 0 && result[4] == 3 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 14 && result[12] == 0 && result[13] == 0 && result[14] == 0 && result[15] == 89 && result[16] == 111 && result[17] == 117 && result[18] == 115 && result[19] == 101 && result[20] == 102 && result[21] == 105 && result[22] == 32 && result[23] == 84 && result[24] == 101 && result[25] == 108 && result[26] == 111 && result[27] == 114 && result[28] == 105);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize2()
        {
            var value = GetSimpleUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 191 && result[1] == 10 && result[2] == 0 && result[3] == 0 && result[4] == 27 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 32 && result[12] == 34 && result[13] == 32 && result[14] == 13 && result[15] == 32 && result[16] == 10 && result[17] == 32 && result[18] == 110 && result[19] == 101 && result[20] == 119 && result[21] == 32 && result[22] == 108 && result[23] == 105 && result[24] == 110 && result[25] == 101 && result[26] == 32 && result[27] == 13 && result[28] == 10 && result[29] == 32 && result[30] == 9 && result[31] == 32 && result[32] == 101 && result[33] == 110 && result[34] == 100 && result[35] == 16 && result[36] == 0 && result[37] == 0 && result[38] == 0 && result[39] == 89 && result[40] == 111 && result[41] == 117 && result[42] == 115 && result[43] == 101 && result[44] == 102 && result[45] == 105 && result[46] == 32 && result[47] == 34 && result[48] == 84 && result[49] == 101 && result[50] == 108 && result[51] == 111 && result[52] == 114 && result[53] == 105 && result[54] == 34);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize3()
        {
            var value = GetSimpleUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 110 && result[1] == 217 && result[2] == 255 && result[3] == 255 && result[4] == 27 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 32 && result[12] == 34 && result[13] == 32 && result[14] == 13 && result[15] == 32 && result[16] == 10 && result[17] == 32 && result[18] == 110 && result[19] == 101 && result[20] == 119 && result[21] == 32 && result[22] == 108 && result[23] == 105 && result[24] == 110 && result[25] == 101 && result[26] == 32 && result[27] == 13 && result[28] == 10 && result[29] == 32 && result[30] == 9 && result[31] == 32 && result[32] == 101 && result[33] == 110 && result[34] == 100 && result[35] == 34 && result[36] == 0 && result[37] == 0 && result[38] == 0 && result[39] == 89 && result[40] == 111 && result[41] == 117 && result[42] == 115 && result[43] == 101 && result[44] == 102 && result[45] == 105 && result[46] == 32 && result[47] == 34 && result[48] == 84 && result[49] == 101 && result[50] == 108 && result[51] == 111 && result[52] == 114 && result[53] == 105 && result[54] == 34 && result[55] == 32 && result[56] == 123 && result[57] == 34 && result[58] == 110 && result[59] == 97 && result[60] == 109 && result[61] == 101 && result[62] == 34 && result[63] == 58 && result[64] == 32 && result[65] == 34 && result[66] == 118 && result[67] == 97 && result[68] == 108 && result[69] == 117 && result[70] == 101 && result[71] == 34 && result[72] == 125);
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
            Assert.True(result[0] == 191 && result[1] == 10 && result[2] == 0 && result[3] == 0 && result[4] == 3 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 14 && result[12] == 0 && result[13] == 0 && result[14] == 0 && result[15] == 89 && result[16] == 111 && result[17] == 117 && result[18] == 115 && result[19] == 101 && result[20] == 102 && result[21] == 105 && result[22] == 32 && result[23] == 84 && result[24] == 101 && result[25] == 108 && result[26] == 111 && result[27] == 114 && result[28] == 105);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize2()
        {
            var value = GetSimpleParentUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 191 && result[1] == 10 && result[2] == 0 && result[3] == 0 && result[4] == 27 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 32 && result[12] == 34 && result[13] == 32 && result[14] == 13 && result[15] == 32 && result[16] == 10 && result[17] == 32 && result[18] == 110 && result[19] == 101 && result[20] == 119 && result[21] == 32 && result[22] == 108 && result[23] == 105 && result[24] == 110 && result[25] == 101 && result[26] == 32 && result[27] == 13 && result[28] == 10 && result[29] == 32 && result[30] == 9 && result[31] == 32 && result[32] == 101 && result[33] == 110 && result[34] == 100 && result[35] == 16 && result[36] == 0 && result[37] == 0 && result[38] == 0 && result[39] == 89 && result[40] == 111 && result[41] == 117 && result[42] == 115 && result[43] == 101 && result[44] == 102 && result[45] == 105 && result[46] == 32 && result[47] == 34 && result[48] == 84 && result[49] == 101 && result[50] == 108 && result[51] == 111 && result[52] == 114 && result[53] == 105 && result[54] == 34);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize3()
        {
            var value = GetSimpleParentUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 110 && result[1] == 217 && result[2] == 255 && result[3] == 255 && result[4] == 27 && result[5] == 0 && result[6] == 0 && result[7] == 0 && result[8] == 65 && result[9] == 108 && result[10] == 105 && result[11] == 32 && result[12] == 34 && result[13] == 32 && result[14] == 13 && result[15] == 32 && result[16] == 10 && result[17] == 32 && result[18] == 110 && result[19] == 101 && result[20] == 119 && result[21] == 32 && result[22] == 108 && result[23] == 105 && result[24] == 110 && result[25] == 101 && result[26] == 32 && result[27] == 13 && result[28] == 10 && result[29] == 32 && result[30] == 9 && result[31] == 32 && result[32] == 101 && result[33] == 110 && result[34] == 100 && result[35] == 34 && result[36] == 0 && result[37] == 0 && result[38] == 0 && result[39] == 89 && result[40] == 111 && result[41] == 117 && result[42] == 115 && result[43] == 101 && result[44] == 102 && result[45] == 105 && result[46] == 32 && result[47] == 34 && result[48] == 84 && result[49] == 101 && result[50] == 108 && result[51] == 111 && result[52] == 114 && result[53] == 105 && result[54] == 34 && result[55] == 32 && result[56] == 123 && result[57] == 34 && result[58] == 110 && result[59] == 97 && result[60] == 109 && result[61] == 101 && result[62] == 34 && result[63] == 58 && result[64] == 32 && result[65] == 34 && result[66] == 118 && result[67] == 97 && result[68] == 108 && result[69] == 117 && result[70] == 101 && result[71] == 34 && result[72] == 125);
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
