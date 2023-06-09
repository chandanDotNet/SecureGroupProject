using Microsoft.Data.SqlClient;
using SecureGroup.Utility;
using System.Collections.Generic;
using System.Data;
using System;
using SecureGroup.ViewModel;

namespace SecureGroup.Models
{
    public class DataAccessLayer
    {



        string connectionString = ConnectionString.CName;


        public IEnumerable<UserViewModel> GetAllUser()
        {
            List<UserViewModel> lstUser = new List<UserViewModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("uspUserDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserViewModel student = new UserViewModel();

                    student.UserId = Convert.ToInt32(rdr["UserId"]);
                    student.Name = rdr["Name"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.Password = rdr["Password"].ToString();
                    student.IsActive = (Boolean)rdr["IsActive"];


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

        public void AddWarehouse(WarehouseViewModel warehouseViewModel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_WarehouseManagement", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@WarehouseId", 1);
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
    }
}
