using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using SecureGroup.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using SecureGroup.Controllers;

namespace SecureGroup.Library
{
    public class UserSession :BaseController
    {
        private static HttpContext _httpContext;
        public static void InsertUserSession(UserViewModel _userViewModel, HttpContext _httpContext1)
        {
           
            try
            {
                // Get the bytes of the string
                if(_userViewModel!=null)
                {
                    _httpContext1.Session.SetString("UserID", Convert.ToString(_userViewModel.UserId));
                    _httpContext.Session.SetString("UserID", Convert.ToString(_userViewModel.UserId));
                    _httpContext.Session.SetString("RoleID", Convert.ToString(_userViewModel.RoleId));
                    _httpContext.Session.SetString("Email", Convert.ToString(_userViewModel.Email));
                    _httpContext.Session.SetString("Name", Convert.ToString(_userViewModel.Name));
                }                

               
            }
            catch (Exception)
            {
                throw;
            }
        }
        public  UserViewModel GetAllSession()
        {
            
            UserViewModel _userViewModel = new UserViewModel();
            try
            {
                // Get the bytes of the string
                _userViewModel.UserId = GetUserSession().UserId;
                    //_userViewModel.UserId = Convert.ToInt32(_httpContext.Session.GetString("UserID"));
                    //_userViewModel.RoleId=Convert.ToInt32(_httpContext.Session.GetString("RoleID"));
                    //_userViewModel.Email = httpContext.Session.GetString("Email");
                    //_userViewModel.Name=httpContext.Session.GetString("Name");



                return _userViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
