using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class TaskUpdateViewModel
    {

        public int TaskUpdateId { get; set; }
        public int TaskId { get; set; }
        public string Comment { get; set; }
        public string SpentTime { get; set; }
        public string TaskDocumentName { get; set; }
        public IFormFile TaskDocument { get; set; }
        public int TaskStatus { get; set; }
        public string TaskStatusValue { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDateTime { get; set; }

        public IList<SelectListItem> TaskStatusList { get; set; }
    }
}
