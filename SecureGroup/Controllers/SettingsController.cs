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
    public class SettingsController : Controller
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
            //_userViewModel = _dataAccessLayerLinq.GetUserList(0, 0);
            _userViewModel = _dataAccessLayer.GetAllUser(4, 0,0).ToList();

            return View(_userViewModel);
        }
        public IActionResult AddUser()
        {
            UserViewModel _userViewModel = new UserViewModel();
            try
            {
                //**************************
                string UserCode = GenerateEmployeeID();


                var RoleList = _dataAccessLayerLinq.GetDropDownListData("UserRole", 0);
                _userViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _userViewModel.DepartmentList = _dataAccessLayerLinq.GetDropDownListData("Department", 0);
                _userViewModel.RoleList = RoleList.Where(x => x.Value == "1" || x.Value == "4").ToList();
                _userViewModel.UserCode= UserCode;

            }
            catch(Exception ex)
            {
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
                _userViewModel.CreatedBy = 1; //Get this data after login 
                //_userViewModel.RoleId = 4; //Employee
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password);

                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 1);
                return RedirectToAction(nameof(UsersList));
            }
            catch (Exception ex)
            {
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
                _userViewModel.CreatedBy = 1; //Get this data after login 
                                              //_userViewModel.RoleId = 4; //Employee
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password);
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 2);
                return RedirectToAction(nameof(UsersList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("UsersList");

        }

        [HttpGet]
        public IActionResult DeleteUser(int Id)
        {
            int response = 0;
            UserViewModel _userViewModel = new UserViewModel();

            _userViewModel.UserId = Id;
            _userViewModel.CreatedBy = 1; //Get this data after login 
            response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 3);

            return RedirectToAction("UsersList");

        }

        [HttpGet]
        public IActionResult AddDepartment()
        {
            DepartmentViewModel _departmentViewModel = new DepartmentViewModel();
            _departmentViewModel.UserList= _dataAccessLayerLinq.GetDropDownListData("User", 0);

            return View(_departmentViewModel);
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentViewModel _departmentViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayer.AddDepartmentData(_departmentViewModel, 8);
                return RedirectToAction(nameof(DepartmentList));
            }
            catch (Exception ex)
            {
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
                return RedirectToAction(nameof(DepartmentList));
            }
            catch (Exception ex)
            {
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
                return RedirectToAction(nameof(DepartmentList));
            }
            catch (Exception ex)
            {
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
                return RedirectToAction(nameof(RoleList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AddRole");
           
        }

        [HttpGet]
        public IActionResult EditRole(int Id)
        {
            UserRoleViewModel _userRoleViewModel = new UserRoleViewModel();
            _userRoleViewModel=_dataAccessLayerLinq.GetRoleList(Id).FirstOrDefault();

            return View(_userRoleViewModel);
        }

        [HttpPost]
        public IActionResult EditRole(UserRoleViewModel _userRoleViewModel)
        {
            int response = 0;

            try
            {
                response = _dataAccessLayerLinq.UpdateRole(_userRoleViewModel,true);
                return RedirectToAction(nameof(RoleList));
            }
            catch (Exception ex)
            {
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
                return RedirectToAction(nameof(RoleList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(RoleList));

        }


        public string GenerateEmployeeID()
        {
            string day, month, year, ID;
            string hr, min, sec;
            string userId="0", EmployeeID;
            day = DateTime.Now.Date.Day.ToString();
            month = DateTime.Now.Month.ToString();
            year = DateTime.Now.Year.ToString();

            hr = DateTime.Now.Hour.ToString();
            min = DateTime.Now.Minute.ToString();
            sec = DateTime.Now.Second.ToString();

            var User = _dataAccessLayerLinq.GetLastUserDetails(0);
            if(User != null)
            {
                userId = User.UserId.ToString();
            }

            EmployeeID = "SG-" + day + month + year + hr + min + sec+"-"+ userId;

            return EmployeeID;
        }
    }
}
