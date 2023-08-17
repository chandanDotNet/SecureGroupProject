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
    public class TaskViewModel
    {

        public int TaskId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Assign To")]
        public int AssignTo { get; set; }
        public string AssignToName { get; set; }
        [Required(ErrorMessage = "Please enter Task Name")]
        public string TaskName { get; set; }
        [Required(ErrorMessage = "Please enter Task Due Date Time")]
        public string TaskDueDateTime { get; set; }
        public string TaskDueTime { get; set; }
        public string TaskDueDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectHeadId { get; set; }
        public string ProjectHeadName { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        [Required(ErrorMessage = "Must Choose a Task Priority")]
        public string TaskPriority { get; set; }
        public string? TaskDescription { get; set; }
        public string TaskDocumentName { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Task Status")]
        public int TaskStatus { get; set; }
        public string TaskStatusValue { get; set; }
       
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public IFormFile? TaskDocument { get; set; }
        public string Comment { get; set; }

        public List<TaskUpdateViewModel> TaskUpdateList { get; set; }
        public TaskUpdateViewModel TaskUpdate { get; set; }
        public IList<SelectListItem> TaskStatusList { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public IList<SelectListItem> ProjectHeadList { get; set; }
        public IList<SelectListItem> SiteList { get; set; }
        public IList<SelectListItem> UserList { get; set; }
    }
}
