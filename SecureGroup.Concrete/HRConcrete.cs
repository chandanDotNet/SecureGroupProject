using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

using SecureGroup.Interface;

using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.Concrete
{
    public class HRConcrete :IHR
    {

        private MsSqlDBContext myDbContext;
        string connectionString = ConnectionString.CName;

        public HRConcrete(MsSqlDBContext context)
        {
            myDbContext = context;
        }


        public List<SelectListItem> GetDropDownListData(string DDName, int? Id, string DDType)
        {
            var list = new List<SelectListItem>();

            if (DDName == "SysVal")
            {
                list = (from sysVal in myDbContext.SysVal
                        where sysVal.IsActive == true && sysVal.Type == DDType
                        orderby sysVal.Value ascending
                        select new SelectListItem()
                        {
                            Text = sysVal.Value,
                            Value = sysVal.Id.ToString(),
                        }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            return list;

        }

        public IEnumerable<AttendanceViewModel> GetUserAttendanceList(int ActionId, int AttendanceId, int UserId, int CreatedBy)
        {
            List<AttendanceViewModel> _attendanceList = new List<AttendanceViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AttendanceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@AttendanceId", AttendanceId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AttendanceViewModel _attendance = new AttendanceViewModel();

                    _attendance.AttendanceId = Convert.ToInt32(rdr["AttendanceId"]);
                    _attendance.UserId = Convert.ToInt32(rdr["UserId"]);
                    _attendance.Date = rdr["Date"].ToString();
                    _attendance.LoginTime = rdr["LoginTime"].ToString();
                    _attendance.LogoutTime = rdr["LogoutTime"].ToString();
                    _attendance.DurationTime = rdr["DurationTime"].ToString();
                    _attendance.Reason = rdr["Reason"].ToString();
                    _attendance.AttendanceStatusId = Convert.ToInt32(rdr["AttendanceStatusId"]);
                    _attendance.AttendanceStatus = rdr["AttendanceStatus"].ToString();
                    _attendance.CreatedBy = (Int32)rdr["CreatedBy"];
                    _attendance.CreatedDate = (DateTime)rdr["CreatedDateTime"];
                    _attendance.UserName = rdr["UserName"].ToString();
                    _attendance.DeviationsLoginTime = rdr["DeviationsLoginTime"].ToString();
                    _attendance.DeviationsLogoutTime = rdr["DeviationsLogoutTime"].ToString();
                    _attendance.DeviationsDurationTime = rdr["DeviationsDurationTime"].ToString();
                    _attendance.DeviationsApprovalStatusId = (Int32)rdr["DeviationsApprovalStatusId"];
                    _attendance.DeviationsApprovalStatus = rdr["DeviationsApprovalStatus"].ToString();

                    _attendanceList.Add(_attendance);
                }
                con.Close();
            }
            return _attendanceList;
        }

    }
}
