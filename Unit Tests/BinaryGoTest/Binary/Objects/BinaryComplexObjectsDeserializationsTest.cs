using BinaryGo.Helpers;
using BinaryGoTest.Models.Complex;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class BinaryComplexObjectsDeserializationsTest : BinaryComplexObjectsSerializationsTest
    {
        [Fact]
        public void ComplexUserTestDeserialize()
        {
            (byte[] Result, ComplexUser Value, BaseOptionInfo SerializerOptions) = ComplexUserTestSerialize();
            var result = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<ComplexUser>(Result);
            Assert.True(result.IsEquals(Value));
        }
    }
}
