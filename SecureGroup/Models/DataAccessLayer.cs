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
    }
}
