using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class WareHouse
    {
        [Key]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }        
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public int WareHouseAdminId { get; set; }
        public Boolean IsActive { get; set; }

    }
}
