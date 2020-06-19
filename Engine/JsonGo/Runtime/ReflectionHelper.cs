using JsonGo.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace JsonGo.Runtime
{
    /// <summary>
    /// Help you to easy of use runtime reflection methods
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// create a delegate from a method
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static TDelegate CreateDelegate<TDelegate>(MethodInfo methodInfo) where TDelegate : Delegate
        {
            return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), methodInfo);
        }

        /// <summary>
        /// Generates types interface to new types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static Type GenerateTypeFromInterface(Type type, ITypeGo options)
        {
            Type[] genericTypes = null;
            if (type.GenericTypeArguments.Length > 0)
            {
                genericTypes = type.GenericTypeArguments;
                type = type.GetGenericTypeDefinition();
            }

            if (options.CustomTypeChanges.TryGetValue(type, out Type newType))
            {
                type = newType;
            }

            if (genericTypes != null)
            {
                for (int i = 0; i < genericTypes.Length; i++)
                {
                    genericTypes[i] = GenerateTypeFromInterface(genericTypes[i], options);
                }
                type = type.MakeGenericType(genericTypes);
            }
            return type;
        }

        /// <summary>
        /// Gets list of properties of type, with inheritance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetListOfProperties(Type type)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (var property in type.GetProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0 || !property.CanRead || !property.CanWrite || property.GetIndexParameters().Length > 0)
                    continue;
                properties.Add(property);
            }
            //properties.AddRange(GetListOfProperties(type.BaseType));
            return properties;
        }

        /// <summary>
        /// Gets type delegate to create instance in an efficient way
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static PropertyCallerInfo<TObjectType, TPropertyType> GetDelegateInstance<TObjectType, TPropertyType>(PropertyInfo propertyInfo)
        {
            Type type = typeof(TObjectType);
            var openGetterType = typeof(Func<,>);
            var concreteGetterType = openGetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            var openSetterType = typeof(Action<,>);
            var concreteSetterType = openSetterType
                .MakeGenericType(type, propertyInfo.PropertyType);

            Delegate getterInvocation = Delegate.CreateDelegate(concreteGetterType, null, propertyInfo.GetGetMethod());
            Delegate setterInvocation = Delegate.CreateDelegate(concreteSetterType, null, propertyInfo.GetSetMethod());

            var callerType = typeof(PropertyCallerInfo<,>);
            var callerGenericType = callerType
                .MakeGenericType(type, propertyInfo.PropertyType);

            return (PropertyCallerInfo<TObjectType, TPropertyType>)Activator.CreateInstance(callerGenericType, getterInvocation, setterInvocation);
        }

        static T[] GetArray<T>(IEnumerable<T> iList)
        {
            var result = new T[iList.Count()];

            Array.Copy(iList.ToArray(), 0, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// object create instance
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<object> GetActivator(Type type)
        {
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            if (emptyConstructor == null)
            {
                return new Func<object>(() => null);
            }
            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(emptyConstructor);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(Func<object>), newExp);

            //compile it
            Func<object> compiled = (Func<object>)lambda.Compile();
            return compiled;
        }

        //public static Func<object> GetActivator(Type type)
        //{
        //    ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
        //    if (emptyConstructor == null)
        //    {
        //        return new Func<object>(() => null);
        //    }
        //    var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
        //    ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
        //    ilGenerator.Emit(OpCodes.Nop);
        //    ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
        //    ilGenerator.Emit(OpCodes.Ret);
        //    return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        //}

        /// <summary>
        /// set value of property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance).SetValue(instance, value);
        }
    }
}
