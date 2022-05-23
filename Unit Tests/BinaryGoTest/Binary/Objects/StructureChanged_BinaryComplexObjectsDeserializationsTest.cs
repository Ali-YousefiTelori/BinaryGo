using BinaryGo.Helpers;
using BinaryGoTest.Models.Complex;
using BinaryGoTest.Models.StructureChanged.Complex;
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
    }
}
