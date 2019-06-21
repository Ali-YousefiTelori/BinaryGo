using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGoPerformance.Models
{
    public class UserCarInfo
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public CarInfo CarInfo { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
