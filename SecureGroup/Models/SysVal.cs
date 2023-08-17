using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class SysVal
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public Boolean IsActive { get; set; }

    }
}
