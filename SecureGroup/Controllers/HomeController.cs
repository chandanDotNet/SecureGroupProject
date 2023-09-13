using Azure;
using Microsoft.AspNetCore.Hosting;
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
using System.Net;
using System.Net.Http;
using System.Net.Mail;
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
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, MsDBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            myDbContext = context;
            this._env = env;
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
            var _userSessionData = GetUserSession();
            DashboardViewModel _dModel = new DashboardViewModel();
            _dModel.RoleId = _userSessionData.RoleId;
            int UserId=_userSessionData.UserId;
            _dModel.RoleName = _userSessionData.RoleName.ToString();
            if(_userSessionData.RoleId==1)
            {
                _dModel = DataAccessLayer.GetDashboardData(1, 0);
            }
            if(_userSessionData.RoleId == 4)
            {
                _dModel = DataAccessLayer.GetDashboardData(2, UserId);
            }
            
            _dModel.projectList = DataAccessLayer.GetProjectsListData(3, 0).ToList();
            _dModel.taskList = DataAccessLayer.GetAllTaskAllocation(4, 0, UserId, 0).ToList();
            _dModel.RoleId = _userSessionData.RoleId;
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
            _loginViewModel.RoleList = DataAccessLayerLinq.GetDropDownListData("UserRole", 0).Where(x => x.Value == "1" || x.Value == "4" || x.Value == "2" || x.Value == "5").ToList(); 

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

                        return RedirectToAction("Dashboard", "Home");
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
        public IActionResult ForgotPasswordVerify()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPasswordVerify(string otp)
        {
            ChangePassword model = new ChangePassword();
            var email = TempData["email"];
            TempData.Keep("email");
            if (otp == null)
                return NoContent();
            else 
            {
                //if ((DateTime.Now - Convert.ToDateTime(TempData["timestamp"])).TotalSeconds < 30)
                var result = DataAccessLayer.OtpVerification(2, (string)email, otp);
                if (result > 0)
                {
                    TempData["UserId"] = result;
                    model.UserId = result;                    
                    return View("ChangePassword", model);
                }
                
            }

            return View("ChangePassword", model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                changePassword.NewPassword = EncryptionLibrary.EncryptText(changePassword.NewPassword);
                var response = DataAccessLayer.ChangePasswordManagement(3, changePassword);
                if (response > 0)
                {
                    TempData["successmessage"] = "Password changed successfully";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

            }
            return View();
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
       
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        public string getPasswordEmailHtmlTemplate()
        {
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "Templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "ForgetPasswordEmail.html";
            string HtmlBody = string.Empty;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                HtmlBody = SourceReader.ReadToEnd();
            }
            return HtmlBody;
        }



        public int GenerateOtp()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            string HtmlBody = getPasswordEmailHtmlTemplate();
            var otp = GenerateOtp();
            string mailBody = HtmlBody.Replace("{OTP}", otp.ToString());
            var result = sendEmail("Forget Password", mailBody, "crmsifsl@gmail.com", email, "", "");
            if (result)
            {
                DataAccessLayer.AddUpdateOtp(1, email, otp);
            }
            //DataAccessLayer.AddUpdateOtp(1, email, otp);
            TempData["email"] = email;
            TempData["timestamp"] = DateTime.Now;
            return Json("Otp sent successfully");
            //return View();
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

        public IActionResult UserKYCUpload(int UserId)
        {
            UserKYC _userKYC = new UserKYC();           
            //int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                _userKYC.UserId = UserId;
                var _user = DataAccessLayer.GetAllUser(4,UserId, 0).FirstOrDefault();
                if (_user != null)
                {
                    _userKYC.AadhaarCardName = _user.AadhaarCardName;
                    _userKYC.PanCardName = _user.PanCardName;
                    _userKYC.VoterCardName = _user.VoterCardName;
                    _userKYC.GSTFormName = _user.GSTFormName;
                    _userKYC.VendorFormName = _user.VendorFormName;
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
                //int UserId = GetUserSession().UserId;
                int UserId = _userKYC.UserId;
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
                if (_userKYC.GSTForm != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_userKYC.GSTForm, "Upload/UserKYC");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _userKYC.GSTFormName = _uploadFileResponse.FileName;
                        }
                    }
                }
                if (_userKYC.VendorForm != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_userKYC.VendorForm, "Upload/UserKYC");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _userKYC.VendorFormName = _uploadFileResponse.FileName;
                        }
                    }
                }

                response = DataAccessLayer.UpdateUserKYCData(5,_userKYC);              
                if (response > 0)
                {
                    _userKYC.IsVerifiedKYC = true;
                    response = DataAccessLayer.UpdateUserKYCData(6, _userKYC);
                    TempData["successmessage"] = "Your file has been uploaded successfully";
                    //return RedirectToAction(nameof(MyAccount));
                    return RedirectToAction("UsersList", "Settings");
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
            string subject="Test", emailBody="This is test email", fromEmail= "crmsifsl@gmail.com", toEmail="mr.cmandal@gmail.com", ccEmail=null, bccEmail=null;
           bool a= sendEmail(subject, emailBody, fromEmail, toEmail, ccEmail, bccEmail);


            //MailMessage msg = new MailMessage();

            //msg.From = new MailAddress("crmsifsl@gmail.com");
            //msg.To.Add("mr.cmandal@gmail.com");
            //msg.Subject = "test";
            //msg.Body = "Test Content";
            ////msg.Priority = MailPriority.High;


            //using (SmtpClient client = new SmtpClient())
            //{
            //    client.EnableSsl = true;
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = new NetworkCredential("crmsifsl@gmail.com", "hkclryrjvlbfjcvy");
            //    client.Host = "smtp.gmail.com";
            //    client.Port = 587;
            //    client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //    client.Send(msg);
            //}


            return View(a);
        }
    }
}

//test
