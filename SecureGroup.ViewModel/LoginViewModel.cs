using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Role")]
        public int RoleId { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public bool IsDesktopPCLogin { get; set; }

        public IList<SelectListItem> RoleList { get; set; }
    }
}
