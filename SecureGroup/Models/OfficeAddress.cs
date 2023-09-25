using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class OfficeAddress
    {
        [Key]
        public int OfficeAddressId { get; set; }
        public string OfficeAddressName { get; set; }
        public string FullAddress { get; set; }
        public int OfficeStateId { get; set; }
        public int OfficeLocationId { get; set; }     
        public double Latitude { get; set; }
        public double Longitutde { get; set; }

        public Boolean IsActive { get; set; }
    }
}
