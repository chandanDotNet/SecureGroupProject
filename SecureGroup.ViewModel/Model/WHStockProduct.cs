using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class WHStockProduct
    {
        [Key]
        public int Id { get; set; }
        public int WareHouseId { get; set; }
        public int ProductId { get; set; } = 0;
        public int SubProductId { get; set; } = 0;
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }      
       

    }
}
