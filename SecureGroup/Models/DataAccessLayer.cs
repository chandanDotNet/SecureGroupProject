using Microsoft.Data.SqlClient;
using SecureGroup.Utility;
using System.Collections.Generic;
using System.Data;
using System;
using SecureGroup.ViewModel;
using Azure;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using SecureGroup.ViewModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using static iTextSharp.awt.geom.Point2D;

namespace SecureGroup.Models
{
    public class DataAccessLayer
    {



        string connectionString = Utility.ConnectionString.CName;


        public IEnumerable<UserViewModel> ValidateUser(int ActionId, int UserId, int RoleId)
        {
            List<UserViewModel> lstUser = new List<UserViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UserManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserViewModel student = new UserViewModel();

                    student.UserId = Convert.ToInt32(rdr["UserId"]);
                    student.Name = rdr["BusinessName"].ToString();
                    student.Name = rdr["Name"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.Password = rdr["Password"].ToString();
                    student.ContactNo = rdr["ContactNo"].ToString();
                    student.MobileNo = rdr["MobileNo"].ToString();
                    student.LandlineNo = rdr["LandlineNo"].ToString();
                    student.AlternativeNo = rdr["AlternativeNo"].ToString();
                    student.RoleId = (Int32)rdr["RoleId"];
                    student.RoleName = rdr["RoleName"].ToString();
                    student.JobTitle = rdr["JobTitle"].ToString();
                    student.UserCode = rdr["UserCode"].ToString();
                    student.DepartmentId = (Int32)rdr["DepartmentId"];
                    student.DepartmentName = rdr["DepartmentName"].ToString();
                    student.ReportingTo = (Int32)rdr["ReportingTo"];
                    student.LoginTime = rdr["LoginTime"].ToString();
                    student.LogoutTime = rdr["LogoutTime"].ToString();

                    lstUser.Add(student);
                }
                con.Close();
            }
            return lstUser;
        }

