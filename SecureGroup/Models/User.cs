using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }       
        public string LandlineNo { get; set; }        
        public string AlternativeNo { get; set; }
        public int RoleId { get; set; }
        public string JobTitle { get; set; }
        public string UserCode { get; set; }
        public int DepartmentId { get; set; }
        public int ReportingTo { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string BloodGroup { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsActive { get; set; }




    }
}
