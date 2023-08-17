using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class PurchaseOrderDetails
    {

        [Key]
        public int PurchaseOrderDetailsId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int SubProductId { get; set; }
        public decimal Thickness { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int GSTTypeId { get; set; }
        public decimal GSTAmount { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }

    }
}
