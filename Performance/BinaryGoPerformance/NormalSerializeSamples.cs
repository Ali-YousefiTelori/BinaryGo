using BenchmarkDotNet.Attributes;
using BinaryGo;
using BinaryGo.Binary;
using BinaryGo.Binary.Deserialize;
using BinaryGo.Json;
using BinaryGo.Runtime;
using BinaryGoPerformance.Models;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeroFormatter;

namespace BinaryGoPerformance
{
    [MemoryDiagnoser]
    public class NormalSerializeSamples
    {
        static byte[] MessagePackBinaryBytes = new byte[] { 148, 1, 178, 65, 108, 105, 32, 89, 111, 117, 115, 101, 102, 105, 32, 84, 101, 108, 111, 114, 105, 28, 215, 255, 200, 183, 30, 32, 94, 215, 210, 47 };
        static byte[] BinaryGoBinaryBytes = new byte[] { 1, 0, 0, 0, 18, 0, 0, 0, 65, 108, 105, 32, 89, 111, 117, 115, 101, 102, 105, 32, 84, 101, 108, 111, 114, 105, 28, 0, 0, 0, 92, 159, 66, 175, 2, 8, 216, 8 };
        public static void InitializeChaches<T>(T obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Serializer.NormalInstance.Serialize(obj);
                //Serializer.SingleIntance.SerializeCompile(obj);
                JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                System.Text.Json.JsonSerializer.Serialize(obj);
            }
        }
        public SimpleUserInfo GetSimpleSample()
        {
            SimpleUserInfo userInfo = new SimpleUserInfo()
            {
                Age = 28,
                CreatedDate = DateTime.Now,
                FullName = "Ali Yousefi Telori",
                Id = 1,
            };
            return userInfo;
        }
        public List<UserInfo> GetSimpleArraySample()
        {
            List<UserInfo> result = new List<UserInfo>();
            for (int i = 1; i < 50; i++)
            {
                UserInfo user = new UserInfo()
                {
                    Age = 28 + i,
                    CreatedDate = DateTime.Now.AddMinutes(i),
                    FullName = "Ali Yousefi Telori " + i,
                    Id = i
                };
                result.Add(user);
            }

            return result;
        }
        public List<RoleInfo> GetArrayRoles()
        {
            List<RoleInfo> result = new List<RoleInfo>();
            RoleInfo roleInfo = new RoleInfo()
            {
                Id = 1,
                Type = RoleType.Admin
            };
            result.Add(roleInfo);
            RoleInfo roleInfo2 = new RoleInfo()
            {
                Id = 2,
                Type = RoleType.Normal
            };
            result.Add(roleInfo2);
            RoleInfo roleInfo3 = new RoleInfo()
            {
                Id = 3,
                Type = RoleType.Viewer
            };
            result.Add(roleInfo3);
            return result;
        }
        public List<ProductInfo> GetArrayProducts()
        {
            List<ProductInfo> result = new List<ProductInfo>();
            for (int i = 1; i < 20; i++)
            {
                ProductInfo product = new ProductInfo()
                {
                    Id = i,
                    Name = "product" + i,
                    CreatedDate = DateTime.Now
                };
                result.Add(product);
            }

            return result;
        }
        public List<CarInfo> GetArrayCars()
        {
            List<CarInfo> result = new List<CarInfo>();
            for (int i = 1; i < 50; i++)
            {
                CarInfo car = new CarInfo()
                {
                    Id = i,
                    Name = "car" + i,
                };
                result.Add(car);
            }

            return result;
        }
        public CompanyInfo GetComplexObjectSample()
        {
            CompanyInfo companyInfo = new CompanyInfo
            {
                Users = GetSimpleArraySample(),
                Name = "company",
                Id = 1,
                Cars = GetArrayCars()
            };
            foreach (var item in companyInfo.Users)
            {
                item.Roles = GetArrayRoles();
                item.Products = GetArrayProducts();
            }
            return companyInfo;
        }


        //public void Run()
        //{
        //    RunSample(GetSimpleSample(), 1);
        //    RunSample(GetSimpleArraySample(), 1);
        //    RunSample(GetComplexObjectSample(), 1);
        //}

        //[Benchmark]
        //public void Run<T>(T sample, int count)
        //{
        //    InitializeChaches(sample);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        RunSample(sample, count);
        //    }

