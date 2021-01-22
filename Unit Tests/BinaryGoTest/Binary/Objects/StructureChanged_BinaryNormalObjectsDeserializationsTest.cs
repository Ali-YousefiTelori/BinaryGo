using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Helpers;
using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using BinaryGoTest.Models.StructureChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class StructureChanged_BinaryNormalObjectsDeserializationsTest : StructureChanged_BinaryNormalObjectsSerializationsTest
    {
        #region SimpleUser
        public void SimpleUserTestDeserializeBase(byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions)
        {
            //in this example server side has SimpleUserInfo
            //server side has Id, Name, Family
            //and the client side has SimpleUserOldStructureInfo
            //client side has Id, Age, BirthDate ,Name

            //new structure of models
            var newStructureModels = BinarySerializer.GetStructureModels(SerializerOptions);

            //my old deserializer
            var myDeserializer = new BinaryDeserializer();
            myDeserializer.Options = new BinaryGo.Helpers.BaseOptionInfo();

            #region VersionChangedControl
            //generate type
            myDeserializer.Options.GenerateType<SimpleUserOldStructureInfo>();
            //add model renamed
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(SimpleUserInfo)), typeof(SimpleUserOldStructureInfo));
            //build new structure to old structure
            myDeserializer.BuildStructure(newStructureModels);
            #endregion

            var result = myDeserializer.Deserialize<SimpleUserOldStructureInfo>(Result);
            Assert.True(result.IsEquals(Value));

            //now serialize from client side and deserialize from server side happen
            result.Age = 150;
            result.BirthDate = DateTime.Now.AddYears(-20);
            BinarySerializer binarySerializer = new BinarySerializer(myDeserializer.Options);
            var resultSerialized = binarySerializer.Serialize(result);
            var resultDeserialized = myDeserializer.Deserialize<SimpleUserOldStructureInfo>(resultSerialized);
            Assert.True(resultDeserialized.IsEquals(Value));
        }

        public void SimpleParentUserTestDeserializeBase(byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions)
        {
            //in this example server side has SimpleUserInfo
            //server side has Id, Name, Family
            //and the client side has SimpleUserOldStructureInfo
            //client side has Id, Age, BirthDate ,Name

            //new structure of models
            var newStructureModels = BinarySerializer.GetStructureModels(SerializerOptions);

            //my old deserializer
            var myDeserializer = new BinaryDeserializer();
            myDeserializer.Options = new BinaryGo.Helpers.BaseOptionInfo();

            #region VersionChangedControl
            //generate type
            myDeserializer.Options.GenerateType<SimpleParentUserOldStructureInfo>();
            //add model renamed
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(SimpleParentUserInfo)), typeof(SimpleParentUserOldStructureInfo));
            //build new structure to old structure
            myDeserializer.BuildStructure(newStructureModels);
            #endregion

            var result = myDeserializer.Deserialize<SimpleParentUserOldStructureInfo>(Result);
            Assert.True(result.IsEquals(Value));

            //now serialize from client side and deserialize from server side happen
            result.Passport = "AV12345678";
            BinarySerializer binarySerializer = new BinarySerializer(myDeserializer.Options);
            var resultSerialized = binarySerializer.Serialize(result);
            var resultDeserialized = myDeserializer.Deserialize<SimpleParentUserOldStructureInfo>(resultSerialized);
            Assert.True(resultDeserialized.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize();
            SimpleUserTestDeserializeBase(Result, Value, SerializerOptions);
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize2();
            SimpleUserTestDeserializeBase(Result, Value, SerializerOptions);
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize3();
            SimpleUserTestDeserializeBase(Result, Value, SerializerOptions);
        }


        [Fact]
        public void SimpleParentUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize();
            SimpleParentUserTestDeserializeBase(Result, Value, SerializerOptions);
        }

        [Fact]
        public void SimpleParentUserTestDeserialize2()
        {

            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize2();
            SimpleParentUserTestDeserializeBase(Result, Value, SerializerOptions);
        }

        [Fact]
        public void SimpleParentUserTestDeserialize3()
        {

            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, SimpleParentUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleParentUserTestSerialize3();
            SimpleParentUserTestDeserializeBase(Result, Value, SerializerOptions);
        }
        #endregion
    }
}
