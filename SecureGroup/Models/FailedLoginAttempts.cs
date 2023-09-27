using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class FailedLoginAttempts
    {

        [Key]
        public int ID { get; set; }       
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string Browser { get; set; }
        public DateTime LogDateTime { get; set; }       
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Reason { get; set; }

    }
}
