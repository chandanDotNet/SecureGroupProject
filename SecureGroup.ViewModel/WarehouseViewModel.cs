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
        
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Address { get; set; }       
        public int CityId { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string Block { get; set; }
        public int UserId { get; set; }
        public string AdminName { get; set; }
        public string AdminContactNo { get; set; }
        public string AdminEmailId { get; set; }
        public IList<SelectListItem> CountryList { get; set;}
        public IList<SelectListItem> StateList { get; set;}
        public IList<SelectListItem> CityList { get; set;}
        public IList<SelectListItem> UserList { get; set;}

    }
}
