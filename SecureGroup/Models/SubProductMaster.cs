using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class SubProductMaster
    {
        [Key]
        public int SubProductId { get; set; }
        public int ProductId { get; set; }
        public string SubProductName { get; set; }
        public string Specifications { get; set; }
        public int UnitId { get; set; }
        public Boolean IsActive { get; set; }
    }
}
