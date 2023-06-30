using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureGroup.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ILogger<EmployeeController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public EmployeeController(ILogger<EmployeeController> logger, MsDBContext context)
        {
            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }

        public IActionResult EmployeeList()
        {
            List<UserViewModel> _userViewModel = new List<UserViewModel>();
            _userViewModel = _dataAccessLayerLinq.GetUserList(0, 0);

            return View(_userViewModel);
        }
        public IActionResult AddEmployee()
        {

            UserViewModel _userViewModel = new UserViewModel();

            return View();
        }


        [HttpPost]
        public IActionResult AddEmployee(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {
                _userViewModel.CreatedBy = 1; //Get this data after login 
                _userViewModel.RoleId = 4; //Employee

                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 1);
                return RedirectToAction(nameof(EmployeeList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AddEmployee");


        }


        public IActionResult EditEmployee(int Id)
        {
            UserViewModel _userViewModel = new UserViewModel();
            if (Id > 0)
            {
                _userViewModel = _dataAccessLayerLinq.GetUserList(Id, 0).FirstOrDefault();                
            }

            return View(_userViewModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {
                _userViewModel.CreatedBy = 1; //Get this data after login 
                _userViewModel.RoleId = 4; //Employee
               
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 2);
                return RedirectToAction(nameof(EmployeeList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("EmployeeList");

        }

        [HttpGet]
        public IActionResult DeleteEmployee(int Id)
        {
            int response = 0;
            UserViewModel _userViewModel = new UserViewModel();

            _userViewModel.UserId = Id;
            _userViewModel.CreatedBy = 1; //Get this data after login 
            response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 3);

            return RedirectToAction("EmployeeList");

        }
    }
}
