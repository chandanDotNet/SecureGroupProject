using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class WarehouseViewModel
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Please enter Warehouse name")]
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a City")]
        public int CityId { get; set; }       
        public string City { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a State")]
        public int StateId { get; set; }
        public string State { get; set; }

        [MaxLength(6)]       
        [MinLength(6,ErrorMessage = "Enter valid ZipCode")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ZipCode must be numeric")]
        public string ZipCode { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Country")]
        public int CountryId { get; set; }

        
        public string Country { get; set; }
        [Required(ErrorMessage = "Please enter Block")]
        public string Block { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a User")]
        public int UserId { get; set; }
        public string AdminName { get; set; }
        public string AdminContactNo { get; set; }
        public string AdminEmailId { get; set; }
     
        public IList<SelectListItem> CountryList { get; set;}
     
        public IList<SelectListItem> StateList { get; set;}
       
        public IList<SelectListItem> CityList { get; set;}
      
        public IList<SelectListItem> UserList { get; set;}

    }


    public class WarehouseRapViewModel
    {
       
        public List<WarehouseViewModel> warehouseViewModel { get; set; }
        public IList<SelectListItem> CountryList { get; set; }
        public IList<SelectListItem> StateList { get; set; }
        public IList<SelectListItem> CityList { get; set; }
        public IList<SelectListItem> UserList { get; set; }
    }
}
