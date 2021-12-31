using BinaryGo.Helpers;
using BinaryGo.Runtime;
using BinaryGo.Runtime.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryGo.Binary.StructureModels
{
    /// <summary>
    /// model structure of binaryGo that wil help the client to update themself to new changes and versioning of server side models breaking changes
    /// do not change the property position please that will be a big brak changes for updates
    /// </summary>
    public class BinaryModelInfo
    {
        /// <summary>
        /// name of model
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// namespace of model
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// assembly name of model
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// binary go structre name
        /// like: object
        /// like int32
        /// like enum as int32
        /// </summary>
        public string BinaryName { get; set; }
        /// <summary>
        /// list of properties
        /// </summary>
        public List<MemberBinaryModelInfo> Properties { get; set; }
        /// <summary>
        /// list of generics
        /// </summary>
        public List<BinaryModelInfo> Generics { get; set; }

        /// <summary>
        /// Get binary model of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeGoInfo"></param>
        /// <param name="option"></param>
        /// <param name="generatedModels"></param>
        /// <returns></returns>
        internal static BinaryModelInfo GetBinaryModel<T>(TypeGoInfo<T> typeGoInfo, BaseOptionInfo option, Dictionary<Type, BinaryModelInfo> generatedModels)
        {
            Type type = typeGoInfo.Type;
            if (generatedModels.TryGetValue(type, out BinaryModelInfo binaryModel))
                return binaryModel;
            binaryModel = new BinaryModelInfo
            {
                Name = type.Name,
                Namespace = type.Namespace,
                AssemblyName = System.IO.Path.GetFileName(type.Assembly.Location),
                Properties = new List<MemberBinaryModelInfo>(),
                Generics = new List<BinaryModelInfo>()
            };

            generatedModels.Add(type, binaryModel);
            foreach (Type genericType in type.GetGenericArguments())
            {
                binaryModel.Generics.Add(GetBinaryModel(genericType, option, generatedModels));
            }

            if (typeGoInfo.Variable is ObjectVariable<T> objectVariable)
            {
                foreach (BasePropertyGoInfo<T> property in objectVariable.Properties)
                {
                    MemberBinaryModelInfo propertyResult = property.GetBinaryMember(option, generatedModels);
                    propertyResult.Index = property.Index;
                    binaryModel.Properties.Add(propertyResult);
                }
            }
            

            return binaryModel;
        }



        internal static BinaryModelInfo GetBinaryModel(Type type, BaseOptionInfo option, Dictionary<Type, BinaryModelInfo> generatedModels)
        {
            System.Reflection.MethodInfo getBinaryModel = typeof(BinaryModelInfo).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                .FirstOrDefault(x => x.Name == nameof(BinaryModelInfo.GetBinaryModel)).MakeGenericMethod(type);
            return (BinaryModelInfo)getBinaryModel.Invoke(null, new object[] { GetTypeGo(type, option), option, generatedModels });
        }

        internal static BaseTypeGoInfo GetTypeGo(Type type, BaseOptionInfo option)
        {
            System.Reflection.MethodInfo getBinaryModel = typeof(BaseTypeGoInfo).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                   .FirstOrDefault(x => x.Name == nameof(BaseTypeGoInfo.Generate)).MakeGenericMethod(type);
            object typeGo = getBinaryModel.Invoke(null, new object[] { option });
            return (BaseTypeGoInfo)typeGo;
        }

        /// <summary>
        /// get full name of model
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{AssemblyName} {Namespace}.{Name}";
        }

        /// <summary>
        /// get full Namespace + name of model
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return $"{Namespace}.{Name}";
        }
    }
}
