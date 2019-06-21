using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JsonGo.CodeGenerators
{
    public class AssemblyLoader
    {
        internal List<Assembly> Assemblies { get; set; } = new List<Assembly>();
        public void Add(string fileName)
        {
            Assemblies.Add(Assembly.LoadFile(fileName));
        }

        public string GenerateCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            CSharpCodeGenerator.GenerateCode(stringBuilder, this);
            return stringBuilder.ToString();
        }
    }
}
