using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class LogManagement
    {

        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string Browser { get; set; }
        public DateTime LogDateTime { get; set; }
        public Boolean IsLogin { get; set; }
    }
}
