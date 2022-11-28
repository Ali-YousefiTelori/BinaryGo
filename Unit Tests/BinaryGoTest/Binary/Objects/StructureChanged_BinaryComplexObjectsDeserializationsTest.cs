using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Binary.StructureModels;
using BinaryGo.Helpers;
using BinaryGo.Runtime;
using BinaryGoTest.Models.Complex;
using BinaryGoTest.Models.StructureChanged.Complex;
using System.Collections.Generic;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class StructureChanged_BinaryComplexObjectsDeserializationsTest : BinaryComplexObjectsSerializationsTest
    {
        [Fact]
        public void SimpleUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, ComplexUser Value, BaseOptionInfo SerializerOptions) = ComplexUserTestSerialize();
            ServerModelTestDeserializeBase<ComplexUser, ComplexUserOldStructureInfo>(Result, Value, SerializerOptions, (clientModel) =>
            {
                clientModel.Phone = "my phone number :)";
                foreach (var item in clientModel.Companies)
                {
                    item.Types = new System.Collections.Generic.List<ComplexTypeOldStructureInfo>()
                    {
                        new ComplexTypeOldStructureInfo()
                        {
                             TypeName = "My type name"
                        }
                    };
                }
            },
            (typeof(ComplexCompanyInfo), typeof(ComplexCompanyOldStructureInfo)),
            (typeof(ComplexCompanyInfo[]), typeof(ComplexCompanyOldStructureInfo[])),
            (typeof(ComplexCarInfo), typeof(ComplexCarOldStructureInfo)),
            (typeof(ComplexCarInfo[]), typeof(ComplexCarOldStructureInfo[])),
            (typeof(CompanyType), typeof(int)));
        }

        [Fact]
        public void SimpleJsonUserTestDeserialize()
        {
            //in thi lines serialize from server side and try to deserialize from client side happen
            (byte[] Result, ComplexUser Value, BaseOptionInfo SerializerOptions) = ComplexUserTestSerialize();
            string fullName = BinaryDeserializer.GetStrcutureModelName(typeof(ComplexUser));
            var jsonStructureModels = Newtonsoft.Json.JsonConvert.SerializeObject(BinarySerializer.GetStructureModels(SerializerOptions));
            var structureModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BinaryModelInfo>>(jsonStructureModels);


            //my old deserializer
            var myDeserializer = new BinaryDeserializer();
            myDeserializer.Options = new BinaryGo.Helpers.BaseOptionInfo();

            //generate type
            myDeserializer.Options.GenerateType<ComplexUserOldStructureInfo>();
            BaseTypeGoInfo.GenerateDefaultVariables(myDeserializer.Options);
            //add model renamed
            myDeserializer.AddMovedType(fullName, typeof(ComplexUserOldStructureInfo));
            myDeserializer.AddMovedType(BinaryDeserializer.GetStrcutureModelName(typeof(ComplexCompanyInfo)), typeof(ComplexCompanyOldStructureInfo));
            myDeserializer.AddMovedType(BinaryDeserializer.GetStrcutureModelName(typeof(ComplexCompanyInfo[])), typeof(ComplexCompanyOldStructureInfo[]));
            myDeserializer.AddMovedType(BinaryDeserializer.GetStrcutureModelName(typeof(ComplexCarInfo)), typeof(ComplexCarOldStructureInfo));
            myDeserializer.AddMovedType(BinaryDeserializer.GetStrcutureModelName(typeof(ComplexCarInfo[])), typeof(ComplexCarOldStructureInfo[]));
            myDeserializer.AddMovedType(BinaryDeserializer.GetStrcutureModelName(typeof(CompanyType)), typeof(int));

            //build new structure to old structure
            myDeserializer.BuildStructure(structureModels);

            var result = myDeserializer.Deserialize<ComplexUserOldStructureInfo>(Result);
            ObjectEqual(result, Value);
            //now serialize from client side and deserialize from server side happen
            result.Phone = "my phone number :)";
            foreach (var item in result.Companies)
            {
                item.Types = new System.Collections.Generic.List<ComplexTypeOldStructureInfo>()
                {
                    new ComplexTypeOldStructureInfo()
                    {
                         TypeName = "My type name"
                    }
                };
            }
            BinarySerializer binarySerializer = new BinarySerializer(myDeserializer.Options);
            var resultSerialized = binarySerializer.Serialize(result);
            var resultDeserialized = myDeserializer.Deserialize<ComplexUserOldStructureInfo>(resultSerialized);
            ObjectEqual(resultDeserialized, Value);
        }
    }
}
