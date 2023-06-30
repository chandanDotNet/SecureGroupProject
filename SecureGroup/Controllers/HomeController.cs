using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Library;
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

        public IActionResult Dashboard()
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
          // vm = DataAccessLayer.GetAllUser().ToList();

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginViewModel.Username) && !string.IsNullOrEmpty(loginViewModel.Password) && loginViewModel.RoleId>0)
                {
                    UserViewModel _userViewModel = new UserViewModel();
                    var Username = loginViewModel.Username;
                    var password = EncryptionLibrary.EncryptText(loginViewModel.Password);

                    _userViewModel = DataAccessLayerLinq.ValidateUser(Username, password, loginViewModel.RoleId);

                    if (_userViewModel.UserId>0)
                    {                       
                            var RoleID = _userViewModel.RoleId;
                            //remove_Anonymous_Cookies(); //Remove Anonymous_Cookies

                            HttpContext.Session.SetString("UserID", Convert.ToString(_userViewModel.UserId));
                            HttpContext.Session.SetString("RoleID", Convert.ToString(_userViewModel.RoleId));
                            HttpContext.Session.SetString("Email", Convert.ToString(_userViewModel.Email));
                            HttpContext.Session.SetString("Name", Convert.ToString(_userViewModel.Name));
                            
                            if (RoleID == 1)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (RoleID == 2)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (RoleID == 3)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                        
                    }
                    else
                    {
                        ViewBag.errormessage = "Entered Invalid Username and Password";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();


        }

        public IActionResult ForgotPasswordVerify()
        {
            return View();


        }

        public IActionResult MyAccount()
        {
            return View();

        }

        public IActionResult DepartmentManager()
        {
            return View();
        }

        public IActionResult CreateRole()
        {
            return View();
        }
    }
}

//aftab
//sadi