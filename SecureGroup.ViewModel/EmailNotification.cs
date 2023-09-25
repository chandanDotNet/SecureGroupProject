using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class EmailNotificationViewModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string AssignToName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
    }
}
