using JsonGo.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonGo.Deserialize
{
    public class ArrayModel : IJsonGoModel
    {
        public List<IJsonGoModel> Items { get; set; } = new List<IJsonGoModel>();

        public void Add(string nameOrValue, IJsonGoModel value)
        {
            Items.Add(value);
        }

        public object Generate(Type type, Deserializer deserializer)
        {
            object obj = Activator.CreateInstance(type);
            //Type dataType = obj.GetType();
            //if (!TypeGoInfo.Types.TryGetValue(dataType, out TypeGoInfo typeGoInfo))
            //{
            //    TypeGoInfo.Types[dataType] = typeGoInfo = TypeGoInfo.Generate(dataType);
            //}
            if (obj is IDictionary)
            {
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];
                foreach (var item in Items)
                {
                    MethodInfo addMethod = type.GetMethod("Add");
                    if (item is ObjectModel objectModel)
                    {
                        var key = objectModel.Properties["Key"].Generate(keyType, deserializer);
                        var value = objectModel.Properties["Value"].Generate(valueType, deserializer);
                        addMethod.Invoke(obj, new object[] { key, value });
                    }
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    MethodInfo addMethod = type.GetMethod("Add");
                    Array array = null;
                    Type elementType = null;
                    if (type != null && type.IsArray)
                    {
                        array = (Array)obj;
                        elementType = array.GetType().GetElementType();
                    }
                    else if (addMethod != null)
                    {
                        elementType = addMethod.GetParameters().FirstOrDefault().ParameterType;
                    }
                    if (elementType != null)
                    {
                        var value = item.Generate(elementType, deserializer);
                        addMethod.Invoke(obj, new object[] { value });
                    }
                }
            }
            return obj;
        }
    }
}
