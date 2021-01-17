using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BinaryGo.CodeGenerators
{
    /// <summary>
    /// Load assemblies to generate compile time codes
    /// </summary>
    public class AssemblyLoader
    {
        internal List<Assembly> Assemblies { get; set; } = new List<Assembly>();
        /// <summary>
        /// Add file name
        /// </summary>
        /// <param name="fileName"></param>
        public void Add(string fileName)
        {
            Assemblies.Add(Assembly.LoadFile(fileName));
        }
        /// <summary>
        /// Generates code from assembly
        /// </summary>
        /// <returns></returns>
        public string GenerateCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            CSharpCodeGenerator.GenerateCode(stringBuilder, this);
            return stringBuilder.ToString();
        }
    }
}
