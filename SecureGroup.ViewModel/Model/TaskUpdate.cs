using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class TaskUpdate
    {
        [Key]
        public int TaskUpdateId { get; set; }
        public int TaskId { get; set; }
        public string Comment { get; set; }
        public string SpentTime { get; set; }
        public string TaskDocument { get; set; }
        public int TaskStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Boolean IsActive { get; set; }
    }
}
