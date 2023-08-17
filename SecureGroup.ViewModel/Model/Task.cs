using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        public int AssignTo { get; set; }
        public string TaskName { get; set; }
        public string TaskDueDateTime{get; set;}
        public int ProjectId { get; set; }
        public int ProjectHeadId { get; set; }
        public int SiteId { get; set; }
        public string TaskPriority { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDocument { get; set; }
        public int TaskStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Boolean IsActive { get; set; }
    }
}
