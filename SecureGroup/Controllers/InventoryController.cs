using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SecureGroup.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer DataAccessLayer = null;
        DataAccessLayerLinq DataAccessLayerLinq = null;
        
        public InventoryController(ILogger<InventoryController> logger, MsDBContext context) { 
        
            _logger = logger;
            myDbContext = context;
            DataAccessLayer = new DataAccessLayer();
            DataAccessLayerLinq = new DataAccessLayerLinq(context);
        }
        public IActionResult WarehouseList()
        {
            //DataAccessLayerLinq DataAccessLayerLinq = new DataAccessLayerLinq();
            List<WarehouseViewModel> warehouseViewModel = new List<WarehouseViewModel>();
            var data = DataAccessLayerLinq.GetWarehouseList(1);
            //warehouseViewModel = (from Warehouse in myDbContext.Warehouse.Cast<WarehouseViewModel>()
            //      select Warehouse).ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult CreateWarehouse()
        {
            //DataAccessLayerLinq DataAccessLayerLinq = new DataAccessLayerLinq();
            WarehouseViewModel warehouseViewModel = new WarehouseViewModel();
            //var data = DataAccessLayerLinq.GetWarehouseList(1);
            //warehouseViewModel = (from Warehouse in myDbContext.Warehouse.Cast<WarehouseViewModel>()
            //      select Warehouse).ToList();

            var LinCL = DataAccessLayerLinq.GetDropDownListData("Country",0);
            warehouseViewModel.CountryList= LinCL;

            var LinSL = DataAccessLayerLinq.GetDropDownListData("State", 0);
            warehouseViewModel.StateList = LinSL;

            var LinCiL = DataAccessLayerLinq.GetDropDownListData("City", 0);
            warehouseViewModel.CityList = LinCiL;

            var LinUser = DataAccessLayerLinq.GetDropDownListData("User", 0);
            warehouseViewModel.UserList = LinUser;

            //warehouseViewModel.StateList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = "0"
            //});
            //warehouseViewModel.CityList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = "0"
            //});

            // warehouseViewModel.CountryList = (IList<SelectListItem>)CL.ToList();
            return View(warehouseViewModel);
        }

        //[HttpPost, ActionName("GetStateByCountryId")]
        [HttpGet]
        public JsonResult GetDropdownListDataById(string Id,string DropdownDataType)
        {
            int catId;
            List<SelectListItem> DdlMasterLists = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(Id))
            {
                catId = Convert.ToInt32(Id);
                DdlMasterLists = DataAccessLayerLinq.GetDropDownListData(DropdownDataType, catId);

            }
            return Json(DdlMasterLists);
        }

        [HttpGet]
        public JsonResult GetUserDetailsById(string Id)
        {
            int catId;
            UserDetailsViewModel UserData = new UserDetailsViewModel();
            if (!string.IsNullOrEmpty(Id))
            {
                catId = Convert.ToInt32(Id);
                UserData = DataAccessLayerLinq.GetUserDetails(catId);

            }
            return Json(UserData);
        }

        [HttpPost]
        public IActionResult CreateWarehouse(WarehouseViewModel warehouseViewModel)
        {
            try
            {
                // TODO: Add insert logic here
                DataAccessLayer.AddWarehouse(warehouseViewModel);

               // return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(warehouseViewModel);
        }




    }
}