        //}
        [GlobalSetup]
        public void Initialize()
        {
            Console.WriteLine("initializer runned");
            //compile time BinaryGo
            BinaryGoModelBuilder.Initialize();
            //InitBinaryGo();
            NormalSerializeSamples normalSamples = new NormalSerializeSamples();
            NormalSerializeSamples.InitializeChaches(normalSamples.GetSimpleSample());
            NormalSerializeSamples.InitializeChaches(normalSamples.GetSimpleArraySample());
            NormalSerializeSamples.InitializeChaches(normalSamples.GetComplexObjectSample());
        }
        //[Benchmark]
        public void RunSimpleSampleCompileTimeBinaryGo()
        {
            Serializer serializer = new Serializer();
            //serializer.SerializeCompile(GetSimpleSample());
        }

        [Benchmark]
        public void RunSimple_Binary_MessagePack()
        {
            MessagePackSerializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_Binary_MessagePack()
        {
            MessagePackSerializer.Serialize(GetComplexObjectSample());
        }

        [Benchmark]
        public void RunSimple_Binary_ZeroFormatter()
        {
            ZeroFormatterSerializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_Binary_ZeroFormatter()
        {
            ZeroFormatterSerializer.Serialize(GetComplexObjectSample());
        }

        //[Benchmark]
        public void RunSimple_Binary_Deserialize_MessagePack()
        {
            MessagePackSerializer.Deserialize<SimpleUserInfo>(MessagePackBinaryBytes);
        }


        static BinarySerializer _BinaryGo_Binary_serializer = new BinarySerializer();
        static BinaryDeserializer _BinaryGo_Binary_Deserializer = new BinaryDeserializer();

        [Benchmark]
        public void RunSimple_Binary_BinaryGo()
        {
            _BinaryGo_Binary_serializer.Serialize(GetSimpleSample());
        }

        //public void InitBinaryGo()
        //{
        //    InitComapny();
        //    InitUser();
        //    InitProduct();
        //    InitRole();
        //    InitCar();
        //}

        //static void InitRole()
        //{
        //    TypeGoInfo<RoleInfo> typeGoInfo;
        //    Type dataType = typeof(RoleInfo);

        //    if (!_BinaryGo_Binary_serializer.TryGetValueOfTypeGo(dataType, out object typeGo))
        //        typeGoInfo = BaseTypeGoInfo.Generate<RoleInfo>(BinarySerializer.DefaultOptions);
        //    else
        //        typeGoInfo = (TypeGoInfo<RoleInfo>)typeGo;

        //    var id = (PropertyGoInfo<RoleInfo, int>)typeGoInfo.Properties[nameof(RoleInfo.Id)];
        //    id.GetValue = x => x.Id;
        //    id.SetValue = (x, v) => x.Id = v;

        //    var type = (PropertyGoInfo<RoleInfo, RoleType>)typeGoInfo.Properties[nameof(RoleInfo.Type)];
        //    type.GetValue = x => x.Type;
        //    type.SetValue = (x, v) => x.Type = v;
        //}

        //static void InitProduct()
        //{
        //    TypeGoInfo<ProductInfo> typeGoInfo;
        //    Type dataType = typeof(ProductInfo);

        //    if (!_BinaryGo_Binary_serializer.TryGetValueOfTypeGo(dataType, out object typeGo))
        //        typeGoInfo = BaseTypeGoInfo.Generate<ProductInfo>(BinarySerializer.DefaultOptions);
        //    else
        //        typeGoInfo = (TypeGoInfo<ProductInfo>)typeGo;

        //    var id = (PropertyGoInfo<ProductInfo, int>)typeGoInfo.Properties[nameof(ProductInfo.Id)];
        //    id.GetValue = x => x.Id;
        //    id.SetValue = (x, v) => x.Id = v;

        //    var name = (PropertyGoInfo<ProductInfo, string>)typeGoInfo.Properties[nameof(ProductInfo.Name)];
        //    name.GetValue = x => x.Name;
        //    name.SetValue = (x, v) => x.Name = v;

        //    var createDate = (PropertyGoInfo<ProductInfo, DateTime>)typeGoInfo.Properties[nameof(ProductInfo.CreatedDate)];
        //    createDate.GetValue = x => x.CreatedDate;
        //    createDate.SetValue = (x, v) => x.CreatedDate = v;
        //}


        //static void InitUser()
        //{
        //    TypeGoInfo<UserInfo> typeGoInfo;
        //    Type dataType = typeof(UserInfo);

        //    if (!_BinaryGo_Binary_serializer.TryGetValueOfTypeGo(dataType, out object typeGo))
        //        typeGoInfo = BaseTypeGoInfo.Generate<UserInfo>(BinarySerializer.DefaultOptions);
        //    else
        //        typeGoInfo = (TypeGoInfo<UserInfo>)typeGo;

        //    var id = (PropertyGoInfo<UserInfo, int>)typeGoInfo.Properties[nameof(UserInfo.Id)];
        //    id.GetValue = x => x.Id;
        //    id.SetValue = (x, v) => x.Id = v;

        //    var age = (PropertyGoInfo<UserInfo, int>)typeGoInfo.Properties[nameof(UserInfo.Age)];
        //    age.GetValue = x => x.Age;
        //    age.SetValue = (x, v) => x.Age = v;

        //    var createDate = (PropertyGoInfo<UserInfo,DateTime>)typeGoInfo.Properties[nameof(UserInfo.CreatedDate)];
        //    createDate.GetValue = x => x.CreatedDate;
        //    createDate.SetValue = (x, v) => x.CreatedDate = v;

        //    var fullname = (PropertyGoInfo<UserInfo, string>)typeGoInfo.Properties[nameof(UserInfo.FullName)];
        //    fullname.GetValue = x => x.FullName;
        //    fullname.SetValue = (x, v) => x.FullName = v;

        //    var products = (PropertyGoInfo<UserInfo, List<ProductInfo>>)typeGoInfo.Properties[nameof(UserInfo.Products)];
        //    products.GetValue = x => x.Products;
        //    products.SetValue = (x, v) => x.Products = v;

        //    var roles = (PropertyGoInfo<UserInfo, List<RoleInfo>>)typeGoInfo.Properties[nameof(UserInfo.Roles)];
        //    roles.GetValue = x => x.Roles;
        //    roles.SetValue = (x, v) => x.Roles = v;
        //}


        //static void InitCar()
        //{
        //    TypeGoInfo<CarInfo> typeGoInfo;
        //    Type dataType = typeof(CarInfo);

        //    if (!_BinaryGo_Binary_serializer.TryGetValueOfTypeGo(dataType, out object typeGo))
        //        typeGoInfo = BaseTypeGoInfo.Generate<CarInfo>(BinarySerializer.DefaultOptions);
        //    else
        //        typeGoInfo = (TypeGoInfo<CarInfo>)typeGo;

        //    var id = (PropertyGoInfo<CarInfo, int>)typeGoInfo.Properties[nameof(CarInfo.Id)];
        //    id.GetValue = x => x.Id;
        //    id.SetValue = (x, v) => x.Id = v;

        //    var name = (PropertyGoInfo<CarInfo, string>)typeGoInfo.Properties[nameof(CarInfo.Name)];
        //    name.GetValue = x => x.Name;
        //    name.SetValue = (x, v) => x.Name = v;
        //}

        //static void InitComapny()
        //{
        //    TypeGoInfo<CompanyInfo> typeGoInfo;
        //    Type dataType = typeof(CompanyInfo);

        //    if (!_BinaryGo_Binary_serializer.TryGetValueOfTypeGo(dataType, out object typeGo))
        //        typeGoInfo = BaseTypeGoInfo.Generate<CompanyInfo>(BinarySerializer.DefaultOptions);
        //    else
        //        typeGoInfo = (TypeGoInfo<CompanyInfo>)typeGo;

        //    var id = (PropertyGoInfo<CompanyInfo, int>)typeGoInfo.Properties[nameof(CompanyInfo.Id)];
        //    id.GetValue = x => x.Id;
        //    id.SetValue = (x, v) => x.Id = v;

        //    var name = (PropertyGoInfo<CompanyInfo, string>)typeGoInfo.Properties[nameof(CompanyInfo.Name)];
        //    name.GetValue = x => x.Name;
        //    name.SetValue = (x, v) => x.Name = v;

        //    var users = (PropertyGoInfo<CompanyInfo, List<UserInfo>>)typeGoInfo.Properties[nameof(CompanyInfo.Users)];
        //    users.GetValue = x => x.Users;
        //    users.SetValue = (x, v) => x.Users = v;

        //    var cars = (PropertyGoInfo<CompanyInfo, List<CarInfo>>)typeGoInfo.Properties[nameof(CompanyInfo.Cars)];
        //    cars.GetValue = x => x.Cars;
        //    cars.SetValue = (x, v) => x.Cars = v;
        //}

        [Benchmark]
        public void RunComplex_Binary_BinaryGo()
        {
            _BinaryGo_Binary_serializer.Serialize(GetComplexObjectSample());
        }

        //[Benchmark]
        public void RunSimple_Binary_Deserialize_BinaryGo()
        {
            BinaryDeserializer deserializer = new BinaryDeserializer();
            deserializer.Deserialize<SimpleUserInfo>(BinaryGoBinaryBytes);
        }

        static Serializer _BinaryGo_serializer = new Serializer();

        [Benchmark]
        public void RunSimple_Json_BinaryGo()
        {
            _BinaryGo_serializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_Json_BinaryGo()
        {
            _BinaryGo_serializer.Serialize(GetComplexObjectSample());
        }

        [Benchmark]
        public void RunSimple_BinaryGo_JsonBinary()
        {
            Serializer serializer = new Serializer();
            serializer.SerializeToBytes(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_BinaryGo_JsonBinary()
        {
            Serializer serializer = new Serializer();
            serializer.SerializeToBytes(GetComplexObjectSample());
        }

        [Benchmark]
        public void RunSimple_Json_JsonNet()
        {
            JsonConvert.SerializeObject(GetSimpleSample(), new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        [Benchmark]
        public void RunComplex_Json_JsonNet()
        {
            JsonConvert.SerializeObject(GetComplexObjectSample(), new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        [Benchmark]
        public void RunSimple_Json_TextJson()
        {
            System.Text.Json.JsonSerializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_Json_TextJson()
        {
            System.Text.Json.JsonSerializer.Serialize(GetComplexObjectSample());
        }

        [Benchmark]
        public void RunSimple_Json_UTF8Json()
        {
            Utf8Json.JsonSerializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunComplex_Json_UTF8Json()
        {
            Utf8Json.JsonSerializer.Serialize(GetComplexObjectSample());
        }

        public static void RunSimple()
        {
            NormalSerializeSamples normalSamples = new NormalSerializeSamples();
            normalSamples.Initialize();
            RunSample(normalSamples.GetSimpleSample(), 1000);
        }
        public static void RunComplex()
        {
            NormalSerializeSamples normalSamples = new NormalSerializeSamples();
            normalSamples.Initialize();
            RunSample(normalSamples.GetComplexObjectSample(), 1000);
        }
        public static void RunArray()
        {
            NormalSerializeSamples normalSamples = new NormalSerializeSamples();
            normalSamples.Initialize();
            RunSample(normalSamples.GetComplexObjectSample(), 1000);
        }
        private static void RunSample<T>(T sample, int count)
        {
            Console.WriteLine("******* Newtonsoft.JsonNET *****");
            Console.WriteLine($"Count {count}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                JsonConvert.SerializeObject(sample, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                //JsonConvert.SerializeObject(sample, new JsonSerializerSettings()
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //    PreserveReferencesHandling = PreserveReferencesHandling.Arrays
                //});
            }
            stopwatch.Stop();
            double JsonNetRes = stopwatch.ElapsedTicks;
            Console.WriteLine("Newtonsoft.JsonNET: \t " + stopwatch.Elapsed);

            Console.WriteLine("******* System.Text.Json *****");

            Console.WriteLine($"Count {count}");

            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                System.Text.Json.JsonSerializer.Serialize(sample);
            }
            stopwatch.Stop();
            double MicrosoftJsonRes = stopwatch.ElapsedTicks;

            Console.WriteLine("System.Text.Json: \t " + stopwatch.Elapsed);

            Serializer serializer = new Serializer();

            Console.WriteLine("******* BinaryGo *****");
            Console.WriteLine($"Count {count}");
            serializer.Setting.HasGenerateRefrencedTypes = false;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                serializer.Serialize(sample);
            }
            stopwatch.Stop();
            double BinaryGoRes = stopwatch.ElapsedTicks;

            Console.WriteLine("BinaryGo: \t " + stopwatch.Elapsed);
            Console.WriteLine("System.Text.Json: \t " + stopwatch.Elapsed);


            Console.WriteLine("******* BinaryGo Compile Time *****");
            Console.WriteLine($"Count {count}");
            //stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < count; i++)
            //{
            //    serializer.SerializeCompile(sample);
            //}
            //stopwatch.Stop();

            //Console.WriteLine("BinaryGo Compile Time: \t " + stopwatch.Elapsed);
            //Console.WriteLine("BinaryGo Compile Time: \t " + Math.Round(JsonNetRes / stopwatch.ElapsedTicks, 2) + "X FASTER than JsonNET");
            //Console.WriteLine("BinaryGo Compile Time: \t " + Math.Round(MicrosoftJsonRes / stopwatch.ElapsedTicks, 2) + "X FASTER than System.Text.Json");

            if (BinaryGoRes > JsonNetRes)
            {
                double tt = BinaryGoRes / JsonNetRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"BinaryGo is {res}X SLOWER than JsonNET");
            }
            else
            {
                double tt = JsonNetRes / BinaryGoRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"BinaryGo is {res}X FASTER than JsonNET");
            }

            if (BinaryGoRes > MicrosoftJsonRes)
            {
                double tt = BinaryGoRes / MicrosoftJsonRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"BinaryGo is {res}X SLOWER than System.Text.Json");
            }
            else
            {
                double tt = MicrosoftJsonRes / BinaryGoRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"BinaryGo is {res}X FASTER than System.Text.Json");
            }
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
