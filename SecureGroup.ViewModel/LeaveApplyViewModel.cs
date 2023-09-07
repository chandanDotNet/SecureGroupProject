using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class LeaveApplyViewModel
    {
        public int LeaveId { get; set; }
        public int UserId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string? LeaveReason { get; set; } = string.Empty;
        public int LeaveType { get; set; }
        public string LeaveTypeValue { get; set; }
        public IFormFile? Document { get; set; }
        public string  DocumentName { get; set; }
        public int LeaveStatus { get; set; }
        public int LeaveApproveRejectBy { get; set; }
        public string? LeaveRejectedReason { get; set; }
        public string NumberOfleaveCount { get; set; }

        public List<LeaveApplyHistory> LeaveApplyHistory { get; set; }

    }

    public class LeaveApplyHistory
    {
        public int LeaveId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string StartDate { get; set; }            
        public string EndDate { get; set; }
        public string NumberOfleaveCount { get; set; }
        public string? LeaveReason { get;set; }
        public int LeaveType { get; set;}
        public string LeaveTypeValue { get; set; }
        public string DocumentName { get; set; }
        public int LeaveStatus { get; set;}
        public string LeaveStatusValue { get; set;}
        public int LeaveApproveRejectBy { get; set; }
        public string LeaveApproveRejectByName { get; set; }
        public string? LeaveRejectedReason { get; set; }
        public int ReportingUserId { get; set; }
        public string ReportingUserName { get; set; }
        public int ReportingUserLeaveStatus { get; set; }
        public string ReportingUserLeaveStatusValue { get; set; }
        

    }
}
