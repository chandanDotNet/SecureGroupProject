using Microsoft.Data.SqlClient;
using SecureGroup.Utility;
using System.Collections.Generic;
using System.Data;
using System;
using SecureGroup.ViewModel;
using Azure;
using System.Diagnostics.Metrics;

namespace SecureGroup.Models
{
    public class DataAccessLayer
    {



        string connectionString = ConnectionString.CName;


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
                    student.Name = rdr["BusinessName"].ToString();
                    student.Name = rdr["Name"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.Password = rdr["Password"].ToString();
                    student.ContactNo = rdr["ContactNo"].ToString();
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

                    lstUser.Add(student);
                }
                con.Close();
            }
            return lstUser;
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

        public void AddWarehouse(WarehouseViewModel warehouseViewModel,int ActionId)
        {
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
                //cmd.Parameters.AddWithValue("@StartDate", warehouseViewModel.St);                

                con.Open();
                var aa= cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddProductItem(WHStockProductViewModel wHStockProduct,int ActionId)
        {
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
            
                //cmd.Parameters.AddWithValue("@StartDate", warehouseViewModel.St);                

                con.Open();
                var aa = cmd.ExecuteNonQuery();
                con.Close();
            }
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
                cmd.Parameters.AddWithValue("@RoleId", userViewModel.RoleId);
                cmd.Parameters.AddWithValue("@CreatedBy", userViewModel.CreatedBy);
                cmd.Parameters.AddWithValue("@JobTitle", userViewModel.JobTitle);
                cmd.Parameters.AddWithValue("@UserCode", userViewModel.UserCode);
                cmd.Parameters.AddWithValue("@DepartmentId", userViewModel.DepartmentId);
                cmd.Parameters.AddWithValue("@ReportingTo", userViewModel.ReportingTo);
                cmd.Parameters.AddWithValue("@LoginTime", userViewModel.LoginTime);
                cmd.Parameters.AddWithValue("@LogoutTime", userViewModel.LogoutTime);
                cmd.Parameters.AddWithValue("@IsFlexibleTime", userViewModel.IsFlexibleTime);

                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            return response;
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
    }
}
