using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using SecureGroup.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecureGroup.Controllers
{
    public class OperationController : BaseController
    {

        private readonly ILogger<OperationController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;
        private IWebHostEnvironment _env;
        public OperationController(ILogger<OperationController> logger, MsDBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
            _env = env;
        }

        public IActionResult ProjectsList()
        {

            List<ProjectViewModel> projectListViewModel = new List<ProjectViewModel>();
            projectListViewModel = _dataAccessLayer.GetProjectsListData(3, 0).ToList();

            return View(projectListViewModel);
        }
        public IActionResult AssignedProjectsList()
        {
            List<ProjectViewModel> projectListViewModel = new List<ProjectViewModel>();
            int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                projectListViewModel = _dataAccessLayer.GetAssignedProjectsListData(5, UserId).ToList();
            }

            return View(projectListViewModel);
        }

        public ActionResult _assignedProjectsTaskUpdatePartial(int Id)
        {
            List<ProjectViewModel> projectListViewModel = new List<ProjectViewModel>();
            int UserId = GetUserSession().UserId;
            if (UserId > 0)
            {
                projectListViewModel = _dataAccessLayer.GetAssignedProjectsTaskListData(6, UserId, Id).ToList();
            }
            return PartialView("_assignedProjectsTaskUpdatePartial", projectListViewModel);
        }

        [HttpGet]
        public IActionResult AddProjects()
        {
            ProjectViewModel _projectViewModel = new ProjectViewModel();
            _projectViewModel.StateList = _dataAccessLayerLinq.GetDropDownListData("State", 101);
            _projectViewModel.CityList = _dataAccessLayerLinq.GetDropDownListData("City", 0);
            _projectViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("UserByRole", 4);
            _projectViewModel.SchemeList = _dataAccessLayerLinq.GetDropDownListData("Scheme", 0);
            _projectViewModel.StatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "ProjectStatus");
            _projectViewModel.ProjectStartDate = DateTime.Now;
            _projectViewModel.ProjectDueDate = DateTime.Now;
            _projectViewModel.CountryId = 101; //India

            return View(_projectViewModel);
        }

        [HttpPost]
        public IActionResult AddProjects(ProjectViewModel _projectViewModel)
        {
            try
            {
                int response = 0;
                _projectViewModel.CreatedBy = GetUserSession().UserId;

                response = _dataAccessLayer.AddUpdateProjectsData(_projectViewModel, 1);

                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(ProjectsList));
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

            return RedirectToAction("ProjectsList");
        }


        [HttpGet]
        public IActionResult EditProjects(int Id)
        {
            ProjectViewModel _projectViewModel = new ProjectViewModel();
            _projectViewModel = _dataAccessLayer.GetProjectsListData(3, Id).FirstOrDefault();
            _projectViewModel.StateList = _dataAccessLayerLinq.GetDropDownListData("State", 101);
            _projectViewModel.CityList = _dataAccessLayerLinq.GetDropDownListData("City", _projectViewModel.StateId);
            _projectViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("UserByRole", 4);
            _projectViewModel.SchemeList = _dataAccessLayerLinq.GetDropDownListData("Scheme", 0);
            _projectViewModel.StatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "ProjectStatus");
            if (_projectViewModel.ProjectDueDate == null)
            {
                _projectViewModel.ProjectDueDate = DateTime.Now;
            }

            return View(_projectViewModel);
        }

        [HttpPost]
        public IActionResult EditProjects(ProjectViewModel _projectViewModel)
        {
            try
            {
                int response = 0;
                _projectViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateProjectsData(_projectViewModel, 2);

                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(ProjectsList));
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

            return RedirectToAction("ProjectsList");
        }

        public IActionResult DeleteProjects(int Id)
        {
            int response = 0;
            try
            {
                ProjectViewModel _projectViewModel = new ProjectViewModel();
                _projectViewModel = _dataAccessLayer.GetProjectsListData(3, Id).FirstOrDefault();

                _projectViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateProjectsData(_projectViewModel, 4);

                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(ProjectsList));
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
            return RedirectToAction(nameof(ProjectsList));
        }

        public IActionResult SitesList()
        {

            List<SiteViewModel> siteViewModel = new List<SiteViewModel>();
            siteViewModel = _dataAccessLayer.GetSiteListData(3, 0).ToList();

            return View(siteViewModel);
        }
        public IActionResult AddSites()
        {

            SiteViewModel siteViewModel = new SiteViewModel();
            siteViewModel.StateList = _dataAccessLayerLinq.GetDropDownListData("State", 101);
            siteViewModel.CityList = _dataAccessLayerLinq.GetDropDownListData("City", 0);

            return View(siteViewModel);
        }
        [HttpPost]
        public IActionResult AddSites(SiteViewModel siteViewModel)
        {
            try
            {
                int Response = 0;
                Response = _dataAccessLayer.AddUpdateSiteData(siteViewModel, 1);
                if (Response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(SitesList));
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
            return RedirectToAction("SitesList");
        }

        public IActionResult EditSites(int Id)
        {

            SiteViewModel siteViewModel = new SiteViewModel();
            siteViewModel = _dataAccessLayer.GetSiteListData(3, Id).FirstOrDefault();
            siteViewModel.StateList = _dataAccessLayerLinq.GetDropDownListData("State", 101);
            siteViewModel.CityList = _dataAccessLayerLinq.GetDropDownListData("City", siteViewModel.StateId);

            return View(siteViewModel);
        }

        [HttpPost]
        public IActionResult EditSites(SiteViewModel siteViewModel)
        {
            try
            {
                int Response = 0;
                Response = _dataAccessLayer.AddUpdateSiteData(siteViewModel, 2);
                if (Response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(SitesList));
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
            return RedirectToAction("SitesList");
        }

        public IActionResult DeleteSites(int Id)
        {

            try
            {
                SiteViewModel siteViewModel = new SiteViewModel();
                int Response = 0;
                siteViewModel.SiteId = Id;
                Response = _dataAccessLayer.AddUpdateSiteData(siteViewModel, 4);
                if (Response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(SitesList));
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
            return RedirectToAction("SitesList");
        }

        [HttpGet]
        public IActionResult TaskAllocation(int TaskId)
        {
            List<TaskViewModel> _taskListViewModel = new List<TaskViewModel>();
            var user = GetUserSession();
            if (user != null)
            {
                TempData["userRole"] = user.RoleId.ToString();
                if (user.RoleId == 4)
                {

                    _taskListViewModel = _dataAccessLayer.GetAllTaskAllocation(4, 0, user.UserId, 0).ToList(); //Get All task (ActionId,TaskId,AssignTo,CreatedBy)
                }
                else
                {
                    _taskListViewModel = _dataAccessLayer.GetAllTaskAllocation(4, 0, 0, 0).ToList(); //Get All task (ActionId,TaskId,AssignTo,CreatedBy)
                }
            }
            else
            {
                _taskListViewModel = _dataAccessLayer.GetAllTaskAllocation(4, TaskId, user.UserId, 0).ToList(); //Get All task (ActionId,TaskId,AssignTo,CreatedBy)
            }
            if (TaskId > 0)
            {
                TempData["TaskId"] = TaskId;
                //_taskListViewModel[0].TaskUpdate.TaskId=TaskId;
            }
            else
            {
                TempData["TaskId"] = 0;
            }
            if(_taskListViewModel.Count>0)
            {
                _taskListViewModel[0].TaskStatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "TaskStatus");
            }else
            {
                return RedirectToAction("NoTaskAllocation");
            }
          



            return View(_taskListViewModel);


        }

        public IActionResult NoTaskAllocation()
        {
            TaskViewModel _taskViewModel = new TaskViewModel();
            try
            {

                _taskViewModel.TaskStatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "TaskStatus");
                _taskViewModel.ProjectList = _dataAccessLayerLinq.GetDropDownListData("Project", 0);
                _taskViewModel.SiteList = _dataAccessLayerLinq.GetDropDownListData("Site", 0);
                _taskViewModel.ProjectHeadList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _taskViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("UserByRole", 4);

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_taskViewModel);
        }

        public ActionResult _taskUpdateListPartial(int Id)
        {
            List<TaskUpdateViewModel> _taskUpdateList = new List<TaskUpdateViewModel>();
            _taskUpdateList = _dataAccessLayer.GetAllTaskUpdateList(5, Id, 0, 0).ToList();
            return PartialView("_taskUpdateListPartial", _taskUpdateList);
        }

        public ActionResult _taskDetailsPartial(int Id)
        {
            TaskViewModel _taskViewModel = new TaskViewModel();
            _taskViewModel = _dataAccessLayer.GetAllTaskAllocation(4, Id, 0, 0).FirstOrDefault(); //Get All task (ActionId,TaskId,AssignTo,CreatedBy)

            return PartialView("_taskDetailsPartial", _taskViewModel);
        }

        public IActionResult AssignTask()
        {
            TaskViewModel _taskViewModel = new TaskViewModel();
            try
            {

                _taskViewModel.TaskStatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "TaskStatus");
                _taskViewModel.ProjectList = _dataAccessLayerLinq.GetDropDownListData("Project", 0);
                _taskViewModel.SiteList = _dataAccessLayerLinq.GetDropDownListData("Site", 0);
                _taskViewModel.ProjectHeadList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _taskViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("UserByRole", 4);

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_taskViewModel);
        }


        [HttpPost]
        public IActionResult AssignTask(TaskViewModel _taskViewModel)
        {
            try
            {
                int response = 0;
                if (_taskViewModel.TaskDocument != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_taskViewModel.TaskDocument, "Upload/Task");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _taskViewModel.TaskDocumentName = _uploadFileResponse.FileName;
                        }
                    }
                }

                _taskViewModel.CreatedBy = GetUserSession().UserId;

                response = _dataAccessLayer.AddUpdateAssignTaskData(_taskViewModel, 1);

                if (response > 0)
                {
                    var user_Details = _dataAccessLayer.GetAllUser(4, _taskViewModel.AssignTo, 0).FirstOrDefault();
                    var task_details = _dataAccessLayer.GetAllTaskAllocation(4, response, 0, 0).FirstOrDefault();
                    var pathToFile = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Templates"
                             + Path.DirectorySeparatorChar.ToString()
                             + "EmailTemplate"
                             + Path.DirectorySeparatorChar.ToString()
                             + "TaskAssignEmail.html";

                    string HtmlBody = string.Empty;
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        HtmlBody = SourceReader.ReadToEnd();
                    }

                    string mailBody = string.Format(HtmlBody,
                       task_details.AssignToName,
                       task_details.TaskName,
                       task_details.ProjectName,
                       task_details.SiteName,
                       task_details.TaskPriority,
                       task_details.TaskDescription,
                       task_details.TaskStatus,
                       string.Format("{0:dddd, d MMMM yyyy}", task_details.TaskDueDateTime)
                       );
                    string subject = "New Task Assigned";

                    var result = sendEmail("New Task Assigned", mailBody,
                                            "crmsifsl@gmail.com", user_Details.Email, "", "");
                    if (result)
                    {
                        // _dataAccessLayer.EmailConfirmation(1, _taskViewModel.TaskId, "success", _taskViewModel.CreatedBy);
                        _dataAccessLayer.EmailConfirmationLog(1, _taskViewModel.TaskId, _taskViewModel.AssignTo, user_Details.Email, "crmsifsl@gmail.com", "", "", subject, mailBody, _taskViewModel.CreatedBy, "succeed", "New Task Email");
                    }
                    else
                    {
                        _dataAccessLayer.EmailConfirmationLog(1, _taskViewModel.TaskId, _taskViewModel.AssignTo, user_Details.Email, "crmsifsl@gmail.com", "", "", subject, mailBody, _taskViewModel.CreatedBy, "failed", "New Task Email");
                    }


                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(TaskAllocation));
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

                //var path = Path.Combine(
                //            Directory.GetCurrentDirectory(), "wwwroot",
                //            TaskDocument.FileName);

                //using (var stream = new FileStream(path, FileMode.Create))
                //{
                //    TaskDocument.CopyToAsync(stream);
                //}
                //return Content("file not selected");
                //DateTime myDT = Convert.ToDateTime("2023-07-05T13:33");
                //String myDate = myDT.ToLongDateString();
                //String myTime = myDT.ToString("hh:mm tt");

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return RedirectToAction("AssignTask");
            //return View();
        }


        public IActionResult EditAssignTask(int Id)
        {
            TaskViewModel _taskViewModel = new TaskViewModel();
            try
            {

                _taskViewModel = _dataAccessLayer.GetAllTaskAllocation(4, Id, 0, 0).FirstOrDefault();
                _taskViewModel.TaskStatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "TaskStatus");
                _taskViewModel.ProjectList = _dataAccessLayerLinq.GetDropDownListData("Project", 0);
                _taskViewModel.SiteList = _dataAccessLayerLinq.GetDropDownListData("Site", 0);
                _taskViewModel.ProjectHeadList = _dataAccessLayerLinq.GetDropDownListData("User", 0);
                _taskViewModel.UserList = _dataAccessLayerLinq.GetDropDownListData("UserByRole", 4);

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_taskViewModel);
        }


        [HttpPost]
        public IActionResult EditAssignTask(TaskViewModel _taskViewModel)
        {
            try
            {
                int response = 0;
                if (_taskViewModel.TaskDocument != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_taskViewModel.TaskDocument, "Upload/Task");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _taskViewModel.TaskDocumentName = _uploadFileResponse.FileName;
                        }
                    }
                }

                _taskViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateAssignTaskData(_taskViewModel, 2);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been updated successfully";
                    return RedirectToAction(nameof(TaskAllocation));
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
            return RedirectToAction("AssignTask");
            //return View();
        }
        public IActionResult DeleteAssignTask(int Id)
        {
            int response = 0;
            TaskViewModel _taskViewModel = new TaskViewModel();
            try
            {

                _taskViewModel = _dataAccessLayer.GetAllTaskAllocation(4, Id, 0, 0).FirstOrDefault();
                _taskViewModel.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateAssignTaskData(_taskViewModel, 3);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(TaskAllocation));
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
            return View(_taskViewModel);
        }
        public IActionResult DownloadFile(string filename)
        {

            // Since this is just and example, I am using a local file located inside wwwroot
            //Afterwards file is converted into a stream
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Task", filename);
            //var path = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.xlsx");
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", filename);
            // call GetFile Method in service and return

            // var dd= FileDownload(filename, "Upload");

        }
        public IActionResult DocumentList()
        {
            //FileDownload("New Text Document.txt");
            return View();
        }

        [HttpPost]
        public IActionResult UserUpdateAssignTask(TaskViewModel _taskViewModel)
        {
            int response = 0, TaskId = 0;
            try
            {

                if (_taskViewModel.TaskUpdate.TaskDocument != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_taskViewModel.TaskUpdate.TaskDocument, "Upload/Task");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _taskViewModel.TaskUpdate.TaskDocumentName = _uploadFileResponse.FileName;
                        }
                    }
                }
                TaskId = _taskViewModel.TaskUpdate.TaskId;
                _taskViewModel.TaskUpdate.CreatedBy = GetUserSession().UserId;


                response = _dataAccessLayer.AddUpdateTaskUpdateData(_taskViewModel.TaskUpdate, 6);
                if (response > 0)
                {
                    TaskAllocationEmailManagement(_taskViewModel.AssignTo, TaskId, _taskViewModel.TaskUpdate.CreatedBy, "Assigned Task Updated", _taskViewModel);
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction("TaskAllocation", new { TaskId = TaskId });
                    //return RedirectToAction(nameof(TaskAllocation), _taskViewModel.TaskUpdate.TaskId);
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
            return RedirectToAction("TaskAllocation", new { TaskId = TaskId });
            //return Json("Success");

        }


        public string getEmailHtmlTemplate()
        {
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "Templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "TaskAssignEmailUpdate.html";
            string HtmlBody = string.Empty;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                HtmlBody = SourceReader.ReadToEnd();
            }
            return HtmlBody;
        }
        public void TaskAllocationEmailManagement(int assignTo, int taskId, int createdBy, string subject, TaskViewModel _taskViewModel)
        {
            var task_details = _dataAccessLayer.GetAllTaskAllocation(4, taskId, 0, 0).FirstOrDefault();
            var user_Details = _dataAccessLayer.GetAllUser(4, task_details.AssignTo, 0).FirstOrDefault();
            string HtmlBody = getEmailHtmlTemplate();
            string mailBody = string.Format(HtmlBody,
               task_details.AssignToName,
               task_details.TaskName,
               task_details.ProjectName,
               task_details.SiteName,
               task_details.TaskPriority,
               task_details.TaskDescription,
               task_details.TaskStatus,
               string.Format("{0:dddd, d MMMM yyyy}", task_details.TaskDueDateTime),
               _taskViewModel.TaskUpdate.SpentTime,
               _taskViewModel.TaskUpdate.TaskStatus,
               _taskViewModel.TaskUpdate.Comment
               );



            var result = sendEmail(subject, mailBody,
                                    "crmsifsl@gmail.com", user_Details.Email, "", "");
            if (result)
            {
                _dataAccessLayer.EmailConfirmationLog(1, taskId, assignTo, user_Details.Email, "crmsifsl@gmail.com", "", "", subject, mailBody, createdBy, "succeed", "Task Updated Email");
            }
            else
            {
                _dataAccessLayer.EmailConfirmationLog(1, taskId, assignTo, user_Details.Email, "crmsifsl@gmail.com", "", "", subject, mailBody, createdBy, "failed", "Task Updated Email");
            }
        }

    }
}
