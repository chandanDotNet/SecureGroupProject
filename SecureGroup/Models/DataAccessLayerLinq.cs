using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureGroup.DBContexts;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;

namespace SecureGroup.Models
{
    public class DataAccessLayerLinq 
    {
        private MsDBContext myDbContext;

        public DataAccessLayerLinq(MsDBContext context) 
        {
            myDbContext = context;
        }

        //public DataAccessLayerLinq(DbContextOptions<MsDBContext> options, MsDBContext context) : base(options)
        //{
        //    myDbContext = context;
        //}

        public List<UserViewModel> GetUserList(int Id,int RoleId)
        {
            List<UserViewModel> _userViewModel = new List<UserViewModel>();            

            if (Id > 0)
            {
                _userViewModel = (from user in myDbContext.User.Cast<User>()                              
                                  join detail in myDbContext.Department on (int?)user.DepartmentId  equals (int?)detail.DepartmentId into Details
                                  from m in Details.DefaultIfEmpty()
                                  join role in myDbContext.UserRole on user.RoleId equals role.UserRoleId into Roles
                                  from role in Roles.DefaultIfEmpty()
                                  where user.UserId == Id && user.IsActive == true
                                  select new UserViewModel
                                  {
                                      UserId = user.UserId,
                                      Name = user.Name,
                                      Email = user.Email,
                                      ContactNo = user.ContactNo,
                                      BusinessName = user.BusinessName,
                                      RoleName = role.RoleName == null ? "N/A" : role.RoleName,
                                      RoleId = user.RoleId==null?0: user.RoleId,
                                      Password = user.Password,
                                      CreatedBy = user.CreatedBy==null?0: user.CreatedBy,
                                      CreatedDate = user.CreatedDate,
                                      DepartmentName = m.DepartmentName == null ? "N/A" : m.DepartmentName,
                                      DepartmentId = user.DepartmentId == null ? 0 : user.DepartmentId,
                                      ReportingTo = user.ReportingTo == null ? 0 : user.ReportingTo,
                                      LoginTime = user.LoginTime == null ? "00:00" : user.LoginTime,
                                      LogoutTime = user.LogoutTime == null ? "00:00" : user.LogoutTime,
                                      UserCode = user.UserCode == null ? "0" : user.UserCode,
                                      JobTitle = user.JobTitle == null ? "00" : user.JobTitle
                                  }).ToList();
            }
            else if(RoleId>0)
            {
                _userViewModel = (from user in myDbContext.User.Cast<User>()
                                  where user.IsActive == true && user.RoleId == RoleId
                                  select new UserViewModel
                                  {
                                      UserId = user.UserId,
                                      Name = user.Name,
                                      Email = user.Email,
                                      ContactNo = user.ContactNo,
                                      BusinessName = user.BusinessName,
                                      RoleName = user.BusinessName,
                                      RoleId = user.RoleId,
                                      Password = user.Password,
                                      CreatedBy = user.CreatedBy,
                                      CreatedDate = user.CreatedDate
                                  }).ToList();
            }
            else
            {
                _userViewModel = (from user in myDbContext.User.Cast<User>()
                                  join detail in myDbContext.Department on user.DepartmentId ==null? 0: user.DepartmentId equals detail.DepartmentId into Details
                                  from m in Details.DefaultIfEmpty()
                                  join role in myDbContext.UserRole on user.RoleId equals role.UserRoleId into Roles
                                  from r in Roles.DefaultIfEmpty()
                                  where user.IsActive == true
                                  select new UserViewModel
                                  {
                                      UserId = user.UserId,
                                      Name = user.Name,
                                      Email = user.Email,
                                      ContactNo = user.ContactNo,
                                      BusinessName = user.BusinessName,
                                      RoleName = r.RoleName == null ? "N/A" : r.RoleName,
                                      RoleId = user.RoleId,
                                      Password = user.Password,
                                      CreatedBy = user.CreatedBy,
                                      CreatedDate = user.CreatedDate,
                                      DepartmentName= m.DepartmentName == null ? "N/A" : m.DepartmentName,
                                      DepartmentId = user.DepartmentId
                                  }).ToList();
            }

            return _userViewModel;
        }

