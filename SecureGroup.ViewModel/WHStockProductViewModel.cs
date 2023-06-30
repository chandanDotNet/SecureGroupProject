using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class WHStockProductViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a WareHouse")]
        public int WareHouseId { get; set; }
        public string WareHouse { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a SubProduct")]
        public int SubProductId { get; set; }
        public string SubProduct { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Unit")]
        public int UnitId { get; set; }
        public string Unit { get; set; }
        [Required(ErrorMessage = "Please Enter Quantity")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage = "Please Enter Unit Price")]
        public decimal UnitPrice { get; set; }
        [Required(ErrorMessage = "Please Enter Amount")]
        public decimal Amount { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Supplier")]
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierContactDetails { get; set; }
        public int CreatedBy { get; set; }
       

        public IList<SelectListItem> ProductList { get; set; }

        public IList<SelectListItem> SubProductList { get; set; }

        public IList<SelectListItem> WareHouseList { get; set; }

        public IList<SelectListItem> UnitList { get; set; }

        public IList<SelectListItem> SupplierList { get; set; }

    }
}
