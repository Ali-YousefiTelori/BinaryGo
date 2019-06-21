using JsonGo;
using JsonGo.CodeGenerators;
using JsonGo.Deserialize;
using JsonGoPerformance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JsonGoCoreConsoleTest
{

    public class Product
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Profile Profile { get; set; }
        public List<Address> Addresses { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class Profile
    {
        public string FullName { get; set; }
        public List<Address> Addresses { get; set; }
    }

    public class Address
    {
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public AddressType Type { get; set; }
        public Profile Parent { get; set; }
    }

    public enum AddressType : byte
    {
        None = 0,
        Home = 1,
        Work = 2
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            RunApp();
            Console.ReadLine();
        }

        static void RunApp()
        {
            try
            {
                AssemblyLoader assemblyLoader = new AssemblyLoader();
                assemblyLoader.Add(@"D:\Github\JsonGo\JsonGoCoreConsoleTest\bin\Debug\netcoreapp3.0\JsonGoPerformance.dll");
                var code = assemblyLoader.GenerateCode();
                Console.WriteLine($"Enter 1 for loop reference testing another is normal object testing: ");
                JsonGoModelBuilder.Initialize();
                if (Console.ReadLine() == "1")
                {
                    Console.WriteLine($"Enter number of items: ");
                    int count = int.Parse(Console.ReadLine());
                    Console.WriteLine(@"///////////////////////// SimpleSample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    LoopReferenceSamples.Run(LoopReferenceSamples.GetSimpleSample(), count);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(@"///////////////////////// SimpleArraySample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    LoopReferenceSamples.Run(LoopReferenceSamples.GetSimpleArraySample(), count);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(@"///////////////////////// ComplexObjectSample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    LoopReferenceSamples.Run(LoopReferenceSamples.GetComplexObjectSample(), count);
                }
                else
                {
                    Console.WriteLine($"Enter number of items: ");
                    int count = int.Parse(Console.ReadLine());
                    Console.WriteLine(@"///////////////////////// SimpleSample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    NormalSamples.Run(NormalSamples.GetSimpleSample(), count);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(@"///////////////////////// SimpleArraySample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    NormalSamples.Run(NormalSamples.GetSimpleArraySample(), count);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(@"///////////////////////// ComplexObjectSample \\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    NormalSamples.Run(NormalSamples.GetComplexObjectSample(), count);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press enter to run again");
            Console.ReadKey();
            RunApp();
        }
    }


}
