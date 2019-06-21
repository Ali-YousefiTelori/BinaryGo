using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JsonGo.Deserialize
{
    public class ObjectModel : IJsonGoModel
    {
        public Dictionary<string, IJsonGoModel> Properties { get; set; } = new Dictionary<string, IJsonGoModel>();

        public void Add(string nameOrValue, IJsonGoModel value)
        {
            Properties.Add(nameOrValue, value);
        }

        public object Generate(Type type, Deserializer deserializer)
        {
            string idName = JsonSettingInfo.IdRefrencedTypeNameNoQuotes;
            string referenceName = JsonSettingInfo.RefRefrencedTypeNameNoQuotes;
            string valuesName = JsonSettingInfo.ValuesRefrencedTypeNameNoQuotes;
            if (Properties.ContainsKey(referenceName))
            {
                return deserializer.DeSerializedObjects[(string)Properties[referenceName].Generate(typeof(string), deserializer)];
            }
            else if (Properties.ContainsKey(valuesName))
            {
                object obj = Properties[valuesName].Generate(type, deserializer);
                foreach (KeyValuePair<string, IJsonGoModel> item in Properties)
                {
                    if (item.Key == idName)
                    {
                        string value = (string)item.Value.Generate(typeof(string), deserializer);
                        deserializer.DeSerializedObjects.Add(value, obj);
                        break;
                    }
                }
                return obj;
            }
            else
            {
                object obj = Activator.CreateInstance(type);
                if (obj is IDictionary)
                {
                    Type keyType = type.GetGenericArguments()[0];
                    Type valueType = type.GetGenericArguments()[1];
                    MethodInfo addMethod = type.GetMethod("Add");
                    foreach (KeyValuePair<string, IJsonGoModel> item in Properties)
                    {
                        var key = Convert.ChangeType(item.Key, keyType);
                        var value = item.Value.Generate(valueType, deserializer);
                        addMethod.Invoke(obj, new object[] { key, value });
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, IJsonGoModel> item in Properties)
                    {
                        if (item.Key == idName)
                        {
                            string value = (string)item.Value.Generate(typeof(string), deserializer);
                            deserializer.DeSerializedObjects.Add(value, obj);
                        }
                        else
                        {
                            Type keyType = deserializer.GetKeyType(obj, item.Key);
                            if (keyType != null)
                            {
                                object value = item.Value.Generate(keyType, deserializer);
                                deserializer.SetValue(obj, value, item.Key);
                            }
                        }
                    }
                }
                return obj;
            }
        }
    }
}
