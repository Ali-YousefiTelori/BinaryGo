using JsonGoTest.Models.Complex;
using Xunit;

namespace JsonGoTest.Binary.Objects
{
    public class BinaryComplexObjectsDeserializationsTest : BinaryComplexObjectsSerializationsTest
    {
        [Fact]
        public void ComplexUserTestDeserialize()
        {
            (byte[] Result, ComplexUser Value) = ComplexUserTestSerialize();
            var result = JsonGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<ComplexUser>(Result);
            Assert.True(result.IsEquals(Value));
        }
    }
}
