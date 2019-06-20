using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public class CarInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CompanyInfo CompanyInfo { get; set; }
    }
}
