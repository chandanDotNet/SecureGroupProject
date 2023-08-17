using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class LeaveApply
    {
        [Key]
        public int LeaveId { get; set; }
        public int UserId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LeaveReason { get; set; } = string.Empty;
        public int LeaveType { get; set; }      
        public string Document { get; set; }
        public int LeaveStatus { get; set; }
        public int LeaveApproveRejectBy { get; set; }
        public string LeaveRejectedReason { get; set; }
        public Boolean IsActive { get; set; }

    }
}
