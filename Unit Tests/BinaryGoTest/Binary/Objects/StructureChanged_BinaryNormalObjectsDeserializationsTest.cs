using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Helpers;
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

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            (byte[] Result, SimpleUserInfo Value, BaseOptionInfo SerializerOptions) = SimpleUserTestSerialize();
            //new structure of models
            var newStructureModels = BinarySerializer.GetStructureModels(SerializerOptions);
            
            //my old deserializer
            var myDeserializer = new BinaryDeserializer();
            myDeserializer.Options = new BinaryGo.Helpers.BaseOptionInfo();
            //generate type
            myDeserializer.Options.GenerateType<SimpleUserOldStructureInfo>();
            //add model renamed
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(SimpleUserInfo)), typeof(SimpleUserOldStructureInfo));
            //build new structure to old structure
            myDeserializer.BuildStructure(newStructureModels);
            var result = myDeserializer.Deserialize<SimpleUserOldStructureInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize2()
        {
            (byte[] Result, SimpleUserInfo Value) = SimpleUserTestSerialize2();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize3()
        {
            (byte[] Result, SimpleUserInfo Value) = SimpleUserTestSerialize3();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<SimpleUserInfo>(Result);
            Assert.True(result.IsEquals(Value));
        }

        #endregion
    }
}
