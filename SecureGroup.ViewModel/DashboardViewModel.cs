using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ProjectCount { get; set; }
        public int ActiveProjectCount { get; set; }
        public int TaskCount { get; set; }
        public int ActiveTaskCount { get; set; }
        public int SiteCount { get; set; }
        public int AdminCount { get; set; }
        public int EmployeeCount { get; set; }
        public int VendorCount { get; set; }
        public int SupplierCount { get; set; }

        public List<ProjectViewModel> projectList { get; set; }
        public List<TaskViewModel> taskList { get; set; }
    }
}
