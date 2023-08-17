using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class ProductMaster
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Specifications { get; set; }
        public int UnitId { get; set; }
        public int GSTTypeId { get; set; }
        public decimal GSTPercen { get; set; }
        public Boolean IsActive { get; set; } 
    }
}
