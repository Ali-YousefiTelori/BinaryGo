using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Helpers;
using BinaryGo.Runtime;
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

        public void ServerModelTestDeserializeBase<TServerModel, TClientModelOld>(byte[] Result, TServerModel Value, BaseOptionInfo SerializerOptions, Action<TClientModelOld> intializeClientTest, params (Type ServerType,Type ClientType)[] MovedTypes)
        {
            //in this example server side has TServerModel
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
            myDeserializer.Options.GenerateType<TClientModelOld>();
            BaseTypeGoInfo.GenerateDefaultVariables(myDeserializer.Options);
            //add model renamed
            myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(typeof(TServerModel)), typeof(TClientModelOld));
            if (MovedTypes?.Length > 0)
            {
                foreach (var movedType in MovedTypes)
                {
                    //add model renamed
                    myDeserializer.AddMovedType(myDeserializer.GetStrcutureModelName(movedType.ServerType), movedType.ClientType);
                }
            }
            //build new structure to old structure
            myDeserializer.BuildStructure(newStructureModels);
            #endregion

            var result = myDeserializer.Deserialize<TClientModelOld>(Result);
            ObjectEqual(result, Value);
            //now serialize from client side and deserialize from server side happen
            intializeClientTest(result);
            BinarySerializer binarySerializer = new BinarySerializer(myDeserializer.Options);
            var resultSerialized = binarySerializer.Serialize(result);
            var resultDeserialized = myDeserializer.Deserialize<TClientModelOld>(resultSerialized);
            ObjectEqual(resultDeserialized, Value);
        }
    }
}
