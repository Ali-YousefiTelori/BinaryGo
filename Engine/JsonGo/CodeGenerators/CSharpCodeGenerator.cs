using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonGo.CodeGenerators
{
    public static class CSharpCodeGenerator
    {
        public static void GenerateCode(StringBuilder stringBuilder, AssemblyLoader assemblyLoader)
        {
            foreach (var assembly in assemblyLoader.Assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    GenerateModel(stringBuilder, type);
                }
            }
            while (NeedToGenerateModels.Count > 0)
            {
                var item = NeedToGenerateModels.First();
                GenerateModel(stringBuilder, item);
                NeedToGenerateModels.Remove(item);
            }
        }
        internal static List<Type> DirectTypes { get; set; } = new List<Type>()
        {
            typeof(DateTime),
            typeof(uint),
            typeof(long),
            typeof(short),
            typeof(byte),
            typeof(double),
            typeof(float),
            typeof(decimal),
            typeof(sbyte),
            typeof(ulong),
            typeof(bool),
            typeof(ushort),
            typeof(int),
            typeof(string)
        };
        internal static List<Type> NeedToGenerateModels { get; set; } = new List<Type>();
        internal static List<Type> SkipToGenerateModels { get; set; } = new List<Type>();

        static void GenerateModel(StringBuilder stringBuilder, Type type)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                GenerateArraryModel(stringBuilder, type);
            }
            else
            {
                GenerateClassModel(stringBuilder, type);
            }
        }
        static void GenerateClassModel(StringBuilder stringBuilder, Type type)
        {
            if (SkipToGenerateModels.Contains(type))
                return;
            SkipToGenerateModels.Add(type);
            var properties = type.GetProperties();
            if (properties.Length == 0)
                return;
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"TypeBuilder<{GetFriendlyName(type)}>.Create().SerializeObject((serializer, builder, obj) =>");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("if (obj == null)");
            stringBuilder.AppendLine("return;");
            stringBuilder.AppendLine("if (serializer.SerializedObjects.TryGetValue(obj, out int index))");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(@"builder.Append(""\""{\""$ref\"":\"""");");
            stringBuilder.AppendLine("builder.Append(index);");
            stringBuilder.AppendLine(@"builder.Append(""\""}\"""");");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("else");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("serializer.ReferencedIndex++;");
            stringBuilder.AppendLine("serializer.SerializedObjects[obj] = serializer.ReferencedIndex;");
            stringBuilder.AppendLine(@"builder.Append(""\""{\""$id\"":\"""");");
            stringBuilder.AppendLine("builder.Append(serializer.ReferencedIndex);");

            foreach (var item in properties)
            {
                if (DirectTypes.Contains(item.PropertyType))
                {
                    stringBuilder.AppendLine($@"builder.Append(""\"",\""{item.Name}\"":\"""");");
                    stringBuilder.AppendLine($"builder.Append(obj.{item.Name});");
                }
                else if (item.PropertyType.IsEnum)
                {
                    stringBuilder.AppendLine($@"builder.Append(""\"",\""{item.Name}\"":\"""");");
                    stringBuilder.AppendLine($"builder.Append((int)obj.{item.Name});");
                }
                else
                {
                    if (!NeedToGenerateModels.Contains(item.PropertyType) && !SkipToGenerateModels.Contains(item.PropertyType))
                        NeedToGenerateModels.Add(item.PropertyType);
                    stringBuilder.AppendLine($@"if (obj.{item.Name} != null)");
                    stringBuilder.AppendLine("{");

                    stringBuilder.AppendLine($@"builder.Append(""\"",\""{item.Name}\"":\"""");");
                    stringBuilder.AppendLine($"serializer.ContinueSerializeCompile(obj.{item.Name});");
                    stringBuilder.AppendLine("}");
                }
            }
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}).Build();");
        }
        static void GenerateArraryModel(StringBuilder stringBuilder, Type type)
        {
            if (SkipToGenerateModels.Contains(type))
                return;
            SkipToGenerateModels.Add(type);
            var properties = type.GetProperties();
            if (properties.Length == 0)
                return;
            stringBuilder.AppendLine($"TypeBuilder<{GetFriendlyName(type)}>.Create().SerializeObject((serializer, builder, obj) =>");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("if (obj == null)");
            stringBuilder.AppendLine("return;");
            stringBuilder.AppendLine("if (serializer.SerializedObjects.TryGetValue(obj, out int index))");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(@"builder.Append(""\""{\""$ref\"":\"""");");
            stringBuilder.AppendLine("builder.Append(index);");
            stringBuilder.AppendLine(@"builder.Append(""\""}\"""");");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("else");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("serializer.ReferencedIndex++;");
            stringBuilder.AppendLine("serializer.SerializedObjects[obj] = serializer.ReferencedIndex;");
            stringBuilder.AppendLine(@"builder.Append(""\""{\""$id\"":\"""");");
            stringBuilder.AppendLine("builder.Append(serializer.ReferencedIndex);");
            stringBuilder.AppendLine(@"builder.Append(""\"",\""$values\"":[\"""");");
            stringBuilder.AppendLine(" foreach (var item in obj)");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("if (item == null)");
            stringBuilder.AppendLine("continue;");
            stringBuilder.AppendLine("serializer.ContinueSerializeCompile(item);");
            stringBuilder.AppendLine("builder.Append(',');");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("serializer.RemoveLastCama();");
            stringBuilder.AppendLine("builder.AppendLine(\"]}\");");
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("}).Build();");
        }

        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof(int))
                return "int";
            else if (type == typeof(short))
                return "short";
            else if (type == typeof(byte))
                return "byte";
            else if (type == typeof(bool))
                return "bool";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(float))
                return "float";
            else if (type == typeof(double))
                return "double";
            else if (type == typeof(decimal))
                return "decimal";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(Task))
            {
                return "void";
            }
            else if (type.BaseType == typeof(Task))
            {
                return GetFriendlyName(type.GetGenericArguments()[0]);
            }
            else if (type.GetGenericArguments().Length > 0)
                return type.Namespace + "." + type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray()) + ">";
            else
                return type.Namespace + "." + type.Name;
        }
    }
}
