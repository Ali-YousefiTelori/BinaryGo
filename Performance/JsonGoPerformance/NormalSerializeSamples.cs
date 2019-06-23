﻿using BenchmarkDotNet.Attributes;
using JsonGo;
using JsonGoPerformance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JsonGoPerformance
{
    public class NormalSerializeSamples
    {
        public static void InitializeChaches<T>(T obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Serializer.SingleIntance.Serialize(obj);
                Serializer.SingleIntance.SerializeCompile(obj);
                JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                System.Text.Json.Serialization.JsonSerializer.ToString(obj);
            }
        }
        public UserInfo GetSimpleSample()
        {
            UserInfo userInfo = new UserInfo()
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
            JsonGoModelBuilder.Initialize();

            NormalSerializeSamples normalSamples = new NormalSerializeSamples();
            NormalSerializeSamples.InitializeChaches(normalSamples.GetSimpleSample());
            NormalSerializeSamples.InitializeChaches(normalSamples.GetSimpleArraySample());
            NormalSerializeSamples.InitializeChaches(normalSamples.GetComplexObjectSample());
        }
        [Benchmark]
        public void RunSimpleSampleCompileTimeJsonGo()
        {
            Serializer serializer = new Serializer(false);
            serializer.SerializeCompile(GetSimpleSample());
        }

        [Benchmark]
        public void RunSimpleSampleJsonGo()
        {
            Serializer serializer = new Serializer(false);
            serializer.Serialize(GetSimpleSample());
        }

        [Benchmark]
        public void RunSimpleSampleJsonNet()
        {
            JsonConvert.SerializeObject(GetSimpleSample(), new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        [Benchmark]
        public void RunSimpleSampleTextJson()
        {
            System.Text.Json.Serialization.JsonSerializer.ToString(GetSimpleSample());
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
                System.Text.Json.Serialization.JsonSerializer.ToString(sample);
            }
            stopwatch.Stop();
            double MicrosoftJsonRes = stopwatch.ElapsedTicks;

            Console.WriteLine("System.Text.Json: \t " + stopwatch.Elapsed);

            Serializer serializer = new Serializer();

            Console.WriteLine("******* JsonGo *****");
            Console.WriteLine($"Count {count}");
            serializer.Setting.HasGenerateRefrencedTypes = false;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                serializer.Serialize(sample);
            }
            stopwatch.Stop();
            double JsonGoRes = stopwatch.ElapsedTicks;

            Console.WriteLine("JsonGo: \t " + stopwatch.Elapsed);
            Console.WriteLine("System.Text.Json: \t " + stopwatch.Elapsed);


            Console.WriteLine("******* JsonGo Compile Time *****");
            Console.WriteLine($"Count {count}");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                serializer.SerializeCompile(sample);
            }
            stopwatch.Stop();

            Console.WriteLine("JsonGo Compile Time: \t " + stopwatch.Elapsed);
            Console.WriteLine("JsonGo Compile Time: \t " + Math.Round(JsonNetRes / stopwatch.ElapsedTicks, 2) + "X FASTER than JsonNET");
            Console.WriteLine("JsonGo Compile Time: \t " + Math.Round(MicrosoftJsonRes / stopwatch.ElapsedTicks, 2) + "X FASTER than System.Text.Json");

            if (JsonGoRes > JsonNetRes)
            {
                double tt = JsonGoRes / JsonNetRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"JsonGo is {res}X SLOWER than JsonNET");
            }
            else
            {
                double tt = JsonNetRes / JsonGoRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"JsonGo is {res}X FASTER than JsonNET");
            }

            if (JsonGoRes > MicrosoftJsonRes)
            {
                double tt = JsonGoRes / MicrosoftJsonRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"JsonGo is {res}X SLOWER than System.Text.Json");
            }
            else
            {
                double tt = MicrosoftJsonRes / JsonGoRes;
                double res = Math.Round(tt, 2);
                Console.WriteLine($"JsonGo is {res}X FASTER than System.Text.Json");
            }
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}