using JsonGo;
using JsonGoPerformance.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JsonGoPerformance
{
    public static class LoopReferenceSamples
    {
        public static void InitializeChaches<T>(T obj)
        {
            for (int i = 0; i < 10; i++)
            {
                var result1 = Serializer.SingleIntance.Serialize(obj);
                JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Arrays
                });
                //System.Text.Json.Serialization.JsonSerializer.ToString(obj);
            }
        }

        public static CompanyInfo GetSimpleSample()
        {
            CompanyInfo companyInfo = new CompanyInfo()
            {
                Id = 1,
                Name = "company name",
                Users = new List<UserInfo>()
            };
            UserInfo userInfo = new UserInfo()
            {
                Age = 28,
                CreatedDate = DateTime.Now,
                FullName = "Ali Yousefi Telori",
                Id = 1,
                CompanyInfo = companyInfo
            };
            companyInfo.Users.Add(userInfo);
            return companyInfo;
        }

        public static List<CompanyInfo> GetSimpleArraySample()
        {
            List<CompanyInfo> result = new List<CompanyInfo>();
            for (int i = 1; i < 50; i++)
            {
                CompanyInfo companyInfo = new CompanyInfo()
                {
                    Id = i,
                    Name = "company name",
                    Users = new List<UserInfo>()
                };
                UserInfo user = new UserInfo()
                {
                    Age = 28 + i,
                    CreatedDate = DateTime.Now.AddMinutes(i),
                    FullName = "Ali Yousefi Telori " + i,
                    Id = i,
                    CompanyInfo = companyInfo
                };
                companyInfo.Users.Add(user);
                result.Add(companyInfo);
            }

            return result;
        }
        public static List<RoleInfo> GetArrayRoles(UserInfo userInfo)
        {
            List<RoleInfo> result = new List<RoleInfo>();
            RoleInfo roleInfo = new RoleInfo()
            {
                Id = 1,
                Type = RoleType.Admin,
                UserInfo = userInfo
            };
            result.Add(roleInfo);
            RoleInfo roleInfo2 = new RoleInfo()
            {
                Id = 2,
                Type = RoleType.Normal,
                UserInfo = userInfo
            };
            result.Add(roleInfo2);
            RoleInfo roleInfo3 = new RoleInfo()
            {
                Id = 3,
                Type = RoleType.Viewer,
                UserInfo = userInfo
            };
            result.Add(roleInfo3);
            return result;
        }

        public static List<ProductInfo> GetArrayProducts(UserInfo userInfo)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            for (int i = 1; i < 20; i++)
            {
                ProductInfo product = new ProductInfo()
                {
                    Id = i,
                    Name = "product" + i,
                    CreatedDate = DateTime.Now,
                    UserInfo = userInfo
                };
                result.Add(product);
            }

            return result;
        }
        public static List<CarInfo> GetArrayCars(CompanyInfo companyInfo)
        {
            List<CarInfo> result = new List<CarInfo>();
            for (int i = 1; i < 50; i++)
            {
                CarInfo car = new CarInfo()
                {
                    Id = i,
                    Name = "car" + i,
                    CompanyInfo = companyInfo
                };
                result.Add(car);
            }

            return result;
        }
        public static List<CompanyInfo> GetComplexObjectSample()
        {
            var companies = GetSimpleArraySample();
            foreach (var item in companies)
            {
                item.Cars = GetArrayCars(item);
            }
            foreach (var item in companies)
            {
                foreach (var user in item.Users)
                {
                    user.Roles = GetArrayRoles(user);
                }
                foreach (var user in item.Users)
                {
                    user.Products = GetArrayProducts(user);
                }
            }
            return companies;
        }

        public static UserInfo GetComplexSample()
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
        public static void Run<T>(T sample, int count)
        {
            InitializeChaches(sample);
            for (int i = 0; i < 5; i++)
            {
                RunSample(sample, count);
            }

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
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Arrays
                });
            }
            stopwatch.Stop();
            double JsonNetRes = stopwatch.ElapsedTicks;
            Console.WriteLine("Newtonsoft.JsonNET: \t " + stopwatch.Elapsed);

            Console.WriteLine("******* System.Text.Json NOT SUPPORT YET *****");

            //Console.WriteLine($"Count {count}");

            //stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < count; i++)
            //{
            //    System.Text.Json.Serialization.JsonSerializer.ToString(sample);
            //}
            //stopwatch.Stop();
            //double MicrosoftJsonRes = stopwatch.ElapsedTicks;

            //Console.WriteLine("System.Text.Json: \t " + stopwatch.Elapsed);

            Serializer serializer = new Serializer();

            Console.WriteLine("******* JsonGo *****");
            Console.WriteLine($"Count {count}");
            serializer.Setting.HasGenerateRefrencedTypes = true;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                serializer.Serialize(sample);
            }
            stopwatch.Stop();
            double JsonGoRes = stopwatch.ElapsedTicks;

            Console.WriteLine("JsonGo Runtime Time: \t " + stopwatch.Elapsed);

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

            //if (JsonGoRes > MicrosoftJsonRes)
            //{
            //    double tt = JsonGoRes / MicrosoftJsonRes;
            //    double res = Math.Round(tt, 2);
            //    Console.WriteLine($"JsonGo is {res}X SLOWER than System.Text.Json");
            //}
            //else
            //{
            //    double tt = MicrosoftJsonRes / JsonGoRes;
            //    double res = Math.Round(tt, 2);
            //    Console.WriteLine($"JsonGo is {res}X FASTER than System.Text.Json");
            //}
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
