using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class WHStockTransferManageViewModel
    {

        public int Id { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Transfer Type")]
        public int TransferType { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Source")]
        public int SourceId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Destination")]
        public int DestinationId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Product")]
        public int ProductId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a SubProduct")]
        public int SubProductId { get; set; }
        [Required(ErrorMessage = "Please Enter Transfer Quantity")]
        public decimal TransferQuantity { get; set; }
        [Required(ErrorMessage = "Please Enter Transfer Date")]
        public DateTime TransferDate { get; set; }
        public int TransferBy { get; set; }


        public IList<SelectListItem> TransferTypeList { get; set; }
        public IList<SelectListItem> ProductList { get; set; }

        public IList<SelectListItem> SubProductList { get; set; }

        public IList<SelectListItem> WareHouseList { get; set; }

    }
}
