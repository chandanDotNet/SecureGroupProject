using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class DepartmentViewModel
    {

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Department Head")]
        public int DepartmentHead { get; set; }
        public string DepartmentHeadName { get; set; }
        public IList<SelectListItem> UserList { get; set; }

    }
}
