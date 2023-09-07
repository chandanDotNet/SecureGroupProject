using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class PaymentInvoiceViewModel
    {

        public int InvoiceId { get; set; }
        public int QuatationToPaymentId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Vendor")]
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorEmail { get; set; }
        public string VendorGSTNo { get; set; }
        public string VendorMobileNo { get; set; }
        public string VendorCode { get; set; }
        public string ShipTo { get; set; }
        public string ShipToAddress { get; set; }
        public string BillTo { get; set; }
        public string BillToAddress { get; set; }
        public string BillToGSTNo { get; set; }
        public string TermsConditions { get; set; }
        public decimal AdvancePaymentPercentage { get; set; }
        public int CreatedBy { get; set; }

        public IList<SelectListItem> ProductList { get; set; }
        public IList<SelectListItem> SubProductList { get; set; }
        public IList<SelectListItem> UnitList { get; set; }
        public IList<SelectListItem> VendorList { get; set; }

        public List<InvoiceItemDetailsViewModel> InvoiceItemDetails { get; set; }

    }

    public class InvoiceItemDetailsViewModel
    {
        public int RowId { get; set; }
        public int InvoiceItemDetailsId { get; set; }
        public int InvoiceId { get; set; }
        public int QuatationToPaymentId { get; set; }
        public int ProductId { get; set; }
        public int SubProductId { get; set; }
        public decimal Thickness { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int GSTTypeId { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal GSTPercen { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public string ProductName { get; set; }
        public string SubProductName { get; set; }
        public string UnitName { get; set; }
        public string GSTType { get; set; }

        public IList<SelectListItem> ProductList { get; set; }
        public IList<SelectListItem> SubProductList { get; set; }
        public IList<SelectListItem> UnitList { get; set; }
        public IList<SelectListItem> VendorList { get; set; }
    }

    public class InvoiceDataForPdfViewModel
    {
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AdvancePaymentPercentage { get; set; }
        public decimal AdvancePaymentAmount { get; set; }
    }
}
