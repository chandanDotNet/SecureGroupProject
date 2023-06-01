using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SecureGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MsDBContext myDbContext;       

        public HomeController(ILogger<HomeController> logger, MsDBContext context)
        {
            _logger = logger;
            myDbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult User()
        {

           List<UserViewModel> vm = new List<UserViewModel>();
            vm = (from user in myDbContext.User.Cast<UserViewModel>()                            
                            select user).ToList();

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