        public UserViewModel GetLastUserDetails(int Id)
        {
            UserViewModel _userViewModel = new UserViewModel();
           
                _userViewModel = (from user in myDbContext.User.Cast<User>()
                                  where user.IsActive == true
                                  orderby user.UserId descending
                                  select new UserViewModel
                                  {
                                      UserId = user.UserId,
                                      Name = user.Name,
                                      Email = user.Email,
                                      ContactNo = user.ContactNo,
                                      BusinessName = user.BusinessName,
                                      RoleName = user.BusinessName,
                                      RoleId = user.RoleId,
                                      Password = user.Password,
                                      CreatedBy = user.CreatedBy,
                                      CreatedDate = user.CreatedDate
                                  }).FirstOrDefault();          

            return _userViewModel;
        }

        public List<WarehouseViewModel> GetWarehouseList(int Id)
        {
            List<WarehouseViewModel> warehouseViewModel = new List<WarehouseViewModel>();

            //warehouseViewModel = (from Warehouse in myDbContext.WareHouse.Cast<WarehouseViewModel>()
            //                      select Warehouse).ToList();

            //var a =  (from city in myDbContext.CityMaster.Cast<CityMaster>()
            //           select city).ToList();

            if(Id>0)
            {
                warehouseViewModel = (from wh in myDbContext.WareHouse.Cast<WareHouse>()
                                      join ad in myDbContext.User on wh.WareHouseAdminId equals ad.UserId
                                      where wh.WarehouseId == Id
                                      select new WarehouseViewModel
                                      {
                                          WarehouseId = wh.WarehouseId,
                                          WarehouseName = wh.WarehouseName,
                                          Address = wh.Address,
                                          CityId = wh.CityId,                                         
                                          StateId = wh.StateId,                                         
                                          ZipCode = wh.ZipCode,
                                          Block = wh.Block,
                                          CountryId = wh.CountryId,
                                          UserId=wh.WareHouseAdminId,
                                          AdminName = ad.Name,
                                          AdminContactNo = ad.ContactNo,
                                          AdminEmailId = ad.Email,

                                      }).ToList();
            }
            else
            {
                warehouseViewModel = (from wh in myDbContext.WareHouse.Cast<WareHouse>()
                                      join ad in myDbContext.User on wh.WareHouseAdminId equals ad.UserId 
                                      join c in myDbContext.CountryMaster on wh.CountryId equals c.ID
                                      join s in myDbContext.StateMaster on wh.StateId equals s.ID
                                      join ct in myDbContext.CityMaster on wh.CityId equals ct.ID
                                      where wh.IsActive == true
                                      orderby wh.WarehouseId
                                      select new WarehouseViewModel
                                      {
                                          WarehouseId = wh.WarehouseId,
                                          WarehouseName = wh.WarehouseName,
                                          Address = wh.Address,
                                          CityId = wh.CityId,
                                          City = ct.Name,
                                          StateId = wh.StateId,
                                          State = s.Name,
                                          ZipCode = wh.ZipCode,
                                          Block = wh.Block,
                                          CountryId = wh.CountryId,
                                          Country = c.Name,
                                          AdminName = ad.Name,
                                          AdminContactNo = ad.ContactNo,
                                          AdminEmailId = ad.Email,
                                      }).ToList();
            }

           

           

            return warehouseViewModel;
        }

