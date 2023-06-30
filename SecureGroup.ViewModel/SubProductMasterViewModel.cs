using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class SubProductMasterViewModel
    {
        public int SubProductId { get; set; }
        [Required(ErrorMessage = "Please enter Sub Product name")]
        public string SubProductName { get; set; }
        public string Specifications { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a UOM")]
        public int  UnitId { get; set; }
        public string Unit { get; set; }

        public IList<SelectListItem> UnitList { get; set; }
        public IList<SelectListItem> ProductList { get; set; }
    }
}
