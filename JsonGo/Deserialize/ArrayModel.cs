using System;
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
            foreach (var item in Items)
            {
                MethodInfo addMethod = type == null ? null : deserializer.FindCachedMember<MethodInfo>(type, "Add");
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
            return obj;
        }
    }
}