        public List<SelectListItem> GetDropDownListData(string DDName, int? Id,string DDType)
        {
            var list = new List<SelectListItem>();

            if (DDName == "SysVal")
            {
                list = (from sysVal in myDbContext.SysVal
                        where sysVal.IsActive == true && sysVal.Type== DDType
                        orderby sysVal.Value ascending
                        select new SelectListItem()
                        {
                            Text = sysVal.Value,
                            Value = sysVal.Id.ToString(),
                        }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            return list;

        }

        public List<SelectListItem> GetDropDownListData(string DDName,int? Id)
        {

            var list = new List<SelectListItem>();
            

            if (DDName == "Country")
            {
                list = (from country in myDbContext.CountryMaster
                        orderby country.Name ascending
                        select new SelectListItem()
                                   {
                                       Text = country.Name,
                                       Value = country.ID.ToString(),
                                       Selected= true,
                                       Disabled= true 
                                       
                                       
,                                   }).ToList();
                
            }

            if (DDName == "City")
            {
                list = (from city in myDbContext.CityMaster where city.StateID == Id
                        orderby city.Name ascending
                        select new SelectListItem()
                        {
                            Text = city.Name,
                            Value = city.ID.ToString(),
                        }).ToList();
            }

            if (DDName == "State")
            {
                list = (from state in myDbContext.StateMaster where state.CountryID == Id 
                        orderby state.Name ascending
                        select new SelectListItem()
                        {
                            Text = state.Name,
                            Value = state.ID.ToString(),
                        }).ToList();
            }

            if (DDName == "User")
            {
                list = (from user in myDbContext.User
                        where user.IsActive == true
                        //where user.RoleId == Id
                        select new SelectListItem()
                        {
                            Text = user.Name,
                            Value = user.UserId.ToString(),
                        }).ToList();
            }
            if (DDName == "UserByRole")
            {
                list = (from user in myDbContext.User
                        where user.IsActive == true
                        where user.RoleId == Id
                        select new SelectListItem()
                        {
                            Text = user.Name,
                            Value = user.UserId.ToString(),
                        }).ToList();
            }

            if (DDName == "Product")
            {
                list = (from product in myDbContext.ProductMaster
                        where product.IsActive== true
                        orderby product.ProductName ascending
                        //where user.RoleId == Id
                        select new SelectListItem()
                        {
                            Text = product.ProductName,
                            Value = product.ProductId.ToString(),
                        }).ToList();
            }
            if (DDName == "SubProduct")
            {
                list = (from subproduct in myDbContext.SubProductMaster
                            where subproduct.ProductId == Id && subproduct.IsActive== true
                        orderby subproduct.SubProductName ascending
                        select new SelectListItem()
                        {
                            Text = subproduct.SubProductName,
                            Value = subproduct.SubProductId.ToString(),
                        }).ToList();
            }
            if (DDName == "AllSubProduct")
            {
                list = (from subproduct in myDbContext.SubProductMaster
                        where  subproduct.IsActive == true
                        orderby subproduct.SubProductName ascending
                        select new SelectListItem()
                        {
                            Text = subproduct.SubProductName,
                            Value = subproduct.SubProductId.ToString(),
                        }).ToList();
            }

            if (DDName == "WareHouse")
            {
                list = (from wareHouse in myDbContext.WareHouse
                        where wareHouse.IsActive== true
                        orderby wareHouse.WarehouseName ascending
                        //where subproduct.ProductId == Id
                        select new SelectListItem()
                        {
                            Text = wareHouse.WarehouseName,
                            Value = wareHouse.WarehouseId.ToString(),
                        }).ToList();
            }

            if (DDName == "Unit")
            {
                list = (from unit in myDbContext.UnitMaster
                        where unit.IsActive== true
                        select new SelectListItem()
                        {
                            Text = unit.UnitName,
                            Value = unit.UnitId.ToString(),
                        }).ToList();
            }

            if (DDName == "Supplier")
            {             

                list = (from user in myDbContext.User
                        where user.IsActive == true
                        where user.RoleId == 5
                        select new SelectListItem()
                        {
                            Text = user.BusinessName,
                            Value = user.UserId.ToString(),
                        }).ToList();
            }
            if (DDName == "Vendor")
            {

                list = (from user in myDbContext.User
                        where user.IsActive == true
                        where user.RoleId == 2
                        select new SelectListItem()
                        {
                            Text = user.BusinessName,
                            Value = user.UserId.ToString(),
                        }).ToList();
            }
            if (DDName == "Vendor&Supplier")
            {
                var roleId = new int[] { 2, 5 };
                list = (from user in myDbContext.User
                        where user.IsActive == true
                        where roleId.Contains(user.RoleId)
                        //where user.RoleId in( 2)
                        select new SelectListItem()
                        {
                            Text = user.BusinessName,
                            Value = user.UserId.ToString(),
                        }).ToList();


            }

            if (DDName == "TransferType")
            {
                list = (from transferType in myDbContext.TransferTypeMaster
                        where transferType.IsActive== true
                        select new SelectListItem()
                        {
                            Text = transferType.TransferType,
                            Value = transferType.Id.ToString(),
                        }).ToList();
            }

            if (DDName == "Site")
            {
                list = (from site in myDbContext.Site
                        where site.IsActive== true
                        orderby site.SiteName ascending                       
                        select new SelectListItem()
                        {
                            Text = site.SiteName,
                            Value = site.SiteId.ToString(),
                        }).ToList();
            }

            if (DDName == "UserRole")
            {
                list = (from role in myDbContext.UserRole
                        where role.IsActive == true
                        orderby role.RoleName ascending
                        select new SelectListItem()
                        {
                            Text = role.RoleName,
                            Value = role.UserRoleId.ToString(),
                        }).ToList();
            }
            if (DDName == "Department")
            {
                list = (from role in myDbContext.Department
                        where role.IsActive == true
                        orderby role.DepartmentName ascending
                        select new SelectListItem()
                        {
                            Text = role.DepartmentName,
                            Value = role.DepartmentId.ToString(),
                        }).ToList();
            }
            if (DDName == "TaskStatus")
            {
                list = (from sysVal in myDbContext.SysVal
                        where sysVal.IsActive == true && sysVal.Type== "TaskStatus"
                        orderby sysVal.Value ascending
                        select new SelectListItem()
                        {
                            Text = sysVal.Value,
                            Value = sysVal.Id.ToString(),
                        }).ToList();
            }

            if (DDName == "Project")
            {
                list = (from project in myDbContext.Project
                        where project.IsActive == true
                        orderby project.ProjectName ascending
                        select new SelectListItem()
                        {
                            Text = project.ProjectName,
                            Value = project.ProjectId.ToString(),
                        }).ToList();
            }
            if (DDName == "Scheme")
            {
                list = (from scheme in myDbContext.Scheme
                        where scheme.IsActive == true
                        orderby scheme.SchemeName ascending
                        select new SelectListItem()
                        {
                            Text = scheme.SchemeName,
                            Value = scheme.SchemeId.ToString(),
                        }).ToList();
            }
            if (DDName == "Year")
            {
                list.Insert(0, new SelectListItem()
                {
                    Text = "2022",
                    Value = "1"
                });
                list.Insert(0, new SelectListItem()
                {
                    Text = "2023",
                    Value = "2"
                });

            }
            if (DDName == "Month")
            {
                list.Insert(0, new SelectListItem()
                {
                    Text = "Jun",
                    Value = "1"
                });
                list.Insert(0, new SelectListItem()
                {
                    Text = "July",
                    Value = "2"
                });

            }

            list.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            return list;

        }


        public UserDetailsViewModel GetUserDetails(int Id)
        {
            UserDetailsViewModel userDetails = new UserDetailsViewModel();           

            userDetails = (from user in myDbContext.User.Cast<User>()
                           where user.UserId == Id
                           select new UserDetailsViewModel
                           {
                               UserId = user.UserId,
                               Name = user.Name,
                               Email = user.Email,
                               ContactNo = user.ContactNo,
                           }).FirstOrDefault();

            //userDetails = (from wh in myDbContext.User.Cast<UserViewModel>()
            //                      join ad in myDbContext.WareHouseAdminDetails on wh.UserId equals ad.UserId
            //                      where ad.UserId == Id

            //                      select new UserDetailsViewModel
            //                      {
            //                          UserId = wh.UserId,
            //                          Name = wh.Name,
            //                          Email = wh.Email,
            //                          ContactNo = ad.ContactNumber,                                     
            //                      }).FirstOrDefault();

            return userDetails;
        }

        public List<WHStockProductViewModel> GetWarehouseProductStockList(int Id)
        {
            List<WHStockProductViewModel> warehouseViewModel = new List<WHStockProductViewModel>();

            //warehouseViewModel = (from Warehouse in myDbContext.WareHouse.Cast<WarehouseViewModel>()
            //                      select Warehouse).ToList();

            //var a = (from city in myDbContext.CityMaster.Cast<CityMaster>()
            //         select city).ToList();

            warehouseViewModel = (from sp in myDbContext.WHStockProduct.Cast<WHStockProduct>()
                                  join wh in myDbContext.WareHouse on sp.WareHouseId equals wh.WarehouseId
                                  join pm in myDbContext.ProductMaster on sp.ProductId equals pm.ProductId
                                  join spm in myDbContext.SubProductMaster on sp.SubProductId equals spm.SubProductId
                                  join u in myDbContext.UnitMaster on sp.UnitId equals u.UnitId
                                  join s in myDbContext.SupplierDetails on sp.SupplierId equals s.SupplierId
                                  where sp.IsActive == true
                                  orderby sp.Id descending
                                  select new WHStockProductViewModel
                                  {
                                      Id = sp.Id,
                                      WareHouse = wh.WarehouseName,
                                      ProductName = pm.ProductName,
                                      SubProduct = spm.SubProductName,
                                      Quantity = sp.Quantity,
                                      Unit = u.UnitName,
                                      Amount = sp.Amount,
                                      SupplierName = s.SupplierName,
                                      SupplierContactDetails = s.ContactName + "/" + s.ContactNumber
                                  }).ToList();



            return warehouseViewModel;
        }

        public WHStockProductViewModel GetWarehouseProductStockDetails(int Id)
        {
            WHStockProductViewModel warehouseViewModel = new WHStockProductViewModel();

            //warehouseViewModel = (from Warehouse in myDbContext.WareHouse.Cast<WarehouseViewModel>()
            //                      select Warehouse).ToList();

            //var a = (from city in myDbContext.CityMaster.Cast<CityMaster>()
            //         select city).ToList();

            warehouseViewModel = (from sp in myDbContext.WHStockProduct.Cast<WHStockProduct>()
                                  join wh in myDbContext.WareHouse on sp.WareHouseId equals wh.WarehouseId
                                  join pm in myDbContext.ProductMaster on sp.ProductId equals pm.ProductId
                                  join spm in myDbContext.SubProductMaster on sp.SubProductId equals spm.SubProductId
                                  join u in myDbContext.UnitMaster on sp.UnitId equals u.UnitId
                                  join s in myDbContext.SupplierDetails on sp.SupplierId equals s.SupplierId
                                  where sp.Id == Id
                                  orderby sp.Id descending
                                  select new WHStockProductViewModel
                                  {
                                      Id = sp.Id,
                                      WareHouse = wh.WarehouseName,
                                      ProductName = pm.ProductName,
                                      SubProduct = spm.SubProductName,
                                      Quantity = sp.Quantity,
                                      Unit = u.UnitName,
                                      Amount = sp.Amount,
                                      SupplierName = s.SupplierName,
                                      SupplierContactDetails = s.ContactName + "/" + s.ContactNumber,
                                      UnitPrice = sp.UnitPrice,
                                      CreatedBy = sp.CreatedBy,
                                      ProductId = sp.ProductId,
                                      SubProductId = sp.SubProductId,
                                      UnitId = sp.UnitId,
                                      SupplierId = sp.SupplierId,
                                      WareHouseId = sp.WareHouseId
                                  }).FirstOrDefault();



            return warehouseViewModel;
        }
        public SubProductMasterViewModel GetSubProductMaster(int id)
        {

            SubProductMasterViewModel subProductMaster = new SubProductMasterViewModel();           

            subProductMaster = (from spm in myDbContext.SubProductMaster.Cast<SubProductMaster>()
                                join um in myDbContext.UnitMaster on spm.UnitId equals um.UnitId
                                join pm in myDbContext.ProductMaster on spm.ProductId equals pm.ProductId
                                where spm.SubProductId == id
                                orderby spm.SubProductName ascending
                                select new SubProductMasterViewModel 
                                 { 
                                     SubProductId= spm.SubProductId,
                                     ProductId= spm.ProductId,
                                     SubProductName=spm.SubProductName,
                                     ProductName = pm.ProductName,
                                     //Specifications = spm.Specifications,
                                     UnitId = spm.UnitId,
                                     Unit = um.UnitName                                 
                                 }).FirstOrDefault();

            return subProductMaster;

        }


        public List<ProductMasterViewModel> GetMasterProductList(int Id)
        {
            List<ProductMasterViewModel> _listProductMaster = new List<ProductMasterViewModel>();
            
            if (Id > 0)
            {
                _listProductMaster = (from wh in myDbContext.ProductMaster.Cast<ProductMaster>()
                                      join ad in myDbContext.UnitMaster on wh.UnitId equals ad.UnitId
                                      join sv in myDbContext.SysVal on wh.GSTTypeId equals sv.Id
                                      where wh.ProductId == Id && wh.IsActive == true
                                      select new ProductMasterViewModel
                                      {
                                          ProductId = wh.ProductId,
                                          ProductName = wh.ProductName,
                                          Specifications = wh.Specifications,
                                          UnitId = wh.UnitId,
                                          UnitName=ad.UnitName,
                                          GSTTypeId = wh.GSTTypeId,
                                          GSTPercen = wh.GSTPercen,
                                          GSTTypeName = sv.Value

                                      }).ToList();
            }
            else
            {
                _listProductMaster = (from wh in myDbContext.ProductMaster.Cast<ProductMaster>()
                                      join ad in myDbContext.UnitMaster on wh.UnitId equals ad.UnitId
                                      join sv in myDbContext.SysVal on wh.GSTTypeId equals sv.Id
                                      where wh.IsActive == true
                                      select new ProductMasterViewModel
                                      {
                                          ProductId = wh.ProductId,
                                          ProductName = wh.ProductName,
                                          Specifications = wh.Specifications,
                                          UnitId = wh.UnitId,
                                          UnitName = ad.UnitName,
                                          GSTTypeId = wh.GSTTypeId,
                                          GSTPercen=wh.GSTPercen,
                                          GSTTypeName=sv.Value

                                      }).ToList();
            }

            return _listProductMaster;
        }

        public List<SubProductMasterViewModel> GetSubMasterProductList(int Id)
        {
            List<SubProductMasterViewModel> _listSubProductMaster = new List<SubProductMasterViewModel>();

            if (Id > 0)
            {
                _listSubProductMaster = (from wh in myDbContext.SubProductMaster.Cast<SubProductMaster>()
                                      join ad in myDbContext.UnitMaster on wh.UnitId equals ad.UnitId
                                      join pm in myDbContext.ProductMaster on wh.ProductId equals pm.ProductId
                                      where wh.SubProductId == Id && wh.IsActive == true
                                      select new SubProductMasterViewModel
                                      {
                                          SubProductId = wh.SubProductId,
                                          ProductId = wh.ProductId,
                                          SubProductName = wh.SubProductName,
                                          ProductName = pm.ProductName,                                          
                                          UnitId = wh.UnitId,
                                          Unit = ad.UnitName

                                      }).ToList();
            }
            else
            {
                _listSubProductMaster = (from wh in myDbContext.SubProductMaster.Cast<SubProductMaster>()
                                         join ad in myDbContext.UnitMaster on wh.UnitId equals ad.UnitId
                                         join pm in myDbContext.ProductMaster on wh.ProductId equals pm.ProductId
                                         where  wh.IsActive == true
                                         select new SubProductMasterViewModel
                                         {
                                             SubProductId = wh.SubProductId,
                                             ProductId = wh.ProductId,
                                             SubProductName = wh.SubProductName,
                                             ProductName = pm.ProductName,                                            
                                             UnitId = wh.UnitId,
                                             Unit = ad.UnitName

                                         }).ToList();
            }

            return _listSubProductMaster;
        }


        public List<DepartmentViewModel> GetDepartmentList(int Id)
        {
            List<DepartmentViewModel> _listDepartment = new List<DepartmentViewModel>();

            if (Id > 0)
            {
                _listDepartment=(from d in myDbContext.Department.Cast<Department>()
                                 join u in myDbContext.User on d.DepartmentHead equals u.UserId                                
                                 where d.DepartmentId == Id && d.IsActive == true
                                 select new DepartmentViewModel
                                 {
                                     DepartmentId = d.DepartmentId,
                                     DepartmentName = d.DepartmentName,
                                     DepartmentHead = d.DepartmentHead,
                                     DepartmentHeadName = u.Name                                    

                                 }).ToList();


            }
            else
            {
                _listDepartment = (from d in myDbContext.Department.Cast<Department>()
                                   join u in myDbContext.User on d.DepartmentHead equals u.UserId
                                   where  d.IsActive == true
                                   select new DepartmentViewModel
                                   {
                                       DepartmentId = d.DepartmentId,
                                       DepartmentName = d.DepartmentName,
                                       DepartmentHead = d.DepartmentHead,
                                       DepartmentHeadName = u.Name

                                   }).ToList();
            }

            return _listDepartment;
        }

        public List<UserRoleViewModel> GetRoleList(int Id)
        {
            List<UserRoleViewModel> _listRole = new List<UserRoleViewModel>();

            if (Id > 0)
            {
                _listRole = (from d in myDbContext.UserRole.Cast<UserRole>()                                   
                                   where d.UserRoleId==Id&& d.IsActive == true
                                   select new UserRoleViewModel
                                   {
                                       UserRoleId = d.UserRoleId,
                                       RoleName = d.RoleName                                      
                                   }).ToList();

            }
            else
            {
                _listRole = (from d in myDbContext.UserRole.Cast<UserRole>()
                                   where d.IsActive == true
                                   select new UserRoleViewModel
                                   {
                                       UserRoleId = d.UserRoleId,
                                       RoleName = d.RoleName
                                   }).ToList();
            }

            return _listRole;
        }


        public int InsertRole(UserRoleViewModel _userRoleViewModel)
        {
            int _response = 0;

           
            if (_userRoleViewModel != null) {

                UserRole userRole = new UserRole() { UserRoleId = _userRoleViewModel.UserRoleId, RoleName = _userRoleViewModel.RoleName, IsActive = true };
                // Add the new object to the Orders collection.
                

                // Submit the change to the database.
                try
                {
                    myDbContext.UserRole.Add(userRole);
                    myDbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                   
                }
            }           

            return _response;
        }

        public int UpdateRole(UserRoleViewModel _userRoleViewModel,Boolean IsActive)
        {
            int _response = 0;            

            if (_userRoleViewModel != null)
            {               

                UserRole userRole = new UserRole() { UserRoleId = _userRoleViewModel.UserRoleId, RoleName = _userRoleViewModel.RoleName, IsActive = IsActive };             

                // Submit the change to the database.
                try
                {
                 myDbContext.UserRole.Update(userRole);
                    _response = myDbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;

                }
            }

            return _response;
        }

        public UserViewModel ValidateUser(string userName, string passWord,int RoleId)
        {
            UserViewModel _userViewModel = new UserViewModel();
            try
            {
                _userViewModel = (from user in myDbContext.User.Cast<User>()
                                  join role in myDbContext.UserRole on user.RoleId equals role.UserRoleId 
                                  where user.IsActive == true && user.Email == userName && user.Password == passWord && user.RoleId== RoleId
                                  select new UserViewModel
                                  {
                                      UserId = user.UserId,
                                      Name = user.Name,
                                      Email = user.Email,
                                      ContactNo = user.ContactNo,
                                      BusinessName = user.BusinessName,
                                      RoleName = role.RoleName,
                                      RoleId = user.RoleId,
                                      Password = user.Password,
                                      CreatedBy = user.CreatedBy,
                                      CreatedDate = user.CreatedDate
                                  }).SingleOrDefault();

                return _userViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int InsertLogManagement(LogManagement _LogManagement)
        {
            int _response = 0;


            if (_LogManagement != null)
            {

               // UserRole userRole = new UserRole() { UserRoleId = _userRoleViewModel.UserRoleId, RoleName = _userRoleViewModel.RoleName, IsActive = true };
                // Add the new object to the Orders collection.


                // Submit the change to the database.
                try
                {
                    myDbContext.LogManagement.Add(_LogManagement);
                    myDbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;

                }
            }

            return _response;
        }

    }
}
