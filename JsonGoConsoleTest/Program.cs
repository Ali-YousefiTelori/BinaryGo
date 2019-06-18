using JsonGo;
using JsonGo.DataTypes;
using JsonGo.Deserialize;
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
            try
            {
                Console.WriteLine($"Enter number of items: ");
                int CYCLES = int.Parse(Console.ReadLine());



                decimal JsonGoRes;
                decimal JsonNetRes;

                List<List<Product>> fullProducts = new List<List<Product>>();
                List<Product> products = new List<Product>();
                Product product = new Product()
                {
                    Age = 27,
                    Name = "gl502vm",
                    Profile = new Profile()
                    {
                        FullName = "ali yousefi",
                        Addresses = new List<Address>()
                        {
                            new Address ()
                            {
                                 Content = "آدرس تلور",
                                 CreatedDate = DateTime.Now,
                                 Type = AddressType.Home
                            },
                            new Address ()
                            {
                                 Content = "آدرس مشهد خیابان علی عصر",
                                 CreatedDate = DateTime.Now.AddYears(1),
                                 Type = AddressType.Work
                            }
                        }
                    },
                    IsEnabled = true
                };
                Product product2 = new Product()
                {
                    Age = 22,
                    Name = "asus rog",
                    Profile = new Profile()
                    {
                        FullName = "amin mardani",
                        Addresses = new List<Address>()
                        {
                            new Address ()
                            {
                                 Content = "آدرس نوکنده",
                                 CreatedDate = DateTime.Now,
                                 Type = AddressType.Home
                            },
                            new Address ()
                            {
                                 Content = "آدرس جنگل گلستان",
                                 CreatedDate = DateTime.Now.AddYears(1),
                                 Type = AddressType.Work
                            }
                        }
                    },
                    IsEnabled = false
                };

                products.Add(product);
                products.Add(product2);

                fullProducts.Add(products);
                Serializer serializer = new Serializer();
                string newtonJson = Newtonsoft.Json.JsonConvert.SerializeObject(fullProducts, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Arrays });

                string jsonGoJson = serializer.Serialize(fullProducts);

                //var value222 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<Product>>>(jsonGoJson);

                Console.WriteLine($"***  SERIALIZE {CYCLES} items ***");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();



                // JSONNET serialize
                for (int i = 0; i < CYCLES; i++)
                {
                    string js = Newtonsoft.Json.JsonConvert.SerializeObject(fullProducts, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Arrays });
                }
                stopwatch.Stop();
                JsonNetRes = stopwatch.ElapsedTicks;

                Console.WriteLine("Newtonsoft.Json: \t " + stopwatch.Elapsed);
                stopwatch.Reset();

                stopwatch.Start();



                // MessagePack serialize
                for (int i = 0; i < CYCLES; i++)
                {
                    var data = Serialize(fullProducts);
                }
                stopwatch.Stop();
                //JsonNetRes = stopwatch.ElapsedTicks;

                Console.WriteLine("MessagePack: \t " + stopwatch.Elapsed);
                stopwatch.Reset();

                //JSONGO SERIALIZE
                stopwatch.Start();
                for (int i = 0; i < CYCLES; i++)
                {
                    string js = new Serializer().Serialize(fullProducts);
                }
                stopwatch.Stop();
                JsonGoRes = stopwatch.ElapsedTicks;
                Console.WriteLine("JsonGo: \t\t " + stopwatch.Elapsed);

                if (JsonGoRes > JsonNetRes)
                {
                    decimal tt = JsonGoRes / JsonNetRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X SLOWER than Json.net");
                }
                else
                {
                    decimal tt = JsonNetRes / JsonGoRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X FASTER than Json.net");
                }

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"***  DESERIALIZE {CYCLES} items***");
                //JSONNET deserialize
                stopwatch.Reset();
                stopwatch.Start();

                for (int i = 0; i < CYCLES; i++)
                {
                    var value = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<Product>>>(newtonJson);
                }

                stopwatch.Stop();
                JsonNetRes = stopwatch.ElapsedTicks;

                Console.WriteLine("Newtonsoft.Json: \t" + stopwatch.Elapsed);

                stopwatch.Reset();

                //JSONGO deserialize
                Deserializer deserializer = new Deserializer();

                stopwatch.Start();
                for (int i = 0; i < CYCLES; i++)
                {
                    var js = deserializer.Deserialize<List<List<Product>>>(jsonGoJson);
                }
                stopwatch.Stop();
                JsonGoRes = stopwatch.ElapsedTicks;
                Console.WriteLine("JsonGo: \t\t" + stopwatch.Elapsed);

                if (JsonGoRes > JsonNetRes)
                {
                    decimal tt = JsonGoRes / JsonNetRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X SLOWER than Json.net");
                }
                else
                {
                    decimal tt = JsonNetRes / JsonGoRes;
                    decimal res = Math.Round(tt, 2);
                    Console.WriteLine($"JsonGo is {res}X FASTER than Json.net");
                }
            }
            catch (Exception ex)
            {

            }
            //Product product = new Product()
            //{
            //    Name = "ali yousefi",
            //    Age = 16,
            //    BirthDay = DateTime.Now
            //};

            ////Console.WriteLine("start");

            ////Thread.Sleep(1000);
            ////GC.Collect();
            ////GC.WaitForPendingFinalizers();
            ////GC.Collect();
            ////Console.WriteLine("ok");


            //RunJsonGo(product);
            //RunNewtonJson(product);
            //RunNewtonJson(product);
            //RunJsonGo(product);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press ENTER to retry, or any other key to exit");
            var info = Console.ReadKey();
            if (info.Key == ConsoleKey.Enter)
            {
                var fileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                Environment.Exit(0);
            }
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
