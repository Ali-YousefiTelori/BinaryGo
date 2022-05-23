using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Helpers;
using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using BinaryGoTest.Models.StructureChanged;
using System;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class StructureChanged_BinaryNormalObjectsDeserializationsTest : BinaryNormalObjectsSerializationsTest
    {
        #region SimpleUser
        
        [Fact]
        public void SimpleUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize();
            ServerModelTestDeserializeBase<SimpleUserInfo, SimpleUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Age = 150;
                clientModel.BirthDate = DateTime.Now.AddYears(-20);
            });
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize2();
            ServerModelTestDeserializeBase<SimpleUserInfo, SimpleUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Age = 150;
                clientModel.BirthDate = DateTime.Now.AddYears(-20);
            });
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize3();
            ServerModelTestDeserializeBase<SimpleUserInfo, SimpleUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Age = 150;
                clientModel.BirthDate = DateTime.Now.AddYears(-20);
            });
        }


        [Fact]
        public void SimpleParentUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize();
            ServerModelTestDeserializeBase<SimpleParentUserInfo, SimpleParentUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Passport = "AV12345678";
            });
        }

        [Fact]
        public void SimpleParentUserTestDeserialize2()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize2();
            ServerModelTestDeserializeBase<SimpleParentUserInfo, SimpleParentUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Passport = "AV12345678";
            });
        }

        [Fact]
        public void SimpleParentUserTestDeserialize3()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize3();
            ServerModelTestDeserializeBase<SimpleParentUserInfo, SimpleParentUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Passport = "AV12345678";
            });
        }

        #endregion
    }
}
