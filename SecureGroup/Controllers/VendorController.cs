using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Library;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureGroup.Controllers
{
    public class VendorController : Controller
    {

        private readonly ILogger<VendorController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public VendorController(ILogger<VendorController> logger, MsDBContext context)
        {
            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }


        public IActionResult VendorList()
        {
            List<UserViewModel> _userViewModel = new List<UserViewModel>();
            _userViewModel = _dataAccessLayerLinq.GetUserList(0, 0);

            return View(_userViewModel);
        }

        public IActionResult VendorDetails()
        {
            return View();
        }

        public IActionResult AddVendor()
        {
            UserViewModel _userViewModel = new UserViewModel();
            

            return View();
        }

        [HttpPost]
        public IActionResult AddVendor(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {                
                _userViewModel.CreatedBy = 1; //Get this data after login 
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password);
                if (_userViewModel.IsSupplier == true)
                {
                    _userViewModel.RoleId = 5;
                }
                else
                {
                    _userViewModel.RoleId = 2;
                }
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 1);
                return RedirectToAction(nameof(VendorList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AddVendor");

           
        }

        public IActionResult EditVendor(int Id)
        {
            UserViewModel _userViewModel = new UserViewModel();           
            if (Id > 0)
            {
                _userViewModel = _dataAccessLayer.GetAllUser(4,Id, 0).FirstOrDefault();
                _userViewModel.Password = EncryptionLibrary.DecryptText(_userViewModel.Password);
                if (_userViewModel.RoleId == 5)
                {
                    _userViewModel.IsSupplier = true;
                }
                else
                {
                    _userViewModel.IsSupplier = false;
                }
            }

            return View(_userViewModel);
        }

        [HttpPost]
        public IActionResult EditVendor(UserViewModel _userViewModel)
        {
            int response = 0;

            try
            {
                _userViewModel.CreatedBy = 1; //Get this data after login 
                _userViewModel.Password = EncryptionLibrary.EncryptText(_userViewModel.Password);
                if (_userViewModel.IsSupplier == true)
                {
                    _userViewModel.RoleId = 5;
                }
                else
                {
                    _userViewModel.RoleId = 2;
                }
                response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 2);
                return RedirectToAction(nameof(VendorList));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("EditVendor");

        }


        [HttpGet]
        public IActionResult DeleteVendor(int Id)
        {
            int response = 0;
            UserViewModel _userViewModel = new UserViewModel();

            _userViewModel.UserId = Id;
            _userViewModel.CreatedBy = 1; //Get this data after login 
            response = _dataAccessLayer.AddUpdateUserData(_userViewModel, 3);

            return RedirectToAction("VendorList");
            
        }

    }
}
