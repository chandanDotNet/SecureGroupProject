using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class SiteViewModel
    {

        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Block { get; set; }
        public string Scheme { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public float? Lat { get; set; }
        public float? Long { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        public IList<SelectListItem> CityList { get; set; }
        public IList<SelectListItem> StateList { get; set; }

    }
}

