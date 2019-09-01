using BenchmarkDotNet.Running;
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
                Console.WriteLine("Select Option:");
                Console.WriteLine("1) Normal Serialize Samples");
                Console.WriteLine("2) Loop Reference Samples");
                Console.WriteLine("3) Normal Deserialize Samples");

                var read = Console.ReadLine();
                if (read == "1")
                    BenchmarkRunner.Run<NormalSerializeSamples>();
                else if (read == "2")
                    BenchmarkRunner.Run<LoopReferenceSamples>();
                else if (read == "3")
                    BenchmarkRunner.Run<NormalDeserializeSamples>();
                else
                    Console.WriteLine($"not support {read}");
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
