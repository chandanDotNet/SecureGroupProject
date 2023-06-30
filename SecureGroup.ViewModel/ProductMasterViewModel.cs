using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class ProductMasterViewModel
    {

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please enter Product name")]
        public string ProductName { get; set; }
        public string Specifications { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a UOM")]
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public Boolean IsActive { get; set; }

        public IList<SelectListItem> UnitList { get; set; }
    }
}
