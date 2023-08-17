using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class PurchaseOrder
    {

        [Key]
        public int PurchaseOrderId { get; set; }
        public string PurchaseNo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int VendorId { get; set; }
        public string VendorAddress { get; set; }
        public string VendorEmail { get; set; }
        public string VendorGSTNo { get; set; }
        public string VendorMobileNo { get; set; }
        public string VendorCode { get; set; }
        public string ShipToAddress { get; set; }
        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string BillToGSTNo { get; set; }
        public string TermsConditions { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }      
        public Boolean IsActive { get; set; }

    }
}
