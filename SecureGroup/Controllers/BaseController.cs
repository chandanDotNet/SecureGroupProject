using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Data;
using System.Reflection;

namespace SecureGroup.Controllers
{
    public abstract class BaseController : Controller
    {
        public void InsertUserSession(UserViewModel _userViewModel)
        {
            HttpContext.Session.SetString("UserID", Convert.ToString(_userViewModel.UserId));
            HttpContext.Session.SetString("RoleID", Convert.ToString(_userViewModel.RoleId));
            HttpContext.Session.SetString("Email", Convert.ToString(_userViewModel.Email));
            HttpContext.Session.SetString("Name", Convert.ToString(_userViewModel.Name));
            HttpContext.Session.SetString("RoleName", Convert.ToString(_userViewModel.RoleName));

        }

        public UserViewModel GetUserSession()
        {
            UserViewModel _userViewModel = new UserViewModel();
            _userViewModel.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            _userViewModel.RoleId = Convert.ToInt32(HttpContext.Session.GetString("RoleID"));
            _userViewModel.Email = HttpContext.Session.GetString("Email");
            _userViewModel.Name = HttpContext.Session.GetString("Name");
            _userViewModel.RoleName = HttpContext.Session.GetString("RoleName");

            return _userViewModel;
        }

        // Upload file on server
        public UploadFileResponseViewModel UploadFile(IFormFile file, string fileLocation)
        {
            UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
            string path = "", filename = string.Empty;
            bool iscopied = false;
            try
            {
                if (file.Length > 0)
                {
                    filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), fileLocation));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else
                {
                    iscopied = false;
                }

            }
            catch (Exception ex)
            {
                _uploadFileResponse.ErrorMessage = ex.Message;
            }

            _uploadFileResponse.UploadSuccess = iscopied;
            _uploadFileResponse.FileName = filename;
            _uploadFileResponse.FilePath = path;

            return _uploadFileResponse;
        }

        public async Task<IActionResult> FileDownload(string filename, string fileLocation)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), fileLocation, filename);
            var fs = new FileStream(path, FileMode.Open);
            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            return File(fs, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }



        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }






    }
}
