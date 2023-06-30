using Microsoft.AspNetCore.Mvc;

namespace SecureGroup.Controllers
{
    public class HRController : Controller
    {
        public IActionResult WorkingHour()
        {
            return View();
        }
    }
}