        public IEnumerable<UserViewModel> GetAllUser(int ActionId,int UserId,int RoleId)
        {
            List<UserViewModel> lstUser = new List<UserViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UserManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserViewModel student = new UserViewModel();

                    student.UserId = Convert.ToInt32(rdr["UserId"]);
                    student.BusinessName = rdr["BusinessName"].ToString();
                    student.Name = rdr["Name"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.Password = rdr["Password"].ToString();
                    student.ContactNo = rdr["ContactNo"].ToString();
                    student.MobileNo = rdr["MobileNo"].ToString();
                    student.LandlineNo = rdr["LandlineNo"].ToString();
                    student.AlternativeNo = rdr["AlternativeNo"].ToString();
                    student.RoleId = (Int32)rdr["RoleId"];
                    student.RoleName = rdr["RoleName"].ToString();
                    student.JobTitle = rdr["JobTitle"].ToString();
                    student.UserCode = rdr["UserCode"].ToString();
                    student.DepartmentId = (Int32)rdr["DepartmentId"];
                    student.DepartmentName = rdr["DepartmentName"].ToString();
                    student.ReportingTo = (Int32)rdr["ReportingTo"];
                    student.LoginTime = rdr["LoginTime"].ToString();
                    student.LogoutTime = rdr["LogoutTime"].ToString();
                    student.IsFlexibleTime =(Boolean) rdr["IsFlexibleTime"];
                    student.PresentAddress = rdr["PresentAddress"].ToString();
                    student.PermanentAddress = rdr["PermanentAddress"].ToString();
                    student.JoiningDate = rdr["JoiningDate"].ToString();
                    student.IsVerifiedKYC = (Boolean)rdr["IsVerifiedKYC"];
                    student.AadhaarCardName = rdr["AadhaarCardName"].ToString();
                    student.PanCardName = rdr["PanCardName"].ToString();
                    student.VoterCardName = rdr["VoterCardName"].ToString();
                    student.GSTNo = rdr["GSTNo"].ToString();
                    student.BloodGroup = rdr["BloodGroup"].ToString();
                    student.GSTFormName = rdr["GSTFormName"].ToString();
                    student.VendorFormName = rdr["VendorFormName"].ToString();
                    student.OfficeAddressId =Convert.ToInt32(rdr["OfficeAddressId"]);
                    student.OfficeAddress = rdr["OfficeAddress"].ToString();

                    lstUser.Add(student);
                }
                con.Close();
            }
            return lstUser;
        }

        public DashboardViewModel GetDashboardData(int ActionId, int Id)
        {
            DashboardViewModel _dashboard = new DashboardViewModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DashboardManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", Id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //DashboardViewModel _dashboard = new DashboardViewModel();

                    _dashboard.ProjectCount = Convert.ToInt32(rdr["ProjectCount"]);
                    _dashboard.TaskCount = Convert.ToInt32(rdr["TaskCount"]);
                    _dashboard.ActiveTaskCount = Convert.ToInt32(rdr["ActiveTaskCount"]);
                    _dashboard.SiteCount = Convert.ToInt32(rdr["SiteCount"]);                    
                    _dashboard.AdminCount = Convert.ToInt32(rdr["AdminCount"]);
                    _dashboard.EmployeeCount = Convert.ToInt32(rdr["EmployeeCount"]);
                    _dashboard.VendorCount = Convert.ToInt32(rdr["VendorCount"]);
                    _dashboard.SupplierCount = Convert.ToInt32(rdr["SupplierCount"]);
                    //_dashboardData.Add(_dashboard);
                }
                con.Close();
            }
            return _dashboard;
        }

        public List<CountryMaster> GetCountry()
        {

            List<CountryMaster> lstCountry = new List<CountryMaster>();
            CountryMaster country = new CountryMaster();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MasterManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    country.ID = Convert.ToInt32(rdr["ID"]);
                    country.Name = rdr["Name"].ToString();
                    country.CountryCode = rdr["CountryCode"].ToString();

                    lstCountry.Add(country);
                }
                con.Close();
            }

            return lstCountry;
        }

        public int AddWarehouse(WarehouseViewModel warehouseViewModel,int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WarehouseManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@WarehouseId", warehouseViewModel.WarehouseId);
                cmd.Parameters.AddWithValue("@WarehouseName", warehouseViewModel.WarehouseName);
                cmd.Parameters.AddWithValue("@Address", warehouseViewModel.Address);
                cmd.Parameters.AddWithValue("@CityId", warehouseViewModel.CityId);
                cmd.Parameters.AddWithValue("@StateId", warehouseViewModel.StateId);
                cmd.Parameters.AddWithValue("@CountryId", warehouseViewModel.CountryId);
                cmd.Parameters.AddWithValue("@ZipCode", warehouseViewModel.ZipCode);
                cmd.Parameters.AddWithValue("@Block", warehouseViewModel.Block);
                cmd.Parameters.AddWithValue("@AdminName", warehouseViewModel.AdminName);
                cmd.Parameters.AddWithValue("@AdminContactNo", warehouseViewModel.AdminContactNo);
                cmd.Parameters.AddWithValue("@AdminEmailId", warehouseViewModel.AdminEmailId);                
                cmd.Parameters.AddWithValue("@Status", warehouseViewModel.StateId);                
                cmd.Parameters.AddWithValue("@AdminId", warehouseViewModel.UserId);
                cmd.Parameters.AddWithValue("@DocumentFileName", warehouseViewModel.DocumentFileName);
                //cmd.Parameters.AddWithValue("@StartDate", warehouseViewModel.St);                

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddProductItem(WHStockProductViewModel wHStockProduct,int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WarehouseManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", wHStockProduct.Id);
                cmd.Parameters.AddWithValue("@WarehouseId", wHStockProduct.WareHouseId);
                cmd.Parameters.AddWithValue("@ProductId", wHStockProduct.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", wHStockProduct.SubProductId);
                cmd.Parameters.AddWithValue("@UnitId", wHStockProduct.UnitId);
                cmd.Parameters.AddWithValue("@Quantity", wHStockProduct.Quantity);
                cmd.Parameters.AddWithValue("@SupplierId", wHStockProduct.SupplierId);
                cmd.Parameters.AddWithValue("@UnitPrice", wHStockProduct.UnitPrice);
                cmd.Parameters.AddWithValue("@Amount", wHStockProduct.Amount);
                cmd.Parameters.AddWithValue("@CreatedBy", wHStockProduct.CreatedBy);
                cmd.Parameters.AddWithValue("@Comment", wHStockProduct.Comment);
                cmd.Parameters.AddWithValue("@CreatedDate", wHStockProduct.CreatedDate);
                cmd.Parameters.AddWithValue("@GSTTypeId", wHStockProduct.GSTTypeId);
                cmd.Parameters.AddWithValue("@GSTAmount", wHStockProduct.GSTAmount);
            
                //cmd.Parameters.AddWithValue("@StartDate", warehouseViewModel.St);                

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }


        public int TransferProductItem(WHStockTransferManageViewModel WHStockTransfer)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WarehouseManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", 3);
                cmd.Parameters.AddWithValue("@WarehouseId", WHStockTransfer.Id);
                cmd.Parameters.AddWithValue("@TransferType", WHStockTransfer.TransferType);
                cmd.Parameters.AddWithValue("@SourceId", WHStockTransfer.SourceId);
                cmd.Parameters.AddWithValue("@DestinationId", WHStockTransfer.DestinationId);
                cmd.Parameters.AddWithValue("@ProductId", WHStockTransfer.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", WHStockTransfer.SubProductId);            
                cmd.Parameters.AddWithValue("@Quantity", WHStockTransfer.TransferQuantity);
                cmd.Parameters.AddWithValue("@TransferDate", WHStockTransfer.TransferDate);
                cmd.Parameters.AddWithValue("@CreatedBy", WHStockTransfer.TransferBy);
                cmd.Parameters.AddWithValue("@Comment", WHStockTransfer.Comment);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public decimal TransferProductQuantityVerify(WHStockTransferManageViewModel WHStockTransfer)
        {

            decimal response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WarehouseManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", 4);
                cmd.Parameters.AddWithValue("@WarehouseId", WHStockTransfer.Id);
                cmd.Parameters.AddWithValue("@TransferType", WHStockTransfer.TransferType);
                cmd.Parameters.AddWithValue("@SourceId", WHStockTransfer.SourceId);
                cmd.Parameters.AddWithValue("@DestinationId", WHStockTransfer.DestinationId);
                cmd.Parameters.AddWithValue("@ProductId", WHStockTransfer.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", WHStockTransfer.SubProductId);
                cmd.Parameters.AddWithValue("@Quantity", WHStockTransfer.TransferQuantity);        

                con.Open();               
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    response = Convert.ToInt32(rdr["Quantity"]);                    
                }
                con.Close();
            }

            return response;
            //throw new NotImplementedException();
        }

        public IEnumerable<WHStockProductViewModel> GetWarehouseStock()
        {
            List<WHStockProductViewModel> _whStock = new List<WHStockProductViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WHStockManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    WHStockProductViewModel stock = new WHStockProductViewModel();

                   // stock.Id = Convert.ToInt32(rdr["Id"]);
                    stock.WareHouseId = Convert.ToInt32(rdr["WareHouseId"]);                    
                    stock.WareHouse = rdr["WareHouseName"].ToString();
                    stock.ProductId = Convert.ToInt32(rdr["ProductId"]);
                    stock.ProductName = rdr["ProductName"].ToString();
                    stock.SubProductId = Convert.ToInt32(rdr["SubProductId"]);
                    stock.SubProduct = rdr["SubProductName"].ToString();
                    stock.Quantity = Convert.ToDecimal(rdr["Quantity"]);                 


                    _whStock.Add(stock);
                }
                con.Close();
            }
            return _whStock;
        }

        public IEnumerable<WHStockProductDetailsViewModel> GetWarehouseStockDetails(int WareHouseId)
        {
            List<WHStockProductDetailsViewModel> _whStock = new List<WHStockProductDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WHStockManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@WareHouseId", WareHouseId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    WHStockProductDetailsViewModel stock = new WHStockProductDetailsViewModel();

                    // stock.Id = Convert.ToInt32(rdr["Id"]);
                    stock.WareHouseId = Convert.ToInt32(rdr["WareHouseId"]);
                    stock.WareHouseName = rdr["WareHouseName"].ToString();
                    stock.ProductId = Convert.ToInt32(rdr["ProductId"]);
                    stock.ProductName = rdr["ProductName"].ToString();
                    stock.SubProductId = Convert.ToInt32(rdr["SubProductId"]);
                    stock.SubProductName = rdr["SubProductName"].ToString();
                    stock.ProductAddQty = Convert.ToDecimal(rdr["ProductAddQty"]);
                    stock.StockIn = Convert.ToDecimal(rdr["StockIn"]);
                    stock.StockOut = Convert.ToDecimal(rdr["StockOut"]);
                    stock.TotalQty = Convert.ToDecimal(rdr["TotalQty"]);
                    stock.TransferType = (rdr["TransferType"]).ToString();


                    _whStock.Add(stock);
                }
                con.Close();
            }
            return _whStock;
        }

        public IEnumerable<WHProductPurchaseInOutDetailsViewModel> GetWarehouseStockPurchaseInOutDetails(int ActionId, int WareHouseId, int ProductId, int SubProductId)
        {
            List<WHProductPurchaseInOutDetailsViewModel> _whStock = new List<WHProductPurchaseInOutDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WHStockManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@WareHouseId", WareHouseId);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", SubProductId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    WHProductPurchaseInOutDetailsViewModel stock = new WHProductPurchaseInOutDetailsViewModel();

                    stock.Id = Convert.ToInt32(rdr["Id"]);
                    stock.WareHouseId = Convert.ToInt32(rdr["WareHouseId"]);
                    stock.WareHouseName = rdr["WareHouseName"].ToString();
                    stock.ProductId = Convert.ToInt32(rdr["ProductId"]);
                    stock.ProductName = rdr["ProductName"].ToString();
                    stock.SubProductId = Convert.ToInt32(rdr["SubProductId"]);
                    stock.SubProductName = rdr["SubProductName"].ToString();
                    stock.ProductQty = Convert.ToDecimal(rdr["ProductQty"]);                    
                    stock.TransferType = (rdr["TransferType"]).ToString();
                    stock.TransferDetails = (rdr["TransferDetails"]).ToString();
                    stock.PurchaseFrom = (rdr["PurchaseFrom"]).ToString();
                    stock.Date = (rdr["Date"]).ToString();

                    _whStock.Add(stock);
                }
                con.Close();
            }
            return _whStock;
        }

        public int DeleteWarehouseStockPurchaseInOutData(int ActionId,int Id)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WHStockManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", Id);               

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddUpdateMasterProductData(ProductMasterViewModel ProductMaster,int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MasterManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@ProductId", ProductMaster.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", ProductMaster.ProductName);
                cmd.Parameters.AddWithValue("@Specifications", ProductMaster.Specifications);
                cmd.Parameters.AddWithValue("@UnitId", ProductMaster.UnitId);         
                cmd.Parameters.AddWithValue("@GSTTypeId", ProductMaster.GSTTypeId);         
                cmd.Parameters.AddWithValue("@GSTPercen", ProductMaster.GSTPercen);         

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddUpdateMasterSubProductData(SubProductMasterViewModel SubProductMaster, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MasterManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@ProductId", SubProductMaster.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", SubProductMaster.SubProductId);
                cmd.Parameters.AddWithValue("@SubProductName", SubProductMaster.SubProductName);
                cmd.Parameters.AddWithValue("@UnitId", SubProductMaster.UnitId);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }


        public int AddUpdateUserData(UserViewModel userViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UserManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", userViewModel.UserId);
                cmd.Parameters.AddWithValue("@BusinessName", userViewModel.BusinessName);
                cmd.Parameters.AddWithValue("@Name", userViewModel.Name);
                cmd.Parameters.AddWithValue("@Email", userViewModel.Email);
                cmd.Parameters.AddWithValue("@Password", userViewModel.Password);
                cmd.Parameters.AddWithValue("@ContactNo", userViewModel.ContactNo);
                cmd.Parameters.AddWithValue("@MobileNo", userViewModel.MobileNo);
                cmd.Parameters.AddWithValue("@LandlineNo", userViewModel.LandlineNo);
                cmd.Parameters.AddWithValue("@AlternativeNo", userViewModel.AlternativeNo);
                cmd.Parameters.AddWithValue("@RoleId", userViewModel.RoleId);
                cmd.Parameters.AddWithValue("@CreatedBy", userViewModel.CreatedBy);
                cmd.Parameters.AddWithValue("@JobTitle", userViewModel.JobTitle);
                cmd.Parameters.AddWithValue("@UserCode", userViewModel.UserCode);
                cmd.Parameters.AddWithValue("@DepartmentId", userViewModel.DepartmentId);
                cmd.Parameters.AddWithValue("@ReportingTo", userViewModel.ReportingTo);
                cmd.Parameters.AddWithValue("@LoginTime", userViewModel.LoginTime);
                cmd.Parameters.AddWithValue("@LogoutTime", userViewModel.LogoutTime);
                cmd.Parameters.AddWithValue("@IsFlexibleTime", userViewModel.IsFlexibleTime);
                cmd.Parameters.AddWithValue("@PermanentAddress", userViewModel.PermanentAddress);
                cmd.Parameters.AddWithValue("@PresentAddress", userViewModel.PresentAddress);
                cmd.Parameters.AddWithValue("@JoiningDate", userViewModel.JoiningDate);
                cmd.Parameters.AddWithValue("@GSTNo", userViewModel.GSTNo);
                cmd.Parameters.AddWithValue("@BloodGroup", userViewModel.BloodGroup);
                cmd.Parameters.AddWithValue("@OfficeAddressId", userViewModel.OfficeAddressId);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int UpdateUserKYCData(int ActionId,UserKYC userKYC)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UserManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", userKYC.UserId);
                cmd.Parameters.AddWithValue("@IsVerifiedKYC", userKYC.IsVerifiedKYC);
                cmd.Parameters.AddWithValue("@AadhaarCardName", userKYC.AadhaarCardName);
                cmd.Parameters.AddWithValue("@PanCardName", userKYC.PanCardName);
                cmd.Parameters.AddWithValue("@VoterCardName", userKYC.VoterCardName);
                cmd.Parameters.AddWithValue("@GSTFormName", userKYC.GSTFormName);
                cmd.Parameters.AddWithValue("@VendorFormName", userKYC.VendorFormName);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddOfficeAddressData(OfficeAddressViewModel officeAddressViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_OfficeAddressManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@OfficeAddressId", officeAddressViewModel.OfficeAddressId);
                cmd.Parameters.AddWithValue("@OfficeAddressName", officeAddressViewModel.OfficeAddressName);
                cmd.Parameters.AddWithValue("@FullAddress", officeAddressViewModel.FullAddress);
                cmd.Parameters.AddWithValue("@OfficeLocationId", officeAddressViewModel.OfficeLocationId);
                cmd.Parameters.AddWithValue("@OfficeStateId", officeAddressViewModel.OfficeStateId);
                cmd.Parameters.AddWithValue("@Lat", officeAddressViewModel.Lat);
                cmd.Parameters.AddWithValue("@Long", officeAddressViewModel.Long);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public IEnumerable<OfficeAddressViewModel> GetOfficeAddressData(int ActionId,int OfficeAddressId)
        {
            List<OfficeAddressViewModel> _officeAddress = new List<OfficeAddressViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_OfficeAddressManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@OfficeAddressId", OfficeAddressId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    OfficeAddressViewModel officeAddress = new OfficeAddressViewModel();

                    // stock.Id = Convert.ToInt32(rdr["Id"]);
                    officeAddress.OfficeAddressId = Convert.ToInt32(rdr["OfficeAddressId"]);
                    officeAddress.OfficeAddressName = rdr["OfficeAddressName"].ToString();
                    officeAddress.FullAddress = rdr["FullAddress"].ToString();
                    officeAddress.OfficeLocationId = Convert.ToInt32(rdr["OfficeLocationId"]);
                    officeAddress.OfficeStateId = Convert.ToInt32(rdr["OfficeStateId"]);
                    officeAddress.Lat = Convert.ToDouble(rdr["Lat"]);
                    officeAddress.Long = Convert.ToDouble(rdr["Long"]);
                    officeAddress.OfficeLocation = rdr["OfficeLocation"].ToString();
                    officeAddress.OfficeState = rdr["OfficeState"].ToString();
                    _officeAddress.Add(officeAddress);
                }
                con.Close();
            }
            return _officeAddress;
        }



        public int AddDepartmentData(DepartmentViewModel departmentViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MasterManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@DepartmentId", departmentViewModel.DepartmentId);
                cmd.Parameters.AddWithValue("@DepartmentName", departmentViewModel.DepartmentName);
                cmd.Parameters.AddWithValue("@DepartmentHead", departmentViewModel.DepartmentHead);
               

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddUpdateAssignTaskData(TaskViewModel taskViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_TaskManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", taskViewModel.TaskId);
                cmd.Parameters.AddWithValue("@AssignTo", taskViewModel.AssignTo);
                cmd.Parameters.AddWithValue("@TaskName", taskViewModel.TaskName);               
                cmd.Parameters.AddWithValue("@TaskDueDateTime", taskViewModel.TaskDueDateTime);
                cmd.Parameters.AddWithValue("@ProjectId", taskViewModel.ProjectId);
                cmd.Parameters.AddWithValue("@ProjectHeadId", taskViewModel.ProjectHeadId);
                cmd.Parameters.AddWithValue("@SiteId", taskViewModel.SiteId);
                cmd.Parameters.AddWithValue("@TaskPriority", taskViewModel.TaskPriority);
                cmd.Parameters.AddWithValue("@TaskDescription", taskViewModel.TaskDescription);
                cmd.Parameters.AddWithValue("@TaskDocument", taskViewModel.TaskDocumentName);
                cmd.Parameters.AddWithValue("@TaskStatus", taskViewModel.TaskStatus);
                cmd.Parameters.AddWithValue("@CreatedBy", taskViewModel.CreatedBy);


                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }
                con.Close();
            }
            return response;
        }


        public int AddUpdateTaskUpdateData(TaskUpdateViewModel taskUpdateViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_TaskManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", taskUpdateViewModel.TaskId);             
                cmd.Parameters.AddWithValue("@TaskDocument", taskUpdateViewModel.TaskDocumentName);
                cmd.Parameters.AddWithValue("@TaskStatus", taskUpdateViewModel.TaskStatus);
                cmd.Parameters.AddWithValue("@CreatedBy", taskUpdateViewModel.CreatedBy);
                cmd.Parameters.AddWithValue("@Comment", taskUpdateViewModel.Comment);
                cmd.Parameters.AddWithValue("@SpentTime", taskUpdateViewModel.SpentTime);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public IEnumerable<TaskViewModel> GetAllTaskAllocation(int ActionId, int TaskId, int AssignTo,int CreatedBy)
        {
                       

            List<TaskViewModel> allTask = new List<TaskViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_TaskManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", TaskId);
                cmd.Parameters.AddWithValue("@AssignTo", AssignTo);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TaskViewModel task = new TaskViewModel();

                    task.TaskId = Convert.ToInt32(rdr["TaskId"]);                    
                    task.AssignTo = (Int32)rdr["AssignTo"];
                    task.AssignToName = rdr["AssignToName"].ToString();
                    task.TaskName = rdr["TaskName"].ToString();
                    task.TaskDueDateTime = rdr["TaskDueDateTime"].ToString();
                    DateTime myDT = Convert.ToDateTime(rdr["TaskDueDateTime"].ToString());
                    task.TaskDueDate = myDT.ToLongDateString();
                    task.TaskDueTime = myDT.ToString("hh:mm tt");
                    
                    task.ProjectId = (Int32)rdr["ProjectId"];
                    task.ProjectName = rdr["ProjectName"].ToString();
                    task.ProjectHeadId = (Int32)rdr["ProjectHeadId"];
                    task.ProjectHeadName = rdr["ProjectHeadName"].ToString();
                    task.SiteId = (Int32)rdr["SiteId"];
                    task.SiteName = rdr["SiteName"].ToString();                    
                    task.TaskPriority = rdr["TaskPriority"].ToString();
                    task.TaskDescription = rdr["TaskDescription"].ToString();
                    task.TaskDocumentName = rdr["TaskDocument"].ToString();
                    task.TaskStatus = (Int32)rdr["TaskStatus"];
                    task.TaskStatusValue = rdr["TaskStatusValue"].ToString();
                    task.CreatedBy = (Int32)rdr["CreatedBy"];
                    task.TaskUpdateList= GetAllTaskUpdateList(5, task.TaskId, 0, 0).ToList();
                    allTask.Add(task);
                }
                con.Close();
            }
            return allTask;
        }

        public IEnumerable<TaskUpdateViewModel> GetAllTaskUpdateList(int ActionId, int TaskId, int AssignTo, int CreatedBy)
        {
            List<TaskUpdateViewModel> allTask = new List<TaskUpdateViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_TaskManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", TaskId);
                cmd.Parameters.AddWithValue("@AssignTo", AssignTo);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TaskUpdateViewModel taskUpdate = new TaskUpdateViewModel();

                    taskUpdate.TaskUpdateId = Convert.ToInt32(rdr["TaskUpdateId"]);
                    taskUpdate.TaskId = Convert.ToInt32(rdr["TaskId"]);                    
                    taskUpdate.Comment = rdr["Comment"].ToString();
                    taskUpdate.SpentTime = rdr["SpentTime"].ToString();
                    taskUpdate.TaskDocumentName = rdr["TaskDocument"].ToString();
                    taskUpdate.CreatedDateTime = rdr["CreatedDateTime"].ToString();
                    taskUpdate.TaskStatus = (Int32)rdr["TaskStatus"];
                    taskUpdate.TaskStatusValue = rdr["TaskStatusValue"].ToString();
                    taskUpdate.CreatedBy = (Int32)rdr["CreatedBy"];                  
                    taskUpdate.CreatedByName = rdr["CreatedByName"].ToString();
                    
                    allTask.Add(taskUpdate);
                }
                con.Close();
            }
            return allTask;
        }

        public IEnumerable<AttendanceViewModel> GetUserAttendanceList(int ActionId, int AttendanceId, int UserId, int CreatedBy)
        {
            List<AttendanceViewModel> _attendanceList = new List<AttendanceViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AttendanceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@AttendanceId", AttendanceId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AttendanceViewModel _attendance = new AttendanceViewModel();

                    _attendance.AttendanceId = Convert.ToInt32(rdr["AttendanceId"]);
                    _attendance.UserId = Convert.ToInt32(rdr["UserId"]);
                    _attendance.Date = rdr["Date"].ToString();
                    _attendance.LoginTime = rdr["LoginTime"].ToString();
                    _attendance.LogoutTime = rdr["LogoutTime"].ToString();
                    _attendance.DurationTime = rdr["DurationTime"].ToString();
                    _attendance.Reason = rdr["Reason"].ToString();
                    _attendance.AttendanceStatusId = Convert.ToInt32(rdr["AttendanceStatusId"]);
                    _attendance.AttendanceStatus = rdr["AttendanceStatus"].ToString();
                    _attendance.CreatedBy = (Int32)rdr["CreatedBy"];
                    _attendance.CreatedDate =(DateTime) rdr["CreatedDateTime"];
                    _attendance.UserName = rdr["UserName"].ToString();
                    _attendance.DeviationsLoginTime = rdr["DeviationsLoginTime"].ToString();
                    _attendance.DeviationsLogoutTime = rdr["DeviationsLogoutTime"].ToString();
                    _attendance.DeviationsDurationTime = rdr["DeviationsDurationTime"].ToString();
                    _attendance.DeviationsApprovalStatusId = (Int32)rdr["DeviationsApprovalStatusId"];
                    _attendance.DeviationsApprovalStatus = rdr["DeviationsApprovalStatus"].ToString();

                    _attendanceList.Add(_attendance);
                }
                con.Close();
            }
            return _attendanceList;
        }

        public int AddUpdateUserAttendanceeData(AttendanceViewModel attendanceViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AttendanceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@AttendanceId", attendanceViewModel.AttendanceId);
                cmd.Parameters.AddWithValue("@UserId", attendanceViewModel.UserId);
                cmd.Parameters.AddWithValue("@Date", SqlDbType.Date).Value= attendanceViewModel.Date;
                cmd.Parameters.AddWithValue("@CreatedBy", attendanceViewModel.CreatedBy);
                cmd.Parameters.AddWithValue("@LoginTime", SqlDbType.Time).Value = attendanceViewModel.LoginTime;
                cmd.Parameters.AddWithValue("@LogoutTime", SqlDbType.Time).Value = attendanceViewModel.LogoutTime;
                cmd.Parameters.AddWithValue("@DurationTime", SqlDbType.Time).Value = attendanceViewModel.DurationTime;
                cmd.Parameters.AddWithValue("@Reason", attendanceViewModel.Reason);
                cmd.Parameters.AddWithValue("@AttendanceStatusId", attendanceViewModel.AttendanceStatusId);
                cmd.Parameters.AddWithValue("@DeviationsLoginTime", SqlDbType.Time).Value=attendanceViewModel.DeviationsLoginTime;
                cmd.Parameters.AddWithValue("@DeviationsLogoutTime", SqlDbType.Time).Value = attendanceViewModel.DeviationsLogoutTime;               
                cmd.Parameters.AddWithValue("@DeviationsDurationTime", SqlDbType.Time).Value = attendanceViewModel.DeviationsDurationTime;
                cmd.Parameters.AddWithValue("@DeviationsApprovalStatusId", attendanceViewModel.DeviationsApprovalStatusId);
                cmd.Parameters.AddWithValue("@AttendanceApproveRejectBy", attendanceViewModel.AttendanceApproveRejectBy);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int UpdateUserAttendanceeTimeData(AttendanceViewModel attendanceViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AttendanceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@AttendanceId", attendanceViewModel.AttendanceId);
                cmd.Parameters.AddWithValue("@UserId", attendanceViewModel.UserId);
                cmd.Parameters.AddWithValue("@AttendanceStatusId", attendanceViewModel.AttendanceStatusId);
               

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public int AddUpdateUserLeaveData(LeaveApplyViewModel _leaveApplyViewModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_LeaveManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@LeaveId", _leaveApplyViewModel.LeaveId);
                cmd.Parameters.AddWithValue("@UserId", _leaveApplyViewModel.UserId);
                cmd.Parameters.AddWithValue("@StartDate", SqlDbType.Date).Value = _leaveApplyViewModel.StartDate;
                cmd.Parameters.AddWithValue("@EndDate", SqlDbType.Date).Value = _leaveApplyViewModel.EndDate;
                cmd.Parameters.AddWithValue("@LeaveReason", _leaveApplyViewModel.LeaveReason);
                cmd.Parameters.AddWithValue("@LeaveType", SqlDbType.Int).Value =Convert.ToInt32(_leaveApplyViewModel.LeaveTypeValue);
                cmd.Parameters.AddWithValue("@Document", _leaveApplyViewModel.DocumentName);
                cmd.Parameters.AddWithValue("@LeaveStatus", _leaveApplyViewModel.LeaveStatus);
                cmd.Parameters.AddWithValue("@LeaveApproveRejectBy", _leaveApplyViewModel.LeaveApproveRejectBy);
                cmd.Parameters.AddWithValue("@LeaveRejectedReason", _leaveApplyViewModel.LeaveRejectedReason);
                cmd.Parameters.AddWithValue("@NumberOfleaveCount", _leaveApplyViewModel.NumberOfleaveCount);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public IEnumerable<LeaveApplyHistory> GetUserLeaveList(int ActionId, int LeaveId, int UserId, int CreatedBy)
        {
            List<LeaveApplyHistory> _leaveApplyList = new List<LeaveApplyHistory>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_LeaveManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@LeaveId", LeaveId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    LeaveApplyHistory _leaveApply = new LeaveApplyHistory();

                    _leaveApply.LeaveId = Convert.ToInt32(rdr["LeaveId"]);
                    _leaveApply.UserId = Convert.ToInt32(rdr["UserId"]);
                    _leaveApply.StartDate = rdr["StartDate"].ToString();
                    _leaveApply.EndDate = rdr["EndDate"].ToString();
                    _leaveApply.NumberOfleaveCount = rdr["NumberOfleaveCount"].ToString();
                    _leaveApply.LeaveReason = rdr["LeaveReason"].ToString();
                    _leaveApply.LeaveStatusValue = rdr["LeaveStatusValue"].ToString();
                    _leaveApply.LeaveStatus = (Int32)rdr["LeaveStatus"];
                    _leaveApply.LeaveApproveRejectBy =(Int32) rdr["LeaveApproveRejectBy"];
                    _leaveApply.LeaveApproveRejectByName = rdr["LeaveApproveRejectByName"].ToString();
                    _leaveApply.LeaveRejectedReason = rdr["LeaveRejectedReason"].ToString();
                    _leaveApply.DocumentName = rdr["Document"].ToString();
                    _leaveApply.LeaveTypeValue = rdr["LeaveTypeValue"].ToString();
                    _leaveApply.LeaveType = (Int32)rdr["LeaveType"];
                    _leaveApply.UserName = rdr["UserName"].ToString();
                    _leaveApply.ReportingUserLeaveStatusValue = rdr["ReportingUserLeaveStatusValue"].ToString();
                    _leaveApply.ReportingUserId = Convert.ToInt32(rdr["ReportingUserId"]);
                    _leaveApply.ReportingUserName = rdr["ReportingUserName"].ToString();

                    _leaveApplyList.Add(_leaveApply);
                }
                con.Close();
            }
            return _leaveApplyList;
        }


        public IEnumerable<ReimbursementViewModel> GetReimbursementData(int ActionId, int Id, int UserId, int CreatedBy)
        {
            List<ReimbursementViewModel> _reimbursementList = new List<ReimbursementViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ReimbursementManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ReimbursementViewModel _reimbursement = new ReimbursementViewModel();

                    _reimbursement.Id = Convert.ToInt32(rdr["Id"]);
                    _reimbursement.ExpenseFor = rdr["ExpenseFor"].ToString();
                    _reimbursement.TotalExpenseAmount = rdr["TotalExpenseAmount"].ToString();
                    _reimbursement.ClaimAmount = rdr["ClaimAmount"].ToString();
                    _reimbursement.ApprovedAmount = rdr["ApprovedAmount"].ToString();
                    _reimbursement.ExpenseDate = rdr["ExpenseDate"].ToString();
                    _reimbursement.ExpenseDate1 =  Convert.ToDateTime(rdr["ExpenseDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat); //Convert.ToDateTime(rdr["ExpenseDate"]);
                    _reimbursement.ExpenseToDate = rdr["ExpenseToDate"].ToString();
                    _reimbursement.ExpenseToDate1 =Convert.ToDateTime(rdr["ExpenseToDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat); //Convert.ToDateTime( rdr["ExpenseToDate"]); 
                    _reimbursement.ExpenseDocumentName = rdr["ExpenseDocument"].ToString();
                    _reimbursement.SignatureDataUrl = rdr["Signature"].ToString();
                    _reimbursement.ApprovedBy = (Int32)rdr["ApprovedBy"];
                    _reimbursement.ApprovedByName = rdr["ApprovedByName"].ToString();
                    _reimbursement.ApprovedDate = rdr["ApprovedDate"].ToString();
                    _reimbursement.StatusId = (Int32)rdr["StatusId"];
                    _reimbursement.Status = rdr["Status"].ToString();
                    _reimbursement.ClaimBy = (Int32)rdr["ClaimBy"];
                    _reimbursement.ClaimByName = rdr["ClaimByName"].ToString();
                    _reimbursement.Comments = rdr["Comments"].ToString();

                    _reimbursementList.Add(_reimbursement);
                }
                con.Close();
            }
            return _reimbursementList;
        }

        public int AddUpdateReimbursementData(ReimbursementViewModel _reimbursement, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ReimbursementManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", _reimbursement.Id);
                cmd.Parameters.AddWithValue("@UserId", _reimbursement.ClaimBy);
                cmd.Parameters.AddWithValue("@ExpenseFor", _reimbursement.ExpenseFor);
                cmd.Parameters.AddWithValue("@TotalExpenseAmount", _reimbursement.TotalExpenseAmount);
                cmd.Parameters.AddWithValue("@ClaimAmount", _reimbursement.ClaimAmount);
                cmd.Parameters.AddWithValue("@ApprovedAmount", _reimbursement.ApprovedAmount);
                //cmd.Parameters.AddWithValue("@ExpenseDate", _reimbursement.ExpenseDate); Convert.ToDateTime(rdr["ExpenseDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                cmd.Parameters.AddWithValue("@ExpenseDate", SqlDbType.DateTime).Value = (_reimbursement.ExpenseDate == null) ? (object)DBNull.Value : Convert.ToDateTime(_reimbursement.ExpenseDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                cmd.Parameters.AddWithValue("@ExpenseDocument", _reimbursement.ExpenseDocumentName);
                cmd.Parameters.AddWithValue("@Signature", _reimbursement.SignatureDataUrl);
                cmd.Parameters.AddWithValue("@ApprovedBy", _reimbursement.ApprovedBy);
                cmd.Parameters.AddWithValue("@ApprovedDate", SqlDbType.DateTime).Value = (_reimbursement.ApprovedDate == null) ? (object)DBNull.Value : Convert.ToDateTime(_reimbursement.ApprovedDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                cmd.Parameters.AddWithValue("@StatusId", _reimbursement.StatusId);
                cmd.Parameters.AddWithValue("@ClaimBy", _reimbursement.ClaimBy);
                cmd.Parameters.AddWithValue("@Comments", _reimbursement.Comments);
                cmd.Parameters.AddWithValue("@CreatedBy", _reimbursement.CreatedBy);
                cmd.Parameters.AddWithValue("@ExpenseToDate", SqlDbType.DateTime).Value = (_reimbursement.ExpenseToDate == null)?(object)DBNull.Value: Convert.ToDateTime(_reimbursement.ExpenseToDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
               

                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }               
                con.Close();
            }
            return response;
        }

        public int AddUpdateReimbursementDocumentData(ReimbursementViewModel _reimbursement, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ReimbursementManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", _reimbursement.Id);               
                cmd.Parameters.AddWithValue("@ExpenseDocument", _reimbursement.ExpenseDocumentName);               
                cmd.Parameters.AddWithValue("@CreatedBy", _reimbursement.CreatedBy);
                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }
                con.Close();
            }
            return response;
        }

        public IEnumerable<DocumentListViewModel> GetReimbursementDocumentData(int ActionId, int Id)
        {
            List<DocumentListViewModel> _documentList = new List<DocumentListViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ReimbursementManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Id", Id);                

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DocumentListViewModel _document = new DocumentListViewModel();

                    _document.DocumentId = Convert.ToInt32(rdr["DocumentId"]);
                    _document.ReimbursementId = Convert.ToInt32(rdr["ReimbursementId"]);
                    _document.DocumentName = rdr["DocumentName"].ToString();
                    _document.DocumentUrl = rdr["DocumentUrl"].ToString();
                    _document.UploadedDate = rdr["UploadedDate"].ToString();
                    _documentList.Add(_document);
                }
                con.Close();
            }
            return _documentList;
        }


        public IEnumerable<SiteViewModel> GetSiteListData(int ActionId, int SiteId)
        {
            List<SiteViewModel> _siteList = new List<SiteViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_SiteManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@SiteId", SiteId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    SiteViewModel _site = new SiteViewModel();

                    _site.SiteId = Convert.ToInt32(rdr["SiteId"]);
                    _site.SiteName = rdr["SiteName"].ToString();
                    _site.Address = rdr["Address"].ToString();
                    _site.StateId = Convert.ToInt32(rdr["StateId"]);
                    _site.StateName = rdr["StateName"].ToString();
                    _site.CityId = Convert.ToInt32(rdr["CityId"]);
                    _site.CityName = rdr["CityName"].ToString();
                    _site.Block = rdr["Block"].ToString();
                    _site.Scheme = rdr["Scheme"].ToString();
                    _site.ZipCode = rdr["ZipCode"].ToString();
                    _site.CountryId = Convert.ToInt32(rdr["CountryId"]);
                    _site.Lat = (float?)Convert.ToDouble(rdr["Lat"]);
                    _site.Long = (float?)Convert.ToDouble(rdr["Long"]);



                    _siteList.Add(_site);
                }
                con.Close();
            }
            return _siteList;
        }

        public int AddUpdateSiteData(SiteViewModel _siteModel, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_SiteManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@SiteId", _siteModel.SiteId);
                cmd.Parameters.AddWithValue("@SiteName", _siteModel.SiteName);
                cmd.Parameters.AddWithValue("@Address", _siteModel.Address);
                cmd.Parameters.AddWithValue("@StateId", _siteModel.StateId);
                cmd.Parameters.AddWithValue("@CityId", _siteModel.CityId);
                cmd.Parameters.AddWithValue("@Block", _siteModel.Block);
                cmd.Parameters.AddWithValue("@Scheme", _siteModel.Scheme);
                cmd.Parameters.AddWithValue("@ZipCode", _siteModel.ZipCode);
                cmd.Parameters.AddWithValue("@CountryId", _siteModel.CountryId);
                cmd.Parameters.AddWithValue("@CreatedBy", _siteModel.CreatedBy);
                cmd.Parameters.AddWithValue("@Lat", _siteModel.Lat);
                cmd.Parameters.AddWithValue("@Long", _siteModel.Long);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }


        public IEnumerable<ProjectViewModel> GetProjectsListData(int ActionId,int ProjectId)
        {
            List<ProjectViewModel> _projectList = new List<ProjectViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ProjectManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProjectViewModel _project = new ProjectViewModel();

                    _project.ProjectId = Convert.ToInt32(rdr["ProjectId"]);                    
                    _project.ProjectName = rdr["ProjectName"].ToString();                    
                    _project.Description = rdr["Description"].ToString();
                    _project.Dates = rdr["Dates"].ToString();
                    if (!(rdr["ProjectStartDate"] is DBNull))
                        _project.ProjectStartDate = Convert.ToDateTime(rdr["ProjectStartDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    if (!(rdr["ProjectDueDate"] is DBNull))
                    //_project.ProjectDueDate = Convert.ToDateTime(rdr["ProjectDueDate"]);
                    _project.ProjectDueDate = Convert.ToDateTime(rdr["ProjectDueDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    if (!(rdr["ProjectBudget"] is DBNull))
                        //_project.ProjectBudget = Convert.ToDecimal(rdr["ProjectBudget"]);
                        _project.ProjectBudget = Convert.ToDecimal(rdr["ProjectBudget"]);

                    _project.ProjectHeadId = Convert.ToInt32(rdr["ProjectHeadId"]);
                    _project.ProjectHeadName =rdr["ProjectHeadName"].ToString();
                    _project.Address = rdr["Address"].ToString();
                    _project.StateId = Convert.ToInt32(rdr["StateId"]);
                    _project.StateName = rdr["StateName"].ToString();
                    _project.CityId = Convert.ToInt32(rdr["CityId"]);
                    _project.CityName = rdr["CityName"].ToString();
                    _project.Block = rdr["Block"].ToString();
                    _project.ZipCode = rdr["ZipCode"].ToString();
                    _project.SchemeId = Convert.ToInt32(rdr["SchemeId"]);
                    _project.SchemeName = rdr["SchemeName"].ToString();
                    _project.CountryId = Convert.ToInt32(rdr["CountryId"]);
                    _project.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);
                    _project.StatusId = Convert.ToInt32(rdr["StatusId"]);
                    _project.Status = rdr["Status"].ToString();

                    _projectList.Add(_project);
                }
                con.Close();
            }
            return _projectList;
        }

        public int AddUpdateProjectsData(ProjectViewModel _project, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ProjectManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@ProjectId", _project.ProjectId);
                cmd.Parameters.AddWithValue("@ProjectName", _project.ProjectName);
                cmd.Parameters.AddWithValue("@Description", _project.Description);
                cmd.Parameters.AddWithValue("@ProjectStartDate", _project.ProjectStartDate);
                cmd.Parameters.AddWithValue("@ProjectDueDate", _project.ProjectDueDate);
                cmd.Parameters.AddWithValue("@ProjectBudget", _project.ProjectBudget);
                cmd.Parameters.AddWithValue("@ProjectHeadId", _project.ProjectHeadId);
                cmd.Parameters.AddWithValue("@Address", _project.Address);
                cmd.Parameters.AddWithValue("@StateId", _project.StateId);
                cmd.Parameters.AddWithValue("@CityId", _project.CityId);
                cmd.Parameters.AddWithValue("@Block", _project.Block);
                cmd.Parameters.AddWithValue("@ZipCode", _project.ZipCode);
                cmd.Parameters.AddWithValue("@SchemeId", _project.SchemeId);
                cmd.Parameters.AddWithValue("@CountryId", _project.CountryId);
                cmd.Parameters.AddWithValue("@StatusId", _project.StatusId);
                cmd.Parameters.AddWithValue("@CreatedBy", _project.CreatedBy);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public IEnumerable<ProjectViewModel> GetAssignedProjectsListData(int ActionId, int CreatedBy)
        {
            List<ProjectViewModel> _projectList = new List<ProjectViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ProjectManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
               
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProjectViewModel _project = new ProjectViewModel();

                    _project.ProjectId = Convert.ToInt32(rdr["ProjectId"]);
                    _project.ProjectName = rdr["ProjectName"].ToString();
                    _project.Description = rdr["Description"].ToString();                   
                    _project.NoOfTask = Convert.ToInt32(rdr["NoOfTask"]);
                    _project.SpentTime = rdr["SpentTime"].ToString();
                    _project.LastStatusUpdate = rdr["LastStatusUpdate"].ToString();                    
                    _project.Status = rdr["Status"].ToString();
                    _project.ProjectHeadName = rdr["ProjectHeadName"].ToString();

                    _projectList.Add(_project);
                }
                con.Close();
            }
            return _projectList;
        }

        public IEnumerable<ProjectViewModel> GetAssignedProjectsTaskListData(int ActionId, int CreatedBy, int ProjectId)
        {
            List<ProjectViewModel> _projectList = new List<ProjectViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ProjectManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProjectViewModel _project = new ProjectViewModel();

                    _project.TaskId = Convert.ToInt32(rdr["TaskId"]);
                    _project.TaskName = rdr["TaskName"].ToString();                    
                    _project.SpentTime = rdr["SpentTime"].ToString();                    

                    _projectList.Add(_project);
                }
                con.Close();
            }
            return _projectList;
        }


        public int AddUpdatePurchaseOrderData(PurchaseOrderViewModel _purchaseOrder, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrderManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", _purchaseOrder.PurchaseOrderId);
                cmd.Parameters.AddWithValue("@PurchaseNo", _purchaseOrder.PurchaseNo);
                //cmd.Parameters.AddWithValue("@PurchaseDate", _purchaseOrder.PurchaseDate);
                cmd.Parameters.AddWithValue("@PurchaseDate", SqlDbType.DateTime).Value = (_purchaseOrder.PurchaseDate == null) ? (object)DBNull.Value : Convert.ToDateTime(_purchaseOrder.PurchaseDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                cmd.Parameters.AddWithValue("@VendorId", _purchaseOrder.VendorId);
                cmd.Parameters.AddWithValue("@VendorAddress", _purchaseOrder.VendorAddress);
                cmd.Parameters.AddWithValue("@VendorEmail", _purchaseOrder.VendorEmail);
                cmd.Parameters.AddWithValue("@VendorGSTNo", _purchaseOrder.VendorGSTNo);
                cmd.Parameters.AddWithValue("@VendorMobileNo", _purchaseOrder.VendorMobileNo);
                cmd.Parameters.AddWithValue("@VendorCode", _purchaseOrder.VendorCode);
                cmd.Parameters.AddWithValue("@ShipTo", _purchaseOrder.ShipTo);
                cmd.Parameters.AddWithValue("@ShipToAddress", _purchaseOrder.ShipToAddress);
                cmd.Parameters.AddWithValue("@BillTo", _purchaseOrder.BillTo);
                cmd.Parameters.AddWithValue("@BillToAddress", _purchaseOrder.BillToAddress);
                cmd.Parameters.AddWithValue("@BillToGSTNo", _purchaseOrder.BillToGSTNo);
                cmd.Parameters.AddWithValue("@TermsConditions", _purchaseOrder.TermsConditions);
                cmd.Parameters.AddWithValue("@CreatedBy", _purchaseOrder.CreatedBy);

                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }
                con.Close();
            }
            return response;
        }

        public int DeletePurchaseOrderData(PurchaseOrderViewModel _purchaseOrder, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrderManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", _purchaseOrder.PurchaseOrderId);             
               
                cmd.Parameters.AddWithValue("@CreatedBy", _purchaseOrder.CreatedBy);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }
        public int AddUpdatePurchaseOrderProductItemData(int ActionId, int PurchaseOrderId,PurchaseOrderDetailsViewModel _purchaseOrderItem)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrderManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@PurchaseOrderDetailsId", _purchaseOrderItem.PurchaseOrderDetailsId);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                cmd.Parameters.AddWithValue("@ProductId", _purchaseOrderItem.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", _purchaseOrderItem.SubProductId);
                cmd.Parameters.AddWithValue("@Thickness", _purchaseOrderItem.Thickness);
                cmd.Parameters.AddWithValue("@UnitId", _purchaseOrderItem.UnitId);
                cmd.Parameters.AddWithValue("@Quantity", _purchaseOrderItem.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", _purchaseOrderItem.UnitPrice);
                cmd.Parameters.AddWithValue("@Amount", _purchaseOrderItem.Amount);
                cmd.Parameters.AddWithValue("@GSTTypeId", _purchaseOrderItem.GSTTypeId);
                cmd.Parameters.AddWithValue("@GSTPercen", _purchaseOrderItem.GSTPercen);
                cmd.Parameters.AddWithValue("@GSTAmount", _purchaseOrderItem.GSTAmount);
                cmd.Parameters.AddWithValue("@TotalAmount", _purchaseOrderItem.TotalAmount);              

                con.Open();
                response = cmd.ExecuteNonQuery();                
                con.Close();
            }
            return response;
        }

        public IEnumerable<PurchaseOrderViewModel> GetPurchaseOrderListData(int ActionId,int PurchaseOrderId, int CreatedBy)
        {
            List<PurchaseOrderViewModel> _purchaseOrderList = new List<PurchaseOrderViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrderManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PurchaseOrderViewModel _purchaseOrder = new PurchaseOrderViewModel();

                    _purchaseOrder.PurchaseOrderId = Convert.ToInt32(rdr["PurchaseOrderId"]);
                    _purchaseOrder.PurchaseNo = rdr["PurchaseNo"].ToString();
                   // _purchaseOrder.PurchaseDate = rdr["PurchaseDate"].ToString();                   
                    _purchaseOrder.VendorId = Convert.ToInt32(rdr["VendorId"]);
                    if (!(rdr["PurchaseDate"] is DBNull))
                    _purchaseOrder.PurchaseDate = Convert.ToDateTime(rdr["PurchaseDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);                    
                    _purchaseOrder.VendorName = rdr["VendorName"].ToString();
                    _purchaseOrder.VendorAddress = rdr["VendorAddress"].ToString();
                    _purchaseOrder.VendorEmail = rdr["VendorEmail"].ToString();
                    _purchaseOrder.VendorGSTNo = rdr["VendorGSTNo"].ToString();
                    _purchaseOrder.VendorMobileNo = rdr["VendorMobileNo"].ToString();
                    _purchaseOrder.VendorCode = rdr["VendorCode"].ToString();
                    _purchaseOrder.ShipTo = rdr["ShipTo"].ToString();
                    _purchaseOrder.ShipToAddress = rdr["ShipToAddress"].ToString();
                    _purchaseOrder.BillTo = rdr["BillTo"].ToString();
                    _purchaseOrder.BillToAddress = rdr["BillToAddress"].ToString();
                    _purchaseOrder.BillToGSTNo = rdr["BillToGSTNo"].ToString();
                    _purchaseOrder.TermsConditions = rdr["TermsConditions"].ToString();


                    if(PurchaseOrderId>0)
                    {
                        _purchaseOrder.PurchaseOrderDetails = GetPurchaseOrderItemListData(4, PurchaseOrderId,0, 0).ToList();
                    }

                    _purchaseOrderList.Add(_purchaseOrder);
                }
                con.Close();
            }
            return _purchaseOrderList;
        }

        public IEnumerable<PurchaseOrderDetailsViewModel> GetPurchaseOrderItemListData(int ActionId, int PurchaseOrderId,int PurchaseOrderDetailsId, int CreatedBy)
        {
            List<PurchaseOrderDetailsViewModel> _purchaseOrderItemList = new List<PurchaseOrderDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PurchaseOrderManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", PurchaseOrderId);
                cmd.Parameters.AddWithValue("@PurchaseOrderDetailsId", PurchaseOrderDetailsId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PurchaseOrderDetailsViewModel _purchaseOrderItem = new PurchaseOrderDetailsViewModel();

                    _purchaseOrderItem.RowId = Convert.ToInt32(rdr["RowId"]);
                    _purchaseOrderItem.PurchaseOrderDetailsId = Convert.ToInt32(rdr["PurchaseOrderDetailsId"]);
                    _purchaseOrderItem.PurchaseOrderId = Convert.ToInt32(rdr["PurchaseOrderId"]);
                    _purchaseOrderItem.ProductId = Convert.ToInt32(rdr["ProductId"]);
                    _purchaseOrderItem.SubProductId = Convert.ToInt32(rdr["SubProductId"]);
                    _purchaseOrderItem.Thickness = Convert.ToDecimal(rdr["Thickness"]);
                    _purchaseOrderItem.UnitId = Convert.ToInt32(rdr["UnitId"]);
                    _purchaseOrderItem.Quantity = Convert.ToDecimal(rdr["Quantity"]);
                    _purchaseOrderItem.UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]);
                    _purchaseOrderItem.Amount = Convert.ToDecimal(rdr["Amount"]);
                    _purchaseOrderItem.GSTTypeId = Convert.ToInt32(rdr["GSTTypeId"]);
                    _purchaseOrderItem.GSTAmount = Convert.ToDecimal(rdr["GSTAmount"]);
                    _purchaseOrderItem.GSTPercen = Convert.ToDecimal(rdr["GSTPercen"]);
                    _purchaseOrderItem.TotalAmount = Convert.ToDecimal(rdr["TotalAmount"]);
                    _purchaseOrderItem.Comment = rdr["Comment"].ToString();
                    _purchaseOrderItem.ProductName = rdr["ProductName"].ToString();
                    _purchaseOrderItem.SubProductName = rdr["SubProductName"].ToString();
                    _purchaseOrderItem.UnitName = rdr["UnitName"].ToString();
                    _purchaseOrderItem.GSTType = rdr["GSTType"].ToString();
                    _purchaseOrderItemList.Add(_purchaseOrderItem);
                }
                con.Close();
            }
            return _purchaseOrderItemList;
        }


        public IEnumerable<QuatationToPaymentDetailsViewModel> GetQuatationToPaymentListData(int ActionId, int QuatationToPaymentId, int CreatedBy)
        {
            List<QuatationToPaymentDetailsViewModel> _quatationToPaymentList = new List<QuatationToPaymentDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_QuatationToPaymentManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@QuatationToPaymentId", QuatationToPaymentId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    QuatationToPaymentDetailsViewModel _quatationToPayment = new QuatationToPaymentDetailsViewModel();

                    _quatationToPayment.QuatationToPaymentId = Convert.ToInt32(rdr["QuatationToPaymentId"]);
                    _quatationToPayment.UserId = Convert.ToInt32(rdr["UserId"]);
                    _quatationToPayment.UserName = rdr["UserName"].ToString();
                    _quatationToPayment.Date = rdr["Date"].ToString();
                    _quatationToPayment.QuatationFileName = rdr["QuatationFileName"].ToString();
                    _quatationToPayment.PIFileName = rdr["PIFileName"].ToString();
                    _quatationToPayment.POId =Convert.ToInt32( rdr["POId"]);
                    _quatationToPayment.PaymentId =Convert.ToInt32( rdr["PaymentId"]);
                    _quatationToPayment.IsQuatationComplete =Convert.ToBoolean( rdr["IsQuatationComplete"]);
                    _quatationToPayment.IsPOComplete =Convert.ToBoolean( rdr["IsPOComplete"]);
                    _quatationToPayment.IsPIComplete =Convert.ToBoolean( rdr["IsPIComplete"]);
                    _quatationToPayment.IsPaymentComplete =Convert.ToBoolean( rdr["IsPaymentComplete"]);
                    _quatationToPayment.IsVFinelBillComplete = Convert.ToBoolean( rdr["IsVFinelBillComplete"]);
                    _quatationToPayment.VFinelBillFileName = rdr["VFinelBillFileName"].ToString();
                    _quatationToPayment.QuatationAmount =Convert.ToDecimal(rdr["QuatationAmount"]);
                    _quatationToPayment.PIAmount =Convert.ToDecimal(rdr["PIAmount"]);
                    _quatationToPayment.AdvanceAmount =Convert.ToDecimal(rdr["AdvanceAmount"]);
                    _quatationToPayment.FinalAmount =Convert.ToDecimal(rdr["FinalAmount"]);

                    _quatationToPaymentList.Add(_quatationToPayment);
                }
                con.Close();
            }
            return _quatationToPaymentList;
        }

        public IEnumerable<QuatationToPaymentDetailsViewModel> GetVendorQuotationReportData(int ActionId, int QuatationToPaymentId, int CreatedBy)
        {
            List<QuatationToPaymentDetailsViewModel> _quatationToPaymentList = new List<QuatationToPaymentDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_QuatationToPaymentManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@QuatationToPaymentId", QuatationToPaymentId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    QuatationToPaymentDetailsViewModel _quatationToPayment = new QuatationToPaymentDetailsViewModel();

                   
                    _quatationToPayment.UserId = Convert.ToInt32(rdr["UserId"]);
                    _quatationToPayment.UserName = rdr["UserName"].ToString();                   
                    _quatationToPayment.QuatationAmount = Convert.ToDecimal(rdr["QuatationAmount"]);
                    _quatationToPayment.PIAmount = Convert.ToDecimal(rdr["PIAmount"]);
                    _quatationToPayment.AdvanceAmount = Convert.ToDecimal(rdr["AdvanceAmount"]);
                    _quatationToPayment.FinalAmount = Convert.ToDecimal(rdr["FinalAmount"]);

                    _quatationToPaymentList.Add(_quatationToPayment);
                }
                con.Close();
            }
            return _quatationToPaymentList;
        }

        public int AddUpdateQuatationToPaymentData(int ActionId, QuatationToPaymentDetailsViewModel _quatationToPayment)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_QuatationToPaymentManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@QuatationToPaymentId", _quatationToPayment.QuatationToPaymentId);
                cmd.Parameters.AddWithValue("@UserId", _quatationToPayment.UserId);
                cmd.Parameters.AddWithValue("@Date", _quatationToPayment.Date);
                cmd.Parameters.AddWithValue("@QuatationFileName", _quatationToPayment.QuatationFileName);
                cmd.Parameters.AddWithValue("@POId", _quatationToPayment.POId);
                cmd.Parameters.AddWithValue("@PIFileName", _quatationToPayment.PIFileName);
                cmd.Parameters.AddWithValue("@PaymentId", _quatationToPayment.PaymentId);              
                cmd.Parameters.AddWithValue("@IsQuatationComplete", _quatationToPayment.IsQuatationComplete);              
                cmd.Parameters.AddWithValue("@IsPOComplete", _quatationToPayment.IsPOComplete);              
                cmd.Parameters.AddWithValue("@IsPIComplete", _quatationToPayment.IsPIComplete);              
                cmd.Parameters.AddWithValue("@IsPaymentComplete", _quatationToPayment.IsPaymentComplete);              
                cmd.Parameters.AddWithValue("@CreatedBy", _quatationToPayment.CreatedBy);              
                cmd.Parameters.AddWithValue("@VFinelBillFileName", _quatationToPayment.VFinelBillFileName);              
                cmd.Parameters.AddWithValue("@IsVFinelBillComplete", _quatationToPayment.IsVFinelBillComplete);              
                cmd.Parameters.AddWithValue("@QuatationAmount", _quatationToPayment.QuatationAmount);              
                cmd.Parameters.AddWithValue("@PIAmount", _quatationToPayment.PIAmount);              
                cmd.Parameters.AddWithValue("@AdvanceAmount", _quatationToPayment.AdvanceAmount);              
                cmd.Parameters.AddWithValue("@FinalAmount", _quatationToPayment.FinalAmount);              

                con.Open();
               
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }
                con.Close();
            }
            return response;
        }

        public int AddUpdateInvoiceData(PaymentInvoiceViewModel _purchaseOrder, int ActionId)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InvoiceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@InvoiceId", _purchaseOrder.InvoiceId);
                cmd.Parameters.AddWithValue("@InvoiceNo", _purchaseOrder.InvoiceNo);
                //cmd.Parameters.AddWithValue("@PurchaseDate", _purchaseOrder.PurchaseDate);
                cmd.Parameters.AddWithValue("@InvoiceDate", SqlDbType.DateTime).Value = (_purchaseOrder.InvoiceDate == null) ? (object)DBNull.Value : Convert.ToDateTime(_purchaseOrder.InvoiceDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                cmd.Parameters.AddWithValue("@VendorId", _purchaseOrder.VendorId);
                cmd.Parameters.AddWithValue("@VendorAddress", _purchaseOrder.VendorAddress);
                cmd.Parameters.AddWithValue("@VendorEmail", _purchaseOrder.VendorEmail);
                cmd.Parameters.AddWithValue("@VendorGSTNo", _purchaseOrder.VendorGSTNo);
                cmd.Parameters.AddWithValue("@VendorMobileNo", _purchaseOrder.VendorMobileNo);
                cmd.Parameters.AddWithValue("@VendorCode", _purchaseOrder.VendorCode);
                cmd.Parameters.AddWithValue("@ShipTo", _purchaseOrder.ShipTo);
                cmd.Parameters.AddWithValue("@ShipToAddress", _purchaseOrder.ShipToAddress);
                cmd.Parameters.AddWithValue("@BillTo", _purchaseOrder.BillTo);
                cmd.Parameters.AddWithValue("@BillToAddress", _purchaseOrder.BillToAddress);
                cmd.Parameters.AddWithValue("@BillToGSTNo", _purchaseOrder.BillToGSTNo);
                cmd.Parameters.AddWithValue("@TermsConditions", _purchaseOrder.TermsConditions);
                cmd.Parameters.AddWithValue("@CreatedBy", _purchaseOrder.CreatedBy);
                cmd.Parameters.AddWithValue("@AdvancePaymentPercentage", _purchaseOrder.AdvancePaymentPercentage);

                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["Id"]);
                }
                con.Close();
            }
            return response;
        }

        public int AddUpdateInvoiceItemData(int ActionId, int InvoiceId, InvoiceItemDetailsViewModel _purchaseOrderItem)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InvoiceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@InvoiceItemDetailsId", _purchaseOrderItem.InvoiceItemDetailsId);
                cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);
                cmd.Parameters.AddWithValue("@ProductId", _purchaseOrderItem.ProductId);
                cmd.Parameters.AddWithValue("@SubProductId", _purchaseOrderItem.SubProductId);
                cmd.Parameters.AddWithValue("@Thickness", _purchaseOrderItem.Thickness);
                cmd.Parameters.AddWithValue("@UnitId", _purchaseOrderItem.UnitId);
                cmd.Parameters.AddWithValue("@Quantity", _purchaseOrderItem.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", _purchaseOrderItem.UnitPrice);
                cmd.Parameters.AddWithValue("@Amount", _purchaseOrderItem.Amount);
                cmd.Parameters.AddWithValue("@GSTTypeId", _purchaseOrderItem.GSTTypeId);
                cmd.Parameters.AddWithValue("@GSTPercen", _purchaseOrderItem.GSTPercen);
                cmd.Parameters.AddWithValue("@GSTAmount", _purchaseOrderItem.GSTAmount);
                cmd.Parameters.AddWithValue("@TotalAmount", _purchaseOrderItem.TotalAmount);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public IEnumerable<PaymentInvoiceViewModel> GetInvoiceListData(int ActionId, int InvoiceId, int CreatedBy)
        {
            List<PaymentInvoiceViewModel> _purchaseOrderList = new List<PaymentInvoiceViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InvoiceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    PaymentInvoiceViewModel _purchaseOrder = new PaymentInvoiceViewModel();

                    _purchaseOrder.InvoiceId = Convert.ToInt32(rdr["InvoiceId"]);
                    _purchaseOrder.InvoiceNo = rdr["InvoiceNo"].ToString();
                    // _purchaseOrder.PurchaseDate = rdr["PurchaseDate"].ToString();                   
                    _purchaseOrder.VendorId = Convert.ToInt32(rdr["VendorId"]);
                    if (!(rdr["InvoiceDate"] is DBNull))
                        _purchaseOrder.InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    _purchaseOrder.VendorName = rdr["VendorName"].ToString();
                    _purchaseOrder.VendorAddress = rdr["VendorAddress"].ToString();
                    _purchaseOrder.VendorEmail = rdr["VendorEmail"].ToString();
                    _purchaseOrder.VendorGSTNo = rdr["VendorGSTNo"].ToString();
                    _purchaseOrder.VendorMobileNo = rdr["VendorMobileNo"].ToString();
                    _purchaseOrder.VendorCode = rdr["VendorCode"].ToString();
                    _purchaseOrder.ShipTo = rdr["ShipTo"].ToString();
                    _purchaseOrder.ShipToAddress = rdr["ShipToAddress"].ToString();
                    _purchaseOrder.BillTo = rdr["BillTo"].ToString();
                    _purchaseOrder.BillToAddress = rdr["BillToAddress"].ToString();
                    _purchaseOrder.BillToGSTNo = rdr["BillToGSTNo"].ToString();
                    _purchaseOrder.TermsConditions = rdr["TermsConditions"].ToString();
                    _purchaseOrder.AdvancePaymentPercentage =Convert.ToDecimal(rdr["AdvancePaymentPercentage"]);


                    if (InvoiceId > 0)
                    {
                        _purchaseOrder.InvoiceItemDetails = GetInvoiceItemListData(4, InvoiceId, 0, 0).ToList();
                    }

                    _purchaseOrderList.Add(_purchaseOrder);
                }
                con.Close();
            }
            return _purchaseOrderList;
        }

        public IEnumerable<InvoiceItemDetailsViewModel> GetInvoiceItemListData(int ActionId, int InvoiceId, int InvoiceItemDetailsId, int CreatedBy)
        {
            List<InvoiceItemDetailsViewModel> _purchaseOrderItemList = new List<InvoiceItemDetailsViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InvoiceManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);
                cmd.Parameters.AddWithValue("@InvoiceItemDetailsId", InvoiceItemDetailsId);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    InvoiceItemDetailsViewModel _purchaseOrderItem = new InvoiceItemDetailsViewModel();

                    _purchaseOrderItem.RowId = Convert.ToInt32(rdr["RowId"]);
                    _purchaseOrderItem.InvoiceItemDetailsId = Convert.ToInt32(rdr["InvoiceItemDetailsId"]);
                    _purchaseOrderItem.InvoiceId = Convert.ToInt32(rdr["InvoiceId"]);
                    _purchaseOrderItem.ProductId = Convert.ToInt32(rdr["ProductId"]);
                    _purchaseOrderItem.SubProductId = Convert.ToInt32(rdr["SubProductId"]);
                    _purchaseOrderItem.Thickness = Convert.ToDecimal(rdr["Thickness"]);
                    _purchaseOrderItem.UnitId = Convert.ToInt32(rdr["UnitId"]);
                    _purchaseOrderItem.Quantity = Convert.ToDecimal(rdr["Quantity"]);
                    _purchaseOrderItem.UnitPrice = Convert.ToDecimal(rdr["UnitPrice"]);
                    _purchaseOrderItem.Amount = Convert.ToDecimal(rdr["Amount"]);
                    _purchaseOrderItem.GSTTypeId = Convert.ToInt32(rdr["GSTTypeId"]);
                    _purchaseOrderItem.GSTAmount = Convert.ToDecimal(rdr["GSTAmount"]);
                    _purchaseOrderItem.GSTPercen = Convert.ToDecimal(rdr["GSTPercen"]);
                    _purchaseOrderItem.TotalAmount = Convert.ToDecimal(rdr["TotalAmount"]);
                    _purchaseOrderItem.Comment = rdr["Comment"].ToString();
                    _purchaseOrderItem.ProductName = rdr["ProductName"].ToString();
                    _purchaseOrderItem.SubProductName = rdr["SubProductName"].ToString();
                    _purchaseOrderItem.UnitName = rdr["UnitName"].ToString();
                    _purchaseOrderItem.GSTType = rdr["GSTType"].ToString();
                    _purchaseOrderItemList.Add(_purchaseOrderItem);
                }
                con.Close();
            }
            return _purchaseOrderItemList;
        }

        public void EmailConfirmation(int ActionId, int TaskId, string Result, int CreatedBy)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_EmailConfirmation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", TaskId);
                cmd.Parameters.AddWithValue("@Result", Result);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //R--
        public void AddUpdateOtp(int ActionId, string Email, int Otp)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_OtpManagemnent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Otp", Otp);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
        }



        public int OtpVerification(int ActionId, string Email, string Otp)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_OtpManagemnent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Otp", Otp);
                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["UserId"]);
                }
                con.Close();
            }
            return response;
        }



        public int ChangePasswordManagement(int ActionId, ChangePassword changePassword)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_PasswordManagemnent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", changePassword.UserId);
                cmd.Parameters.AddWithValue("@Password", changePassword.NewPassword);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
        }

        public void EmailConfirmationLog(int ActionId, int TaskId, int AssignTo, string ToEmail, string FromEmail, string CcEmail,
            string BccEmail, string Subject, string MailBody, int CreatedBy, string Status, string Flag)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_EmailConfirmation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@TaskId", TaskId);
                cmd.Parameters.AddWithValue("@AssignTo", AssignTo);
                cmd.Parameters.AddWithValue("@ToEmail", ToEmail);
                cmd.Parameters.AddWithValue("@FromEmail", FromEmail);
                cmd.Parameters.AddWithValue("@CcEmail", CcEmail);
                cmd.Parameters.AddWithValue("@BccEmail", BccEmail);
                cmd.Parameters.AddWithValue("@Subject", Subject);
                cmd.Parameters.AddWithValue("@MailBody", MailBody);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Flag", Flag);
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        //--


        public IEnumerable<LogManagementViewModel> GetAllUserLogManagement(int ActionId, int UserId, int RoleId)
        {
            List<LogManagementViewModel> UserLogManagement = new List<LogManagementViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_LogManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@RoleId", RoleId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    LogManagementViewModel logManagement = new LogManagementViewModel();

                    logManagement.Id = Convert.ToInt32(rdr["Id"]);
                    logManagement.UserId = Convert.ToInt32(rdr["UserId"]);
                    logManagement.UserName = rdr["Name"].ToString();
                    logManagement.RoleName = rdr["RoleName"].ToString();
                    logManagement.Email = rdr["Email"].ToString();
                    logManagement.IpAddress = rdr["IpAddress"].ToString();
                    logManagement.LogDateTime = rdr["LogDateTime"].ToString();
                    logManagement.LoginStatus = rdr["LoginStatus"].ToString();

                    UserLogManagement.Add(logManagement);
                }
                con.Close();
            }
            return UserLogManagement;
        }

        public IEnumerable<EmailNotificationViewModel> GetEmailNotification(int actionId)
        {
            List<EmailNotificationViewModel> _notificationList = new List<EmailNotificationViewModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_EmailConfirmation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", actionId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();



                while (rdr.Read())
                {
                    EmailNotificationViewModel notification = new EmailNotificationViewModel();
                    notification.Id = Convert.ToInt32(rdr["Id"]);
                    notification.TaskName = rdr["TaskName"].ToString();
                    notification.AssignToName = rdr["AssignToName"].ToString();
                    notification.Email = rdr["Email"].ToString();
                    notification.Subject = rdr["Subject"].ToString();
                    notification.Status = rdr["Status"].ToString();
                    notification.CreatedDate = rdr["CreatedDate"].ToString();



                    _notificationList.Add(notification);
                }
                con.Close();
            }
            return _notificationList;
        }


        public string GetDocumentFileName(int actionId, int fileId)
        {
            string response = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetDocumentFileName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", actionId);
                cmd.Parameters.AddWithValue("@WarehouseId", fileId);
                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToString(resp["DocumentName"]);
                }
                con.Close();
            }
            return response;
        }

        public IEnumerable<WarehouseFileHistoryListViewModel> GetDocumentFileHistoryName(int actionId, int id)
        {
            List<WarehouseFileHistoryListViewModel> _fileHistoryList = new List<WarehouseFileHistoryListViewModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetDocumentFileName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", actionId);
                cmd.Parameters.AddWithValue("@WarehouseId", id);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    WarehouseFileHistoryListViewModel fileHistory = new WarehouseFileHistoryListViewModel();
                    fileHistory.FileId = Convert.ToInt32(rdr["Id"]);
                    fileHistory.WarehouseId = Convert.ToInt32(rdr["WarehouseId"]);
                    fileHistory.FileName = rdr["DocumentName"].ToString();
                    fileHistory.UploadedDate = rdr["CreatedDate"].ToString();
                    _fileHistoryList.Add(fileHistory);
                }
                con.Close();
            }
            return _fileHistoryList;
        }

        public int ValidateUserGeoLocation(int ActionId, LoginViewModel loginViewModel)
        {
            int response = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                cmd.Parameters.AddWithValue("@Email", loginViewModel.Username);
                cmd.Parameters.AddWithValue("@Password", loginViewModel.Password);
                cmd.Parameters.AddWithValue("@RoleId", loginViewModel.RoleId);
                cmd.Parameters.AddWithValue("@Lat", loginViewModel.Lat);
                cmd.Parameters.AddWithValue("@Long", loginViewModel.Long);
                
                con.Open();
                var resp = cmd.ExecuteReader();
                while (resp.Read())
                {
                    response = Convert.ToInt32(resp["DistanceStatus"]);
                    double Distance = Convert.ToDouble(resp["Distance"]);
                }
                con.Close();
            }
            return response;
        }
    }
}
