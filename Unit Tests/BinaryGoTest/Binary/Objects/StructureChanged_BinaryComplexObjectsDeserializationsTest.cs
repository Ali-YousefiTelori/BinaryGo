using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Helpers;
using BinaryGoTest.Models.Complex;
using BinaryGoTest.Models.StructureChanged.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class StructureChanged_BinaryComplexObjectsDeserializationsTest : BinaryComplexObjectsSerializationsTest
    {

        public void ComplexUserTestDeserializeBase(byte[] Result, ComplexUser Value, BaseOptionInfo SerializerOptions)
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
            myDeserializer.Options.GenerateType<ComplexUserOldStructureInfo>();
            //add model renamed
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(ComplexUser)), typeof(ComplexUserOldStructureInfo));
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(ComplexCompanyInfo)), typeof(ComplexCompanyOldStructureInfo));
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(ComplexCompanyInfo[])), typeof(ComplexCompanyOldStructureInfo[]));
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(ComplexCarInfo)), typeof(ComplexCarOldStructureInfo));
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(ComplexCarInfo[])), typeof(ComplexCarOldStructureInfo[]));
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(CompanyType)), typeof(int));
            //build new structure to old structure
            myDeserializer.BuildStructure(newStructureModels);
            #endregion

            var result = myDeserializer.Deserialize<ComplexUserOldStructureInfo>(Result);
            Assert.True(result.IsEquals(Value));

            //now serialize from client side and deserialize from server side happen
            result.Phone = "my phoe number :)";
            BinarySerializer binarySerializer = new BinarySerializer(myDeserializer.Options);
            var resultSerialized = binarySerializer.Serialize(result);
            var resultDeserialized = myDeserializer.Deserialize<ComplexUserOldStructureInfo>(resultSerialized);
            Assert.True(resultDeserialized.IsEquals(Value));
        }

        [Fact]
        public void SimpleUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, ComplexUser Value, BaseOptionInfo SerializerOptions) = ComplexUserTestSerialize();
            ComplexUserTestDeserializeBase(Result, Value, SerializerOptions);
        }
    }
}
