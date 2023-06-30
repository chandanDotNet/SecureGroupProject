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
    public class MasterController : Controller
    {


        private readonly ILogger<MasterController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public MasterController(ILogger<MasterController> logger, MsDBContext context)
        {

            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }


        public IActionResult MasterProductList()
        {
            List<ProductMasterViewModel> listproductMaster = new List<ProductMasterViewModel>();
            try
            {                
                listproductMaster = _dataAccessLayerLinq.GetMasterProductList(0).ToList();
            }catch (Exception ex)
            {
                throw new Exception();
            }
            return View(listproductMaster);
        }

        public IActionResult AddMasterProduct()
        {
            ProductMasterViewModel productMasterViewModel = new ProductMasterViewModel();
            productMasterViewModel.UnitList=_dataAccessLayerLinq.GetDropDownListData("Unit",0).ToList();

            return View(productMasterViewModel);
        }


        [HttpPost]
        public IActionResult AddMasterProduct(ProductMasterViewModel productMasterViewModel)
        {

            try
            {
                // TODO: Add insert logic here
               int Response= _dataAccessLayer.AddUpdateMasterProductData(productMasterViewModel, 2);
                return RedirectToAction(nameof(MasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }          

            return View(productMasterViewModel);
        }

        public IActionResult EditMasterProduct(int Id)
        {
            ProductMasterViewModel _productMasterViewModel = new ProductMasterViewModel();
            List<ProductMasterViewModel> _listproductMaster = new List<ProductMasterViewModel>();
            try
            {
                _listproductMaster = _dataAccessLayerLinq.GetMasterProductList(Id).ToList();
                if (_listproductMaster.Count > 0)
                {
                    _productMasterViewModel = _listproductMaster.FirstOrDefault();
                }
                
                _productMasterViewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return View(_productMasterViewModel);
        }


        [HttpPost]
        public IActionResult EditMasterProduct(ProductMasterViewModel productMasterViewModel)
        {

            try
            {
                // TODO: Add insert logic here
                int Response = _dataAccessLayer.AddUpdateMasterProductData(productMasterViewModel, 3);
                return RedirectToAction(nameof(MasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(productMasterViewModel);
        }

        public IActionResult DeleteMasterProduct(int Id)
        {
            ProductMasterViewModel _productMasterViewModel = new ProductMasterViewModel();
            _productMasterViewModel.ProductId = Id;
            try
            {
                // TODO: Add insert logic here
                int Response = _dataAccessLayer.AddUpdateMasterProductData(_productMasterViewModel, 4);
                return RedirectToAction(nameof(MasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(_productMasterViewModel);
        }

        public IActionResult SubMasterProductList()
        {
            List<SubProductMasterViewModel> _listSubproductMaster = new List<SubProductMasterViewModel>();
            try
            {
                _listSubproductMaster = _dataAccessLayerLinq.GetSubMasterProductList(0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return View(_listSubproductMaster);
        }

        public IActionResult AddSubMasterProduct()
        {
            SubProductMasterViewModel _subproductMasterViewModel = new SubProductMasterViewModel();
            _subproductMasterViewModel.ProductList= _dataAccessLayerLinq.GetDropDownListData("Product", 0).ToList();
            _subproductMasterViewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0).ToList();

            return View(_subproductMasterViewModel);
        }


        [HttpPost]
        public IActionResult AddSubMasterProduct(SubProductMasterViewModel subproductMasterViewModel)
        {

            try
            {
                // TODO: Add insert logic here
                int Response = _dataAccessLayer.AddUpdateMasterSubProductData(subproductMasterViewModel, 5);
                return RedirectToAction(nameof(SubMasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(subproductMasterViewModel);
        }

        public IActionResult EditSubMasterProduct(int Id)
        {
            SubProductMasterViewModel _subproductMasterViewModel = new SubProductMasterViewModel();

            List<SubProductMasterViewModel> _listSubproductMaster = new List<SubProductMasterViewModel>();
            try
            {
                _listSubproductMaster = _dataAccessLayerLinq.GetSubMasterProductList(Id).ToList();
                if(_listSubproductMaster.Count>0)
                {
                    _subproductMasterViewModel= _listSubproductMaster.FirstOrDefault();
                }
                _subproductMasterViewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0).ToList();
                _subproductMasterViewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }           

            return View(_subproductMasterViewModel);
        }


        [HttpPost]
        public IActionResult EditSubMasterProduct(SubProductMasterViewModel subproductMasterViewModel)
        {

            try
            {
                // TODO: Add insert logic here
                int Response = _dataAccessLayer.AddUpdateMasterSubProductData(subproductMasterViewModel, 6);
                return RedirectToAction(nameof(SubMasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(subproductMasterViewModel);
        }

        public IActionResult DeleteSubMasterProduct(int Id)
        {
            SubProductMasterViewModel subproductMasterViewModel=new SubProductMasterViewModel();
            subproductMasterViewModel.SubProductId = Id;
            try
            {
                // TODO: Add insert logic here
                int Response = _dataAccessLayer.AddUpdateMasterSubProductData(subproductMasterViewModel, 7);
                return RedirectToAction(nameof(SubMasterProductList));
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(subproductMasterViewModel);
        }



    }
}
