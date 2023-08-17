using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class AttendanceViewModel
    {

      
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string DurationTime { get; set; }
        public string Reason { get; set; }
        public int AttendanceStatusId { get; set; }
        public string AttendanceStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public string DeviationsLoginTime { get; set; }
        public string DeviationsLogoutTime { get; set; }
        public string DeviationsDurationTime { get; set; }
        public int DeviationsApprovalStatusId { get; set; }
        public string DeviationsApprovalStatus { get; set; }
        public int AttendanceApproveRejectBy { get; set; }
        public string AttendanceApproveRejectByName { get; set; }



    }

    public class AttendanceRaperViewModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public int AttendanceId { get; set; }
        public string Reason { get; set; }

        public string DeviationsLoginTime { get; set; }
        public string DeviationsLogoutTime { get; set; }
        public string DeviationsDurationTime { get; set; }
        public int DeviationsApprovalStatusId { get; set; }

        public Boolean IsMyAttendance { get; set; }

        public IList<SelectListItem> YearList { get; set; }
        public IList<SelectListItem> MonthList { get; set; }
       
        public List<AttendanceViewModel> attendanceListViewModel { get; set; }
    }
}
