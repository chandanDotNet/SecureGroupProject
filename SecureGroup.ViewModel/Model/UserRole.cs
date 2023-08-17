using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public string RoleName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
