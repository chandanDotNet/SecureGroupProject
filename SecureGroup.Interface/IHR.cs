using Microsoft.AspNetCore.Mvc.Rendering;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.Interface
{
    public interface IHR
    {


        List<SelectListItem> GetDropDownListData(string DDName, int? Id, string DDType);
        IEnumerable<AttendanceViewModel> GetUserAttendanceList(int ActionId, int AttendanceId, int UserId, int CreatedBy);
    }
}
