using System;

namespace JsonGo.Deserialize
{
    public class ValueModel : IJsonGoModel
    {
        public ValueModel()
        {

        }
        public ValueModel(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public void Add(string nameOrValue, IJsonGoModel value)
        {
            Value = nameOrValue;
        }

        public object Generate(Type type, Deserializer deserializer)
        {
            return deserializer.GetValue(type, Value.Trim('\"'));
        }
    }
}
