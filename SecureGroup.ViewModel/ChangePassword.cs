using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class ChangePassword
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
