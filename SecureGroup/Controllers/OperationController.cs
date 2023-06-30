using Microsoft.AspNetCore.Mvc;

namespace SecureGroup.Controllers
{
    public class OperationController : Controller
    {
        public IActionResult ProjectsList()
        {
            return View();
        }

        public IActionResult SitesList()
        {
            return View();
        }

        public IActionResult TaskAllocation()
        {
            return View();
        }
        public IActionResult DocumentList()
        {
            return View();
        }

       
    }
}
