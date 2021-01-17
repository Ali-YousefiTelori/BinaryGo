using BenchmarkDotNet.Running;
using BinaryGo;
using BinaryGo.Binary;
using BinaryGo.CodeGenerators;
using BinaryGo.Json.Deserialize;
using BinaryGo.Runtime;
using BinaryGoPerformance;
using BinaryGoPerformance.Models;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BinaryGoCoreConsoleTest
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
                NormalSerializeSamples normalSerializeSamples = new NormalSerializeSamples();
                //normalSerializeSamples.InitBinaryGo();
                normalSerializeSamples.RunComplex_Binary_BinaryGo();
                normalSerializeSamples.RunComplex_Binary_MessagePack();
                normalSerializeSamples.RunComplex_Binary_ZeroFormatter();
                normalSerializeSamples.RunComplex_BinaryGo_JsonBinary();
                normalSerializeSamples.RunComplex_Json_BinaryGo();
                normalSerializeSamples.RunComplex_Json_JsonNet();
                normalSerializeSamples.RunComplex_Json_TextJson();
                normalSerializeSamples.RunComplex_Json_UTF8Json();
                //var bytesaaa = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(normalSerializeSamples.GetSimpleSample()).ToArray();
                //var deserialized = BinaryGo.Binary.Deserialize.BinaryDeserializer.NormalInstance.Deserialize<BinaryGoPerformance.Models.SimpleUserInfo>(bytesaaa);
                //var staticdata = GetText(bytesaaa);
                //var data1 = MessagePackSerializer.Serialize(normalSerializeSamples.GetSimpleSample());
                //BinarySerializer serializer = new BinarySerializer();
                //var data2 = serializer.Serialize(normalSerializeSamples.GetSimpleSample()).ToArray();

                //var data = normalSerializeSamples.GetSimpleSample();
                //var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(data);
                //var bytes = result.ToArray();
                //var text = string.Join(Environment.NewLine, TypeGoInfo.Generate(typeof(Profile)).SerializeProperties.Select(x => $"name: {x.Name} , type: {x.Type.FullName}"));
                Console.WriteLine("Select Option:");
                Console.WriteLine("1) Normal Serialize Samples");
                Console.WriteLine("2) Loop Reference Samples");
                Console.WriteLine("3) Normal Deserialize Samples");

                Console.WriteLine("Select Manual Option:");
                Console.WriteLine("4) Simple Normal Serialize Samples");
                Console.WriteLine("5) Complex Normal Serialize Samples");
                Console.WriteLine("6) Array Normal Serialize Samples");
                Console.WriteLine("7) Simple Loop Reference Samples");
                Console.WriteLine("8) Complex Loop Reference Samples");
                Console.WriteLine("9) Array Loop Reference Samples");
                Console.WriteLine("10) Normal Deserialize Samples");

                var read = Console.ReadLine();
                if (read == "1")
                    BenchmarkRunner.Run<NormalSerializeSamples>();
                else if (read == "2")
                    BenchmarkRunner.Run<LoopReferenceSamples>();
                else if (read == "3")
                    BenchmarkRunner.Run<NormalDeserializeSamples>();
                else if (read == "4")
                {
                    NormalSerializeSamples.RunSimple();
                }
                else if (read == "5")
                {
                    NormalSerializeSamples.RunComplex();
                }
                else if (read == "6")
                {
                    NormalSerializeSamples.RunArray();
                }
                else if (read == "7")
                {
                    LoopReferenceSamples.RunSimple();
                }
                else if (read == "8")
                {
                    LoopReferenceSamples.RunComplex();
                }
                else if (read == "9")
                {
                    LoopReferenceSamples.RunArray();
                }
                else if (read == "10")
                    new NormalDeserializeSamples().RunDeserialize(100000);
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
        public static string GetText(Span<byte> bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append($"{bytes[i]},");
            }
            return builder.ToString();
        }
    }


}
