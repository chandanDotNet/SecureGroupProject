using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System;
using SecureGroup.Library;

namespace SecureGroup.Controllers
{
    public class SettingsController : BaseController
    {

        private readonly ILogger<SettingsController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public SettingsController(ILogger<SettingsController> logger, MsDBContext context)
        {
            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }


        public IActionResult UsersList()
        {
            List<UserViewModel> _userViewModel = new List<UserViewModel>();            
            _userViewModel = _dataAccessLayer.GetAllUser(4, 0, 0).ToList(); //Get All type user (ActionId,UserId,RoleId)

            return View(_userViewModel);
        }
        public IActionResult AddUser()
        {
            UserViewModel _userViewModel = new UserViewModel();
            try
            {               
                string UserCode = GenerateEmployeeID(_userViewModel.Name); //Generate Employee code like- (CompanyName(SIFSL)\FY(2324)\UserName first char-number)

                var RoleList = _dataAccessLayerLinq.GetDropDownListData("UserRole", 0);
                _userViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _userViewModel.DepartmentList = _dataAccessLayerLinq.GetDropDownListData("Department", 0);
                _userViewModel.RoleList = RoleList.Where(x => x.Value == "1" || x.Value == "4").ToList();
                _userViewModel.UserCode = UserCode;

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_userViewModel);
        }


        [HttpPost]
        public IActionResult AddUser(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {
                             
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password); //Encrypt the password
                _userViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 1);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(UsersList));
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

