using JsonGo;
using JsonGo.Deserialize;
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
            try
            {
                Console.WriteLine($"Enter number of items: ");
                Serializer serializer = new Serializer();
         
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

                foreach (Address item in product.Profile.Addresses)
                {
                    item.Parent = product.Profile;
                }
                product.Profile.Addresses.AddRange(product.Profile.Addresses);
                product.Addresses = product.Profile.Addresses;
                products.Add(product);
                products.Add(product);
                products.Add(product2);

                fullProducts.Add(products);
                fullProducts.Add(products);
                string newtonJson = Newtonsoft.Json.JsonConvert.SerializeObject(fullProducts, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Arrays });

                string jsonGoJson = serializer.Serialize(fullProducts);


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

                //JSONGO SERIALIZE
                stopwatch.Start();
                for (int i = 0; i < CYCLES; i++)
                {
                    string js = serializer.Serialize(fullProducts);
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
    }


}
