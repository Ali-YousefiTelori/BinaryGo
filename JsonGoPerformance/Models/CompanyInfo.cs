using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserInfo> Users { get; set; }
        public List<CarInfo> CarInfo { get; set; }
    }
}
