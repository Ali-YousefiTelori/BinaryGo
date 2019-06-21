using JsonGo;
using JsonGo.DataTypes;
using JsonGo.Deserialize;
using JsonGoPerformance;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonGoConsoleTest
{


    [MessagePackObject]
    public class Product
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public int Age { get; set; }
        [Key(2)]
        public Profile Profile { get; set; }
        [Key(3)]
        public List<Address> Addresses { get; set; }
        [Key(4)]
        public bool IsEnabled { get; set; }
    }

    [MessagePackObject]
    public class Profile
    {
        [Key(0)]
        public string FullName { get; set; }
        [Key(1)]
        public List<Address> Addresses { get; set; }
    }

    [MessagePackObject]
    public class Address
    {
        [Key(0)]
        public string Content { get; set; }
        [JsonIgnore]
        [Ignore]
        [Key(1)]
        public DateTime CreatedDate { get; set; }
        [Key(2)]
        public AddressType Type { get; set; }
        [Key(3)]
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

        static byte[] Serialize<T>(T thisObj)
        {
            return MessagePackSerializer.Serialize<T>(thisObj);
        }

        static T Deserialize<T>(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

    }
}
