using Microsoft.AspNetCore.Mvc;


namespace SecureGroup.Controllers
{
    public class ClientController : Controller
    {

      //private  IHR _IHr;

        //public ClientController(IHR iHr)
        //{
        //    _IHr = iHr;
        //}
        public ActionResult ClientList()
        {
            //var data=_IHr.GetUserAttendanceList
            return View();
        }
    }
}
