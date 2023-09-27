using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class UserViewModel
    {

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

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid mobile number")]
        public string? MobileNo { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid landline number")]
        public string? LandlineNo { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? AlternativeNo { get; set; }
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
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string JoiningDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Boolean IsSupplier { get; set; }
        public Boolean IsFlexibleTime { get; set; }
        public Boolean IsVerifiedKYC { get; set; }
        public string AadhaarCardName { get; set; }
        public string PanCardName { get; set; }
        public string VoterCardName { get; set; }
        public string GSTNo { get; set; }
        public string BloodGroup { get; set; }
        public string GSTFormName { get; set; }
        public string VendorFormName { get; set; }
        public int OfficeAddressId { get; set; }
        public string OfficeAddress { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public IList<SelectListItem> RoleList { get; set; }
        public IList<SelectListItem> DepartmentList { get; set; }
        public IList<SelectListItem> UserList { get; set; }
        public IList<SelectListItem> OfficeAddressList { get; set; }

    }

    public class UserKYC
    {
        public int UserId { get; set; }
        public Boolean IsVerifiedKYC { get; set; }
        public string AadhaarCardName { get; set; }
        public string PanCardName { get; set; }
        public string VoterCardName { get; set; }
        public string GSTFormName { get; set; }
        public string VendorFormName { get; set; }

        public IFormFile? AadhaarCard { get; set; }
        public IFormFile? PanCard { get; set; }
        public IFormFile? VoterCard { get; set; }
        public IFormFile? GSTForm { get; set; }
        public IFormFile? VendorForm { get; set; }
    }

    public class OfficeAddressViewModel
    {
        public int OfficeAddressId { get; set; }
        public string OfficeAddressName { get; set; }
        public string FullAddress { get; set; }
        public int OfficeStateId { get; set; }
        public int OfficeLocationId { get; set; }
        public string OfficeLocation { get; set; }
        public string OfficeState { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public IList<SelectListItem> LocationList { get; set; }
        public IList<SelectListItem> StateList { get; set; }
    }
}
