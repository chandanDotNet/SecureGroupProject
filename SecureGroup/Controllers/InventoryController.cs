using Azure;
using Microsoft.AspNetCore.Http;
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
    public class InventoryController : BaseController
    {
        private readonly ILogger<InventoryController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public InventoryController(ILogger<InventoryController> logger, MsDBContext context)
        {

            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }
        public IActionResult WarehouseList()
        {

            //DataAccessLayerLinq DataAccessLayerLinq = new DataAccessLayerLinq();
            List<WarehouseViewModel> warehouseViewModel = new List<WarehouseViewModel>();
            warehouseViewModel = _dataAccessLayerLinq.GetWarehouseList(0);
            //warehouseViewModel = (from Warehouse in myDbContext.Warehouse.Cast<WarehouseViewModel>()
            //      select Warehouse).ToList();
            WarehouseRapViewModel warehouseRapViewModel = new WarehouseRapViewModel();
            warehouseRapViewModel.warehouseViewModel = warehouseViewModel;

            var LinCL = _dataAccessLayerLinq.GetDropDownListData("Country", 0);
            warehouseRapViewModel.CountryList = LinCL;

            var LinSL = _dataAccessLayerLinq.GetDropDownListData("State", 0);
            warehouseRapViewModel.StateList = LinSL;

            var LinCiL = _dataAccessLayerLinq.GetDropDownListData("City", 0);
            warehouseRapViewModel.CityList = LinCiL;

            var LinUser = _dataAccessLayerLinq.GetDropDownListData("User", 0);
            warehouseRapViewModel.UserList = LinUser;


            return View(warehouseRapViewModel);
        }

        [HttpGet]
        public IActionResult CreateWarehouse(int Id)
        {
            //DataAccessLayerLinq DataAccessLayerLinq = new DataAccessLayerLinq();
            WarehouseViewModel warehouseViewModel = new WarehouseViewModel();
            //var data = DataAccessLayerLinq.GetWarehouseList(1);
            //warehouseViewModel = (from Warehouse in myDbContext.Warehouse.Cast<WarehouseViewModel>()
            //      select Warehouse).ToList();
            if (Id > 0)
            {
                warehouseViewModel = _dataAccessLayerLinq.GetWarehouseList(Id).FirstOrDefault();
            }

            var LinCL = _dataAccessLayerLinq.GetDropDownListData("Country", 0);
            warehouseViewModel.CountryList = LinCL;


            var LinSL = _dataAccessLayerLinq.GetDropDownListData("State", 101);
            warehouseViewModel.StateList = LinSL;

            var LinCiL = _dataAccessLayerLinq.GetDropDownListData("City", 0);
            warehouseViewModel.CityList = LinCiL;

            var LinUser = _dataAccessLayerLinq.GetDropDownListData("User", 0);
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
            // return PartialView("_CreateWarehouse", warehouseViewModel);
            return View(warehouseViewModel);
        }

        [HttpGet]
        public IActionResult EditWarehouse(int Id)
        {

            WarehouseViewModel warehouseViewModel = new WarehouseViewModel();
            try
            {
                if (Id > 0)
                {
                    warehouseViewModel = _dataAccessLayerLinq.GetWarehouseList(Id).FirstOrDefault();
                }

                var LinCL = _dataAccessLayerLinq.GetDropDownListData("Country", 0);
                warehouseViewModel.CountryList = LinCL;

                var LinSL = _dataAccessLayerLinq.GetDropDownListData("State", 101);
                warehouseViewModel.StateList = LinSL;

                var LinCiL = _dataAccessLayerLinq.GetDropDownListData("City", warehouseViewModel.StateId);
                warehouseViewModel.CityList = LinCiL;

                var LinUser = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                warehouseViewModel.UserList = LinUser;
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            //return PartialView("_CreateWarehouse", warehouseViewModel);
            return View(warehouseViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditWarehouse(WarehouseViewModel warehouseViewModel)
        {
            int response = 0;
            //if (ModelState.IsValid)
            //{
            try
            {
               
                response = _dataAccessLayer.AddWarehouse(warehouseViewModel, 7);               
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(WarehouseList));
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
            //}
            return RedirectToAction(nameof(WarehouseList));
            //return PartialView("_CreateWarehouse");
            
        }

        //[HttpPost, ActionName("GetStateByCountryId")]
        [HttpGet]
        public JsonResult GetDropdownListDataById(string Id, string DropdownDataType)
        {
            int catId;
            List<SelectListItem> DdlMasterLists = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(Id))
            {
                catId = Convert.ToInt32(Id);
                DdlMasterLists = _dataAccessLayerLinq.GetDropDownListData(DropdownDataType, catId);

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
                UserData = _dataAccessLayerLinq.GetUserDetails(catId);

            }
            return Json(UserData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWarehouse(WarehouseViewModel warehouseViewModel)
        {
            int response = 0;
            //if (ModelState.IsValid)
            //{
            try
            {
                // TODO: Add insert logic here
                response= _dataAccessLayer.AddWarehouse(warehouseViewModel, 1);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(WarehouseList));
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
            // }
            return RedirectToAction(nameof(WarehouseList));
            //return PartialView("_CreateWarehouse", warehouseViewModel);
            
        }


        public IActionResult WarehouseStock()
        {
            List<WHStockProductViewModel> _whProductList = new List<WHStockProductViewModel>();
            //_whProductList = _dataAccessLayerLinq.GetWarehouseProductStockList(0);
            _whProductList = _dataAccessLayer.GetWarehouseStock().ToList();

            return View(_whProductList);
        }

        public IActionResult WarehouseStockDetails(int WareHouseId)
        {
            List<WHStockProductDetailsViewModel> _whProductList = new List<WHStockProductDetailsViewModel>();
            //_whProductList = _dataAccessLayerLinq.GetWarehouseProductStockList(0);
            _whProductList = _dataAccessLayer.GetWarehouseStockDetails(WareHouseId).ToList();
            if (_whProductList.Count > 0)
            {
                TempData["WareHouseName"] = _whProductList[0].WareHouseName;
            }

            return View(_whProductList);
        }

        public IActionResult WHProductPurchaseDetails(int WareHouseId,int ProductId,int SubProductId)
        {
            List<WHProductPurchaseInOutDetailsViewModel> _wHPurchaseInOutDetailsList = new List<WHProductPurchaseInOutDetailsViewModel>();
            _wHPurchaseInOutDetailsList= _dataAccessLayer.GetWarehouseStockPurchaseInOutDetails(3,WareHouseId,ProductId,SubProductId).ToList();

            //List<WHStockProductDetailsViewModel> _whProductList = new List<WHStockProductDetailsViewModel>();
            ////_whProductList = _dataAccessLayerLinq.GetWarehouseProductStockList(0);
            //_whProductList = _dataAccessLayer.GetWarehouseStockDetails(WareHouseId).ToList();
            //if (_whProductList.Count > 0)
            //{
            //    TempData["WareHouseName"] = _whProductList[0].WareHouseName;
            //}

            return View(_wHPurchaseInOutDetailsList);
        }
        public IActionResult WHProductStockInDetails(int WareHouseId, int ProductId, int SubProductId)
        {
            List<WHProductPurchaseInOutDetailsViewModel> _wHPurchaseInOutDetailsList = new List<WHProductPurchaseInOutDetailsViewModel>();
            _wHPurchaseInOutDetailsList = _dataAccessLayer.GetWarehouseStockPurchaseInOutDetails(4, WareHouseId, ProductId, SubProductId).ToList();          

            return View(_wHPurchaseInOutDetailsList);
        }
        public IActionResult WHProductStockOutDetails(int WareHouseId, int ProductId, int SubProductId)
        {
            List<WHProductPurchaseInOutDetailsViewModel> _wHPurchaseInOutDetailsList = new List<WHProductPurchaseInOutDetailsViewModel>();
            _wHPurchaseInOutDetailsList = _dataAccessLayer.GetWarehouseStockPurchaseInOutDetails(5, WareHouseId, ProductId, SubProductId).ToList();

            return View(_wHPurchaseInOutDetailsList);
        }

        public IActionResult DeleteWHProductStockPurchaseDetails(int Id, int WareHouseId, int ProductId, int SubProductId)
        {
            int Response = 0;
            Response = _dataAccessLayer.DeleteWarehouseStockPurchaseInOutData(6, Id);

            //return RedirectToAction("WHProductPurchaseDetails", new { WareHouseId = WareHouseId, ProductId= ProductId, SubProductId= SubProductId });
            return RedirectToAction("WarehouseStockDetails", new { WareHouseId = WareHouseId, ProductId= ProductId, SubProductId= SubProductId });
        }
        public IActionResult DeleteWHProductStockInDetails(int Id, int WareHouseId, int ProductId, int SubProductId)
        {
            int Response = 0;
            Response = _dataAccessLayer.DeleteWarehouseStockPurchaseInOutData(7, Id);

            //return RedirectToAction("WHProductStockInDetails", new { WareHouseId = WareHouseId, ProductId = ProductId, SubProductId = SubProductId });
            return RedirectToAction("WarehouseStockDetails", new { WareHouseId = WareHouseId, ProductId = ProductId, SubProductId = SubProductId });
        }
        public IActionResult DeleteWHProductStockOutDetails(int Id, int WareHouseId, int ProductId, int SubProductId)
        {
            int Response = 0;
            Response = _dataAccessLayer.DeleteWarehouseStockPurchaseInOutData(7, Id);

            //return RedirectToAction("WHProductStockOutDetails", new { WareHouseId = WareHouseId, ProductId = ProductId, SubProductId = SubProductId });
            return RedirectToAction("WarehouseStockDetails", new { WareHouseId = WareHouseId, ProductId = ProductId, SubProductId = SubProductId });
        }

        [HttpGet]
        public IActionResult Abc()
        {

            return View();
        }


        [HttpGet]
        public IActionResult Transferproduct()
        {

            WHStockTransferManageViewModel wHStockTransferManageViewModel = new WHStockTransferManageViewModel();
            wHStockTransferManageViewModel.WareHouseList = _dataAccessLayerLinq.GetDropDownListData("WareHouse", 0);
            wHStockTransferManageViewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            wHStockTransferManageViewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("SubProduct", 0);
            wHStockTransferManageViewModel.TransferTypeList = _dataAccessLayerLinq.GetDropDownListData("TransferType", 0);
            wHStockTransferManageViewModel.TransferDate = DateTime.Now;

            return View(wHStockTransferManageViewModel);
        }

        [HttpGet]
        public JsonResult GetSubProductDetailsById(string Id)
        {
            int _Id;
            SubProductMasterViewModel SubProduct = new SubProductMasterViewModel();
            if (!string.IsNullOrEmpty(Id))
            {
                _Id = Convert.ToInt32(Id);
                SubProduct = _dataAccessLayerLinq.GetSubProductMaster(_Id);

            }
            return Json(SubProduct);
        }

        [HttpGet]
        public JsonResult CheckProductAvailable(int TransferType, int Source, int Destination, int ProductId, int SubProduct, decimal Quantity)
        {
            int _Id;
            WHStockTransferManageViewModel WHStockTransfer = new WHStockTransferManageViewModel
            {
                TransferType = TransferType,
                SourceId = Source,
                DestinationId = Destination,
                ProductId = ProductId,
                SubProductId = SubProduct,
                TransferQuantity = Quantity

            };

            decimal Response = _dataAccessLayer.TransferProductQuantityVerify(WHStockTransfer);
            if (Response > 0)
            {

            }


            return Json(Response);
        }

        [HttpPost]
        public IActionResult Transferproduct(WHStockTransferManageViewModel wHStockTransferManageViewModel)
        {
            int response = 0;
            try
            {
                wHStockTransferManageViewModel.TransferBy = GetUserSession().UserId;
                 response = _dataAccessLayer.TransferProductItem(wHStockTransferManageViewModel);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Transferproduct));
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



            return View(wHStockTransferManageViewModel);

        }

        [HttpGet]
        public IActionResult AddProductItem()
        {
            WHStockProductViewModel wHStockProduct = new WHStockProductViewModel();
            wHStockProduct.WareHouseList = _dataAccessLayerLinq.GetDropDownListData("WareHouse", 0);
            wHStockProduct.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            wHStockProduct.SubProductList = _dataAccessLayerLinq.GetDropDownListData("SubProduct", 0);
            wHStockProduct.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            wHStockProduct.SupplierList = _dataAccessLayerLinq.GetDropDownListData("Supplier", 0);

            return View(wHStockProduct);
            //return PartialView("_AddProductWH", wHStockProduct);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddProductItem(WHStockProductViewModel wHStockProduct)
        {
            int response = 0;
            //if (ModelState.IsValid)
            //{
            try
            {
                // TODO: Add insert logic here
                response= _dataAccessLayer.AddProductItem(wHStockProduct, 2);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(WarehouseStock));
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

            //}
            //else
            //{
            //    wHStockProduct.WareHouseList = _dataAccessLayerLinq.GetDropDownListData("WareHouse", 0);
            //    wHStockProduct.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            //    wHStockProduct.SubProductList = _dataAccessLayerLinq.GetDropDownListData("SubProduct", 0);
            //    wHStockProduct.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            //    wHStockProduct.SupplierList = _dataAccessLayerLinq.GetDropDownListData("Supplier", 0);
            //}               
            //return View(wHStockProduct);
            return RedirectToAction("WarehouseStock");
        }



        [HttpGet]
        public IActionResult EditProductItem(int Id)
        {
            WHStockProductViewModel wHStockProduct = new WHStockProductViewModel();


            if (Id != null || Id != 0)
            {
                wHStockProduct = _dataAccessLayerLinq.GetWarehouseProductStockDetails((int)Id);
            }
            wHStockProduct.WareHouseList = _dataAccessLayerLinq.GetDropDownListData("WareHouse", 0);
            wHStockProduct.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            wHStockProduct.SubProductList = _dataAccessLayerLinq.GetDropDownListData("SubProduct", wHStockProduct.ProductId);
            wHStockProduct.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            wHStockProduct.SupplierList = _dataAccessLayerLinq.GetDropDownListData("Supplier", 0);

            return View(wHStockProduct);
            //return PartialView("_AddProductWH", wHStockProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProductItem(WHStockProductViewModel wHStockProduct)
        {
            int response = 0;
            //if (ModelState.IsValid)
            //{
            try
            {
                // TODO: Add insert logic here
                response= _dataAccessLayer.AddProductItem(wHStockProduct, 5);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(WarehouseStock));
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

            return RedirectToAction(nameof(WarehouseStock));
        }

        [HttpGet]
        public IActionResult DeleteProductItem(int Id)
        {
            int response = 0;
            WHStockProductViewModel wHStockProduct = new WHStockProductViewModel();

            wHStockProduct.Id = Id;
            response= _dataAccessLayer.AddProductItem(wHStockProduct, 6);
            if (response > 0)
            {
                TempData["successmessage"] = "Your data has been deleted successfully";
                return RedirectToAction(nameof(WarehouseStock));
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction("WarehouseStock");
            //return PartialView("_AddProductWH", wHStockProduct);
        }



    }
}
