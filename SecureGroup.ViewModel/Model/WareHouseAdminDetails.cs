using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class WareHouseAdminDetails
    {
        [Key]
        public int WareHouseAdminId { get; set; }
        public int WareHouseId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean IsActive { get; set; }

        
    }
}
