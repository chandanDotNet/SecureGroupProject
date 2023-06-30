using Microsoft.AspNetCore.Mvc;

namespace SecureGroup.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult ClientList()
        {
            return View();
        }
    }
}
