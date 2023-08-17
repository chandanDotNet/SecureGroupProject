using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class Scheme
    {

        [Key]
        public int SchemeId { get; set; }
        public string SchemeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Boolean IsActive { get; set; }
    }
}
