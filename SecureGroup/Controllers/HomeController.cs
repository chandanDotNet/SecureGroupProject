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
        DataAccessLayer DataAccessLayer = null;
        DataAccessLayerLinq DataAccessLayerLinq = null;
        public HomeController(ILogger<HomeController> logger, MsDBContext context)
        {
            _logger = logger;
            myDbContext = context;
            DataAccessLayer = new DataAccessLayer();
            DataAccessLayerLinq = new DataAccessLayerLinq(context);
        }

        public IActionResult Index()
        {
           // DataAccessLayer.GetCountry();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult User()
        {

           List<UserViewModel> vm = new List<UserViewModel>();


            //vm = (from user in myDbContext.User.Cast<UserViewModel>()                            
            //                select user).ToList();

            //Chandan
           vm = DataAccessLayer.GetAllUser().ToList();

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
