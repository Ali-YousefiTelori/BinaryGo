using System;
using System.Linq;
using System.Text;
using Xunit;

namespace BinaryGoTest
{
    public class BaseTests
    {
        public void SequenceEqual(byte[] result, byte[] value)
        {
            Assert.True(result.SequenceEqual(value), $"I just expect: [{string.Join(",", value.Select(x => x.ToString()))}] But your serialized value is: [{string.Join(",", result.Select(x => x.ToString()))}]");
        }

        public void TextEqual(byte[] result, string value)
        {
            var lengthBytes = BitConverter.GetBytes(value.Length);
            var encodeBytes = lengthBytes.Concat(Encoding.UTF8.GetBytes(value)).ToArray();
            Assert.True(result.SequenceEqual(encodeBytes), $"In your text they are not equal '{Encoding.UTF8.GetString(result)}' != '{value}', I just expect: [{string.Join(",", encodeBytes.Select(x => x.ToString()))}] But your serialized value is: [{string.Join(",", result.Select(x => x.ToString()))}]");
        }

        public void ObjectEqual(object obj1, object obj2)
        {
            var method = obj1.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.Name == "IsEquals").FirstOrDefault(x => x.GetParameters()[0].ParameterType == obj2.GetType());
            Assert.True((bool)method.Invoke(obj1, new object[] { obj2 }), $"Objects are not equal '{Newtonsoft.Json.JsonConvert.SerializeObject(obj1)}' \r\n != \r\n '{Newtonsoft.Json.JsonConvert.SerializeObject(obj2)}'");
        }
    }
}
