using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class SupplierDetails
    {
        [Key]
        public int SupplierId { get; set; }        
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string FullAddress { get; set; }
        public Boolean IsActive { get; set; }

    }
}
