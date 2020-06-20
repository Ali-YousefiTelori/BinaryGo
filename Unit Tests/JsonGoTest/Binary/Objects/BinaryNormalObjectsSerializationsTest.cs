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
            Assert.True(result[0] == 1 && result[1] == 191 && result[2] == 10 && result[3] == 0 && result[4] == 0 && result[5] == 3 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 65 && result[10] == 108 && result[11] == 105 && result[12] == 14 && result[13] == 0 && result[14] == 0 && result[15] == 0 && result[16] == 89 && result[17] == 111 && result[18] == 117 && result[19] == 115 && result[20] == 101 && result[21] == 102 && result[22] == 105 && result[23] == 32 && result[24] == 84 && result[25] == 101 && result[26] == 108 && result[27] == 111 && result[28] == 114 && result[29] == 105);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize2()
        {
            var value = GetSimpleUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 1 && result[1] == 191 && result[2] == 10 && result[3] == 0 && result[4] == 0 && result[5] == 27 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 65 && result[10] == 108 && result[11] == 105 && result[12] == 32 && result[13] == 34 && result[14] == 32 && result[15] == 13 && result[16] == 32 && result[17] == 10 && result[18] == 32 && result[19] == 110 && result[20] == 101 && result[21] == 119 && result[22] == 32 && result[23] == 108 && result[24] == 105 && result[25] == 110 && result[26] == 101 && result[27] == 32 && result[28] == 13 && result[29] == 10 && result[30] == 32 && result[31] == 9 && result[32] == 32 && result[33] == 101 && result[34] == 110 && result[35] == 100 && result[36] == 16 && result[37] == 0 && result[38] == 0 && result[39] == 0 && result[40] == 89 && result[41] == 111 && result[42] == 117 && result[43] == 115 && result[44] == 101 && result[45] == 102 && result[46] == 105 && result[47] == 32 && result[48] == 34 && result[49] == 84 && result[50] == 101 && result[51] == 108 && result[52] == 111 && result[53] == 114 && result[54] == 105 && result[55] == 34);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleUserInfo Value) SimpleUserTestSerialize3()
        {
            var value = GetSimpleUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 1 && result[1] == 110 && result[2] == 217 && result[3] == 255 && result[4] == 255 && result[5] == 27 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 65 && result[10] == 108 && result[11] == 105 && result[12] == 32 && result[13] == 34 && result[14] == 32 && result[15] == 13 && result[16] == 32 && result[17] == 10 && result[18] == 32 && result[19] == 110 && result[20] == 101 && result[21] == 119 && result[22] == 32 && result[23] == 108 && result[24] == 105 && result[25] == 110 && result[26] == 101 && result[27] == 32 && result[28] == 13 && result[29] == 10 && result[30] == 32 && result[31] == 9 && result[32] == 32 && result[33] == 101 && result[34] == 110 && result[35] == 100 && result[36] == 34 && result[37] == 0 && result[38] == 0 && result[39] == 0 && result[40] == 89 && result[41] == 111 && result[42] == 117 && result[43] == 115 && result[44] == 101 && result[45] == 102 && result[46] == 105 && result[47] == 32 && result[48] == 34 && result[49] == 84 && result[50] == 101 && result[51] == 108 && result[52] == 111 && result[53] == 114 && result[54] == 105 && result[55] == 34 && result[56] == 32 && result[57] == 123 && result[58] == 34 && result[59] == 110 && result[60] == 97 && result[61] == 109 && result[62] == 101 && result[63] == 34 && result[64] == 58 && result[65] == 32 && result[66] == 34 && result[67] == 118 && result[68] == 97 && result[69] == 108 && result[70] == 117 && result[71] == 101 && result[72] == 34 && result[73] == 125);
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
            Assert.True(result[0] == 1 && result[1] == 191 && result[2] == 10 && result[3] == 0 && result[4] == 0 && result[5] == 14 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 89 && result[10] == 111 && result[11] == 117 && result[12] == 115 && result[13] == 101 && result[14] == 102 && result[15] == 105 && result[16] == 32 && result[17] == 84 && result[18] == 101 && result[19] == 108 && result[20] == 111 && result[21] == 114 && result[22] == 105 && result[23] == 3 && result[24] == 0 && result[25] == 0 && result[26] == 0 && result[27] == 65 && result[28] == 108 && result[29] == 105);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize2()
        {
            var value = GetSimpleParentUser2();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 1 && result[1] == 191 && result[2] == 10 && result[3] == 0 && result[4] == 0 && result[5] == 16 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 89 && result[10] == 111 && result[11] == 117 && result[12] == 115 && result[13] == 101 && result[14] == 102 && result[15] == 105 && result[16] == 32 && result[17] == 34 && result[18] == 84 && result[19] == 101 && result[20] == 108 && result[21] == 111 && result[22] == 114 && result[23] == 105 && result[24] == 34 && result[25] == 27 && result[26] == 0 && result[27] == 0 && result[28] == 0 && result[29] == 65 && result[30] == 108 && result[31] == 105 && result[32] == 32 && result[33] == 34 && result[34] == 32 && result[35] == 13 && result[36] == 32 && result[37] == 10 && result[38] == 32 && result[39] == 110 && result[40] == 101 && result[41] == 119 && result[42] == 32 && result[43] == 108 && result[44] == 105 && result[45] == 110 && result[46] == 101 && result[47] == 32 && result[48] == 13 && result[49] == 10 && result[50] == 32 && result[51] == 9 && result[52] == 32 && result[53] == 101 && result[54] == 110 && result[55] == 100);
            return (result.ToArray(), value);
        }

        [Fact]
        public (byte[] Result, SimpleParentUserInfo Value) SimpleParentUserTestSerialize3()
        {
            var value = GetSimpleParentUser3();
            var result = JsonGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            Assert.True(result[0] == 1 && result[1] == 110 && result[2] == 217 && result[3] == 255 && result[4] == 255 && result[5] == 34 && result[6] == 0 && result[7] == 0 && result[8] == 0 && result[9] == 89 && result[10] == 111 && result[11] == 117 && result[12] == 115 && result[13] == 101 && result[14] == 102 && result[15] == 105 && result[16] == 32 && result[17] == 34 && result[18] == 84 && result[19] == 101 && result[20] == 108 && result[21] == 111 && result[22] == 114 && result[23] == 105 && result[24] == 34 && result[25] == 32 && result[26] == 123 && result[27] == 34 && result[28] == 110 && result[29] == 97 && result[30] == 109 && result[31] == 101 && result[32] == 34 && result[33] == 58 && result[34] == 32 && result[35] == 34 && result[36] == 118 && result[37] == 97 && result[38] == 108 && result[39] == 117 && result[40] == 101 && result[41] == 34 && result[42] == 125 && result[43] == 27 && result[44] == 0 && result[45] == 0 && result[46] == 0 && result[47] == 65 && result[48] == 108 && result[49] == 105 && result[50] == 32 && result[51] == 34 && result[52] == 32 && result[53] == 13 && result[54] == 32 && result[55] == 10 && result[56] == 32 && result[57] == 110 && result[58] == 101 && result[59] == 119 && result[60] == 32 && result[61] == 108 && result[62] == 105 && result[63] == 110 && result[64] == 101 && result[65] == 32 && result[66] == 13 && result[67] == 10 && result[68] == 32 && result[69] == 9 && result[70] == 32 && result[71] == 101 && result[72] == 110 && result[73] == 100);
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
