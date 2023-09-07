using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class QuatationViewModel
    {
        public int QuatationId { get; set; }
        public int UserId { get; set; }      
        public string QuatationName { get; set; }
        public string QuatationFileName { get; set; }
        public IFormFile QuatationFile { get; set; }
        public IList<SelectListItem> UserList { get; set; }

    }
}