            return RedirectToAction("AddEmployee");

        }


        public IActionResult EditUser(int Id)
        {
            UserViewModel _userViewModel = new UserViewModel();
            if (Id > 0)
            {
                // _userViewModel = _dataAccessLayerLinq.GetUserList(Id, 0).FirstOrDefault();
                _userViewModel = _dataAccessLayer.GetAllUser(4, Id, 0).FirstOrDefault();
                _userViewModel.RoleList = _dataAccessLayerLinq.GetDropDownListData("UserRole", 0);
                _userViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _userViewModel.DepartmentList = _dataAccessLayerLinq.GetDropDownListData("Department", 0);
                _userViewModel.Password = EncryptionLibrary.DecryptText(_userViewModel.Password);
            }

            return View(_userViewModel);
        }

        [HttpPost]
        public IActionResult EditUser(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {
                _userViewModel.CreatedBy = GetUserSession().UserId;
                //_userViewModel.RoleId = 4; //Employee
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password);
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 2);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(UsersList));
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

            return RedirectToAction("UsersList");

        }

        [HttpGet]
        public IActionResult DeleteUser(int Id)
        {
            int response = 0;
            UserViewModel _userViewModel = new UserViewModel();
            try
            {
                _userViewModel.UserId = Id;
                _userViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 3);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(UsersList));
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
            return RedirectToAction("UsersList");

        }

        public IActionResult UserKYCVerify(int Id)
        {
            UserKYC _userKYC = new UserKYC();
            if (Id > 0)
            {
                _userKYC.UserId = Id;
                var _user = _dataAccessLayer.GetAllUser(4, Id, 0).FirstOrDefault();
                if (_user != null)
                {
                    _userKYC.AadhaarCardName = _user.AadhaarCardName;
                    _userKYC.PanCardName = _user.PanCardName;
                    _userKYC.VoterCardName = _user.VoterCardName;
                }
            }
            return View(_userKYC);
        }

        

        public IActionResult UpdateKYCVerify(int Id)
        {
            int response = 0;
            UserKYC _userKYC = new UserKYC();            
            if (Id > 0)
            {
                _userKYC.UserId = Id;
                _userKYC.IsVerifiedKYC = true;
            }

            response = _dataAccessLayer.UpdateUserKYCData(6, _userKYC);
            if (response > 0)
            {
                TempData["successmessage"] = "Verified successfully";
                return RedirectToAction("UserKYCVerify",new { Id=Id });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction("UserKYCVerify", new { Id = Id });
        }

        public IActionResult UpdateKYCVerifyReject(int Id)
        {
            int response = 0;
            UserKYC _userKYC = new UserKYC();
            if (Id > 0)
            {
                _userKYC.UserId = Id;
                _userKYC.IsVerifiedKYC = false;
            }

            response = _dataAccessLayer.UpdateUserKYCData(6, _userKYC);
            if (response > 0)
            {
                TempData["successmessage"] = "Rejected successfully";
                return RedirectToAction("UserKYCVerify", new { Id = Id });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction("UserKYCVerify", new { Id = Id });
        }



        [HttpGet]
        public IActionResult AddDepartment()
        {
            DepartmentViewModel _departmentViewModel = new DepartmentViewModel();
            _departmentViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("User", 0);

            return View(_departmentViewModel);
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentViewModel _departmentViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayer.AddDepartmentData(_departmentViewModel, 8);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(DepartmentList));
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

            return RedirectToAction("AddDepartment");
        }

        [HttpGet]
        public IActionResult DepartmentList()
        {
            List<DepartmentViewModel> _departmentListViewModel = new List<DepartmentViewModel>();
            _departmentListViewModel = _dataAccessLayerLinq.GetDepartmentList(0);

            return View(_departmentListViewModel);
        }

        [HttpGet]
        public IActionResult EditDepartment(int Id)
        {
            DepartmentViewModel _departmentViewModel = new DepartmentViewModel();
            _departmentViewModel = _dataAccessLayerLinq.GetDepartmentList(Id).FirstOrDefault();
            _departmentViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("User", 0);

            return View(_departmentViewModel);
        }

        [HttpPost]
        public IActionResult EditDepartment(DepartmentViewModel _departmentViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayer.AddDepartmentData(_departmentViewModel, 9);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(DepartmentList));
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

            return RedirectToAction("EditDepartment");
        }

        [HttpGet]
        public IActionResult DeleteDepartment(DepartmentViewModel _departmentViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayer.AddDepartmentData(_departmentViewModel, 10);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(DepartmentList));
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

            return RedirectToAction("DepartmentList");
        }

        [HttpGet]
        public IActionResult RoleList()
        {
            List<UserRoleViewModel> _userRoleViewModel = new List<UserRoleViewModel>();
            _userRoleViewModel = _dataAccessLayerLinq.GetRoleList(0);
            return View(_userRoleViewModel);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            UserRoleViewModel _userRoleViewModel = new UserRoleViewModel();

            return View(_userRoleViewModel);
        }


        [HttpPost]
        public IActionResult AddRole(UserRoleViewModel _userRoleViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayerLinq.InsertRole(_userRoleViewModel);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(RoleList));
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

            return RedirectToAction("AddRole");

        }

        [HttpGet]
        public IActionResult EditRole(int Id)
        {
            UserRoleViewModel _userRoleViewModel = new UserRoleViewModel();
            _userRoleViewModel = _dataAccessLayerLinq.GetRoleList(Id).FirstOrDefault();

            return View(_userRoleViewModel);
        }

        [HttpPost]
        public IActionResult EditRole(UserRoleViewModel _userRoleViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayerLinq.UpdateRole(_userRoleViewModel, true);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(RoleList));
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

            return RedirectToAction("AddRole");

        }

        [HttpGet]
        public IActionResult DeleteRole(int Id)
        {
            int response = 0;

            try
            {
                UserRoleViewModel _userRoleViewModel = new UserRoleViewModel();
                _userRoleViewModel = _dataAccessLayerLinq.GetRoleList(Id).FirstOrDefault();
                response = _dataAccessLayerLinq.UpdateRole(_userRoleViewModel, false);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(RoleList));
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

            return RedirectToAction(nameof(RoleList));

        }

        


        [HttpGet]
        public JsonResult CreateUserCodeOnKey(string? Name)
        {
            string UserCode = string.Empty;

            UserCode = GenerateEmployeeID(Name);

            return Json(UserCode);
        }

        public string GenerateEmployeeID(string? Name)
        {
            string day, month, year, nextyear, ID;
            string hr, min, sec;
            string userId = "0", EmployeeID;
            day = DateTime.Now.Date.Day.ToString();
            month = DateTime.Now.Month.ToString();
            year = DateTime.Now.Year.ToString().Substring(2);
            nextyear = DateTime.Now.AddYears(1).ToString("yy");
            if (Name == null)
            {
                Name = "000";
            }
            else
            {
                Name = GetEmpNameForEmpCode(Name);
            }

            hr = DateTime.Now.Hour.ToString();
            min = DateTime.Now.Minute.ToString();
            sec = DateTime.Now.Second.ToString();

            var User = _dataAccessLayerLinq.GetLastUserDetails(0);
            if (User != null)
            {
                userId = User.UserId.ToString() + 1;
            }

            //EmployeeID = "SIFSL-" + day + month + year + hr + min + sec+"-"+ userId;
            EmployeeID = "SIFSL\\" + year + nextyear + "\\" + Name + "-" + userId;

            return EmployeeID;
        }

        public string GetEmpNameForEmpCode(String name)
        {
            var names = name.Split(' ');
            String _name = "";

            for (int i = 0; i != names.Length; i++)
            {

                //if (i != names.Length - 1)
                //{
                if (i == 0)
                {
                    _name = names[i].Substring(0, 1);
                }
                else
                {
                    _name = _name + "0";
                }
                //}
            }
            return _name; // Tony Stark is
        }
    }
}
