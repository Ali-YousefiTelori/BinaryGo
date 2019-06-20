using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
