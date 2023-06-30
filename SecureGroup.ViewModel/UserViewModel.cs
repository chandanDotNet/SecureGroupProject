using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class UserViewModel    {
        
        public int UserId { get; set; }
        public string BusinessName { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactNo { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Role")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string JobTitle { get; set; }
        public string UserCode { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Department")]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Reporting to")]
        public int ReportingTo { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsSupplier { get; set; }
        public Boolean IsFlexibleTime { get; set; }

        public IList<SelectListItem> RoleList { get; set; }
        public IList<SelectListItem> DepartmentList { get; set; }
        public IList<SelectListItem> UserList { get; set; }

    }
}
