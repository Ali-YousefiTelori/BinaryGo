using BinaryGoTest.Models.Complex;
using BinaryGoTest.Models.Inheritance;
using BinaryGoTest.Models.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryGoTest.Binary.Objects
{
    public class BinaryComplexObjectsSerializationsTest
    {
        static Random Random = new Random();
        #region ComplexUser
        public ComplexUser GetComplexUser()
        {
            return new ComplexUser()
            {
                Id = 2751,
                UserName = "Ali\r\n علی" + Random.Next(10, int.MaxValue).ToString(),
                Password = "Yousefi \t Telori یوسفی یونیکد",
                Companies = GetCompanies().ToArray()
            };
        }


        public List<ComplexCompanyInfo> GetCompanies()
        {
            List<ComplexCompanyInfo> result = new List<ComplexCompanyInfo>();
            for (int i = 0; i < Random.Next(10, 100); i++)
            {
                result.Add(new ComplexCompanyInfo()
                {
                    Id = Random.Next(0, int.MaxValue),
                    IsClosed = true,
                    Key = Guid.NewGuid(),
                    Name = "Hello World",
                    Type = ComapnyType.Goverment,
                    Cars = GetCars()
                });
            }
            return result;
        }
        public List<ComplexCarInfo> GetCars()
        {
            List<ComplexCarInfo> result = new List<ComplexCarInfo>();
            for (int i = 0; i < Random.Next(10, 100); i++)
            {
                result.Add(new ComplexCarInfo()
                {
                    Name = "Good Car",
                    CreationDateTime = DateTime.Now,
                    Data = new byte[] { 5, 6, 8, 9, 11, 250, 110 },
                    Weight = 15640.156
                });
            }
            return result;
        }

        [Fact]
        public (byte[] Result, ComplexUser Value) ComplexUserTestSerialize()
        {
            var value = GetComplexUser();
            var result = BinaryGo.Binary.BinarySerializer.NormalInstance.Serialize(value);
            return (result.ToArray(), value);
        }

        #endregion

    }
}
