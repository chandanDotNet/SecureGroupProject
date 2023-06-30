using System;

namespace SecureGroup.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }= string.Empty;
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }

    }
}
