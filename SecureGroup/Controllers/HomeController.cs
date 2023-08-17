using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Library;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using SecureGroup.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SecureGroup.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private MsDBContext myDbContext;
        DataAccessLayer DataAccessLayer = null;
        DataAccessLayerLinq DataAccessLayerLinq = null;
       // HttpContext httpContext= _httpContext;
        public  HttpContext _httpContext;

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

        public IActionResult Dashboard(int RoleID)
        {
            var _userSessionData = GetUserSession();
            DashboardViewModel _dModel = new DashboardViewModel();
            _dModel.RoleId = RoleID;
            _dModel.RoleName = _userSessionData.RoleName.ToString();
                 
            
            return View(_dModel);
        }

        public IActionResult EDashboard()
        {
            var SessionData = GetUserSession();
           
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
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error2()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            LoginViewModel _loginViewModel = new LoginViewModel();
            _loginViewModel.RoleList = DataAccessLayerLinq.GetDropDownListData("UserRole", 0).Where(x => x.Value == "1" || x.Value == "4" || x.Value == "2" || x.Value == "5").ToList(); ;

            return View(_loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(loginViewModel.Username) && !string.IsNullOrEmpty(loginViewModel.Password) && loginViewModel.RoleId > 0)
                {
                    UserViewModel _userViewModel = new UserViewModel();
                    var Username = loginViewModel.Username;
                    var password = EncryptionLibrary.EncryptText(loginViewModel.Password);

                    _userViewModel = DataAccessLayerLinq.ValidateUser(Username, password, loginViewModel.RoleId);

                    if (_userViewModel != null && _userViewModel.UserId > 0)
                    {
                        var RoleID = _userViewModel.RoleId;
                        //remove_Anonymous_Cookies(); //Remove Anonymous_Cookies
                        
                        InsertUserSession(_userViewModel); //Store Session data                      

                        InsertLogInfo(_userViewModel,true);
                        TempData["successmessage"] = "You are successfully logged in";

                        return RedirectToAction("Dashboard", "Home", RoleID);
                        //if (RoleID == 1) //Admin
                        //{
                        //    return RedirectToAction("Dashboard", "Home");
                        //}
                        //else if (RoleID == 2) //Vendor
                        //{
                        //    return RedirectToAction("VDashboard", "Home");
                        //}
                        //else if (RoleID == 3) //Client
                        //{
                        //    return RedirectToAction("CDashboard", "Home");
                        //}
                        //else if (RoleID == 4) //Employee
                        //{
                        //    return RedirectToAction("EDashboard", "Home");
                        //}

                    }
                    else
                    {
                        loginViewModel.RoleList = DataAccessLayerLinq.GetDropDownListData("UserRole", 0);
                        TempData["errormessage"] = "Entered Invalid Username and Password";
                        return View(loginViewModel);
                    }
                }

                TempData["errormessage"] = "Entered Invalid Username and Password";
                loginViewModel.RoleList = DataAccessLayerLinq.GetDropDownListData("UserRole", 0);
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
        }


      

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                UserViewModel _userViewModel=new UserViewModel();
                CookieOptions option = new CookieOptions();

                if (Request.Cookies["SGChannel"] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append("SGChannel", "", option);
                }
                _userViewModel.UserId = GetUserSession().UserId;
                _userViewModel.RoleId= GetUserSession().RoleId;
                InsertLogInfo(_userViewModel, false);
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void InsertLogInfo(UserViewModel _userViewModel,Boolean IsLogin)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            var mackAddress = HttpContext.Connection.LocalIpAddress;
            LogManagement logManagement = new LogManagement();
            logManagement.UserId = _userViewModel.UserId;
            logManagement.RoleId = _userViewModel.RoleId;
            logManagement.LogDateTime = DateTime.Now;
            logManagement.IsLogin = IsLogin;
            logManagement.IpAddress= remoteIpAddress.ToString();
            DataAccessLayerLinq.InsertLogManagement(logManagement);


            AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
            attendanceViewModel.CreatedBy = GetUserSession().UserId;
            attendanceViewModel.UserId = GetUserSession().UserId;
            if(IsLogin==true)
            {
                attendanceViewModel.AttendanceStatusId = 1;
            }else if(IsLogin==false)
            {
                attendanceViewModel.AttendanceStatusId = 2;
            }
            

            DataAccessLayer.UpdateUserAttendanceeTimeData(attendanceViewModel, 9);



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
            UserViewModel _userViewModel = new UserViewModel();
            int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                // _userViewModel = _dataAccessLayerLinq.GetUserList(Id, 0).FirstOrDefault();
                _userViewModel = DataAccessLayer.GetAllUser(4, UserId, 0).FirstOrDefault();                
                //_userViewModel.Password = EncryptionLibrary.DecryptText(_userViewModel.Password);
            }
            return View(_userViewModel);

        }

        public IActionResult UserKYCUpload()
        {
            UserKYC _userKYC = new UserKYC();           
            int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                _userKYC.UserId = UserId;
                var _user = DataAccessLayer.GetAllUser(4,UserId, 0).FirstOrDefault();
                if (_user != null)
                {
                    _userKYC.AadhaarCardName = _user.AadhaarCardName;
                    _userKYC.PanCardName = _user.PanCardName;
                    _userKYC.VoterCardName = _user.VoterCardName;
                }
            }
            return View(_userKYC);
        }

        [HttpPost]
        public IActionResult UserKYCUpload(UserKYC _userKYC)
        {
            try
            {
                int response = 0;
                int UserId = GetUserSession().UserId;
                if (UserId > 0)
                {
                    _userKYC.UserId = UserId;
                    // _userViewModel = _dataAccessLayerLinq.GetUserList(Id, 0).FirstOrDefault();                
                }
                if (_userKYC.AadhaarCard != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_userKYC.AadhaarCard, "Upload/UserKYC");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _userKYC.AadhaarCardName = _uploadFileResponse.FileName;
                        }
                    }
                }
                if (_userKYC.PanCard != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_userKYC.PanCard, "Upload/UserKYC");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _userKYC.PanCardName = _uploadFileResponse.FileName;
                        }
                    }
                }
                if (_userKYC.VoterCard != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_userKYC.VoterCard, "Upload/UserKYC");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _userKYC.VoterCardName = _uploadFileResponse.FileName;
                        }
                    }
                }        

                response = DataAccessLayer.UpdateUserKYCData(5,_userKYC);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your file has been uploaded successfully";
                    return RedirectToAction(nameof(MyAccount));
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }              

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }            
           
            return RedirectToAction("MyAccount");

        }

        public IActionResult UserKYCVerify()
        {
            int response = 0;
            UserKYC _userKYC = new UserKYC();
            int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                _userKYC.UserId = UserId;              
            }

            response = DataAccessLayer.UpdateUserKYCData(6, _userKYC);
            if (response > 0)
            {
                TempData["successmessage"] = "Verified successfully";
                return RedirectToAction(nameof(MyAccount));
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return View(_userKYC);

        }

        public IActionResult DownloadFile(string filename)
        {

            if (filename==null)
            {
                TempData["errormessage"] = "Something went wrong!- File Not Exists!";
                return RedirectToAction("UserKYCUpload");
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload/UserKYC", filename);
            if (!System.IO.File.Exists(path))
            {
                TempData["errormessage"] = "Something went wrong!- File Not Exists!";
                return RedirectToAction("UserKYCUpload");
            }
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload/UserKYC", filename);
            //var path = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.xlsx");
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", filename);
            // call GetFile Method in service and return

            // var dd= FileDownload(filename, "Upload");

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

//test
