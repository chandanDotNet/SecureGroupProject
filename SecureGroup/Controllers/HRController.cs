using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
//using SecureGroup.Interface;
using SecureGroup.Models;
using SecureGroup.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SecureGroup.Controllers
{
    public class HRController : BaseController
    {

        //IHR _IHR;
        private readonly ILogger<HRController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;


        public HRController(ILogger<HRController> logger, MsDBContext context)
        {
            //_IHR = IHR;
            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);

        }

        public IActionResult Attendance(Boolean? IsMyAttendance)
        {

            //  var dataTest=_IHR.GetUserAttendanceList(4, 0, 0, 0).ToList();
            // var  data2 = _IHR.GetDropDownListData("SysVal", 0, "AttendanceStatus");

            if (IsMyAttendance == null)
            {
                IsMyAttendance = false;
            }

            AttendanceRaperViewModel attendanceRaperViewModel = new AttendanceRaperViewModel();
            List<AttendanceViewModel> _attendanceListViewModel = new List<AttendanceViewModel>();
            UserViewModel _userViewModel = new UserViewModel();
            var UserDetails = GetUserSession();
            if (UserDetails != null)
            {
                _userViewModel = UserDetails;
                attendanceRaperViewModel.UserId = UserDetails.UserId;
                attendanceRaperViewModel.RoleId = UserDetails.RoleId;
            }

            if (_userViewModel.RoleId == 1 && IsMyAttendance == true)
            {
                _attendanceListViewModel = _dataAccessLayer.GetUserAttendanceList(4, 0, _userViewModel.UserId, 0).ToList(); //int ActionId, int AttendanceId, int UserId, int CreatedBy
            }
            else if (_userViewModel.RoleId == 1 && IsMyAttendance == false)
            {
                _attendanceListViewModel = _dataAccessLayer.GetUserAttendanceList(4, 0, 0, 0).ToList(); //int ActionId, int AttendanceId, int UserId, int CreatedBy
            }
            else
            {
                _attendanceListViewModel = _dataAccessLayer.GetUserAttendanceList(4, 0, _userViewModel.UserId, 0).ToList(); //int ActionId, int AttendanceId, int UserId, int CreatedBy
            }


            attendanceRaperViewModel.attendanceListViewModel = _attendanceListViewModel;
            attendanceRaperViewModel.YearList = _dataAccessLayerLinq.GetDropDownListData("Year", 0);
            attendanceRaperViewModel.MonthList = _dataAccessLayerLinq.GetDropDownListData("Month", 0);
            return View(attendanceRaperViewModel);
        }
        public IActionResult WorkingHour()
        {
            return View();
        }
        public IActionResult LeaveApply(int? LeaveId)
        {
            int UserId = 0;
            UserViewModel _userViewModel = new UserViewModel();
            var UserDetails = GetUserSession();
            if (UserDetails != null)
            {
                _userViewModel = UserDetails;
            }
            if (_userViewModel.RoleId == 1)
            {
                UserId = 0;
            }
            else
            {
                UserId = _userViewModel.UserId;
            }

            LeaveApplyViewModel leaveApplyViewModel = new LeaveApplyViewModel();
            leaveApplyViewModel.LeaveApplyHistory = _dataAccessLayer.GetUserLeaveList(2, 0, UserId, 0).ToList();


            return View(leaveApplyViewModel);
        }

        [HttpPost]
        public IActionResult LeaveApply(LeaveApplyViewModel _leaveApplyViewModel)
        {

            try
            {
                int response = 0;
                if (_leaveApplyViewModel.Document != null)
                {
                    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                    _uploadFileResponse = UploadFile(_leaveApplyViewModel.Document, "Upload");
                    if (_uploadFileResponse != null)
                    {
                        if (_uploadFileResponse.UploadSuccess == true)
                        {
                            _leaveApplyViewModel.DocumentName = _uploadFileResponse.FileName;
                        }
                    }
                }
                if (_leaveApplyViewModel.LeaveTypeValue == null)
                {
                    _leaveApplyViewModel.LeaveTypeValue = "0";
                }

                _leaveApplyViewModel.UserId = GetUserSession().UserId;

                response = _dataAccessLayer.AddUpdateUserLeaveData(_leaveApplyViewModel, 1);

                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(LeaveApply));
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

            return RedirectToAction("LeaveApply");

            //return View(leaveApplyViewModel);
        }


        [HttpGet]
        public IActionResult LeaveApproval()
        {
            List<LeaveApplyHistory> _leaveApproval = new List<LeaveApplyHistory>();
            try
            {
                int UserId = 0;
                UserViewModel _userViewModel = new UserViewModel();
                var UserDetails = GetUserSession();
                if (UserDetails != null)
                {
                    _userViewModel = UserDetails;
                }
                if (_userViewModel.RoleId == 1)
                {
                    UserId = 0;
                    _leaveApproval = _dataAccessLayer.GetUserLeaveList(2, 0, UserId, 0).ToList();
                }
                else
                {
                    UserId = _userViewModel.UserId;
                    _leaveApproval = _dataAccessLayer.GetUserLeaveList(6, 0, UserId, 0).ToList();
                }
                
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return View(_leaveApproval);
        }

        [HttpGet]
        public IActionResult ApproveRejectLeave(int Id, int StatusId)
        {
            int response = 0;
            try
            {
                LeaveApplyViewModel _leaveApplyViewModel = new LeaveApplyViewModel();
                _leaveApplyViewModel.LeaveApproveRejectBy = GetUserSession().UserId;
                _leaveApplyViewModel.LeaveId = Id;
                _leaveApplyViewModel.LeaveStatus = StatusId;


                response = _dataAccessLayer.AddUpdateUserLeaveData(_leaveApplyViewModel, 3);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(LeaveApproval));
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
            return RedirectToAction("LeaveApproval");

        }

        [HttpGet]
        public IActionResult ApproveRejectLeaveByReportingUser(int Id, int StatusId)
        {
            int response = 0;
            try
            {
                LeaveApplyViewModel _leaveApplyViewModel = new LeaveApplyViewModel();
                _leaveApplyViewModel.LeaveApproveRejectBy = GetUserSession().UserId;
                _leaveApplyViewModel.LeaveId = Id;
                _leaveApplyViewModel.LeaveStatus = StatusId;


                response = _dataAccessLayer.AddUpdateUserLeaveData(_leaveApplyViewModel, 5);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(LeaveApproval));
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
            return RedirectToAction("LeaveApproval");

        }

        [HttpGet]
        public IActionResult DeleteLeave(int LeaveId)
        {
            int response = 0;
            try
            {
                LeaveApplyViewModel _leaveApplyViewModel = new LeaveApplyViewModel();
                _leaveApplyViewModel.LeaveApproveRejectBy = GetUserSession().UserId;
                _leaveApplyViewModel.LeaveId = LeaveId;


                response = _dataAccessLayer.AddUpdateUserLeaveData(_leaveApplyViewModel, 4);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been deleted successfully";
                    return RedirectToAction(nameof(LeaveApproval));
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
            return RedirectToAction("LeaveApproval");

        }

        [HttpGet]
        public IActionResult ApproveRejectAttendance(int Id, int StatusId)
        {
            int response = 0;
            try
            {
                AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
                attendanceViewModel.AttendanceApproveRejectBy = GetUserSession().UserId;
                attendanceViewModel.UserId = GetUserSession().UserId;
                attendanceViewModel.AttendanceId = Id;
                attendanceViewModel.AttendanceStatusId = StatusId;
                response = _dataAccessLayer.AddUpdateUserAttendanceeData(attendanceViewModel, 7);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Attendance));
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
            return RedirectToAction("Attendance");

        }



        [HttpPost]
        public IActionResult DeviationsAttendancePartial(AttendanceRaperViewModel _attendance)
        {

            int response = 0;
            try
            {
                //************** Calculate Hours Duration ************

                DateTime date = DateTime.Parse(_attendance.DeviationsLoginTime, System.Globalization.CultureInfo.CurrentCulture);
                DateTime date2 = DateTime.Parse(_attendance.DeviationsLogoutTime, System.Globalization.CultureInfo.CurrentCulture);

                string a = (date - date2).ToString();

                AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
                attendanceViewModel.CreatedBy = GetUserSession().UserId;
                attendanceViewModel.UserId = GetUserSession().UserId;
                attendanceViewModel.Reason = _attendance.Reason;
                attendanceViewModel.DeviationsLoginTime = _attendance.DeviationsLoginTime;
                attendanceViewModel.DeviationsLogoutTime = _attendance.DeviationsLogoutTime;
                attendanceViewModel.AttendanceId = _attendance.AttendanceId;
                attendanceViewModel.DeviationsDurationTime = (date2 - date).ToString();
                //TimeSpan sincelast = TimeSpan.FromTicks(_attendance.DeviationsLoginTime);

                response = _dataAccessLayer.AddUpdateUserAttendanceeData(attendanceViewModel, 5);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Attendance));
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
            return RedirectToAction("Attendance");
        }


        [HttpPost]
        public ActionResult AllApproveAttendance(string[] ids)
        {
            //string[] ids = formCollection["AttendanceId"];
            int response = 0;

            if (ids.Length > 0)
            {

                try
                {
                    AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
                    attendanceViewModel.AttendanceApproveRejectBy = GetUserSession().UserId;
                    attendanceViewModel.UserId = GetUserSession().UserId;
                    attendanceViewModel.DeviationsApprovalStatusId = 13;
                    foreach (string id in ids)
                    {
                        attendanceViewModel.AttendanceId = Convert.ToInt32(id);
                        response = _dataAccessLayer.AddUpdateUserAttendanceeData(attendanceViewModel, 6);

                    }


                    if (response > 0)
                    {
                        TempData["successmessage"] = "Your data has been saved successfully";
                        return RedirectToAction(nameof(Attendance));
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
            }
            else
            {
                TempData["errormessage"] = "Something went wrong! Please select rows.";
            }

            return RedirectToAction("Attendance");
        }


        [HttpGet]
        public IActionResult Reimbursement()
        {

            WrapperReimbursementViewModel _wrapperReimbursement = new WrapperReimbursementViewModel();
            List<ReimbursementViewModel> _reimbursementList = new List<ReimbursementViewModel>();
            int UserId = 0, RoleId = 0;
            try
            {
                if (GetUserSession().RoleId == 1)
                {
                    UserId = 0;
                }
                if (GetUserSession().RoleId == 4)
                {
                    UserId = GetUserSession().UserId;
                }

                _reimbursementList = _dataAccessLayer.GetReimbursementData(3, 0, UserId, 0).ToList();
                _wrapperReimbursement.ReimbursementList = _reimbursementList;
                _wrapperReimbursement.StatusList = _dataAccessLayerLinq.GetDropDownListData("SysVal", 0, "ReimbursementStatus");

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_wrapperReimbursement);

        }

        public IActionResult AddReimbursement()
        {
            int response = 0;
            ReimbursementViewModel _reimbursement = new ReimbursementViewModel();
            try
            {


            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_reimbursement);

        }

        [HttpPost]
        public IActionResult AddReimbursement(ReimbursementViewModel _reimbursement)
        {
            int response = 0;
            UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
            try
            {
                if (String.IsNullOrWhiteSpace(_reimbursement.SignatureDataUrl))
                {
                    //return;
                }
                else
                {
                    var base64Signature = _reimbursement.SignatureDataUrl.Split(",")[1];
                    var binarySignature = Convert.FromBase64String(base64Signature);
                    System.IO.File.WriteAllBytes("Upload/Signature-" + Guid.NewGuid() + ".png", binarySignature);
                }

                //foreach (var file in _reimbursement.ExpenseDocument)
                //{

                //    //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                //    ////create folder if not exist
                //    //if (!Directory.Exists(path))
                //    //    Directory.CreateDirectory(path);
                //    //string fileNameWithPath = Path.Combine(path, file.FileName);
                //    //using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                //    //{
                //    //    file.CopyTo(stream);
                //    //}

                //    _uploadFileResponse = UploadFile(file, "Upload");
                //    if (_uploadFileResponse != null)
                //    {
                //        if (_uploadFileResponse.UploadSuccess == true)
                //        {
                //            _reimbursement.ExpenseDocumentName = _uploadFileResponse.FileName;
                //        }
                //    }

                //}

                //if (_reimbursement.ExpenseDocument != null)
                //{
                //    UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                //    _uploadFileResponse = UploadFile(_reimbursement.ExpenseDocument, "Upload");
                //    if (_uploadFileResponse != null)
                //    {
                //        if (_uploadFileResponse.UploadSuccess == true)
                //        {
                //            _reimbursement.ExpenseDocumentName = _uploadFileResponse.FileName;
                //        }
                //    }
                //}
                // byte[] bytes = Convert.FromBase64String(base64String.Split(',')[1]);

                _reimbursement.ClaimBy = GetUserSession().UserId;
                _reimbursement.CreatedBy = GetUserSession().UserId;
                _reimbursement.StatusId = 17;
                response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursement, 1);
                if (response > 0)
                {

                    //*********************************
                    if (_reimbursement.ExpenseDocument != null)
                        foreach (var file in _reimbursement.ExpenseDocument)
                        {
                            _uploadFileResponse = UploadFile(file, "Upload");
                            if (_uploadFileResponse != null)
                            {
                                if (_uploadFileResponse.UploadSuccess == true)
                                {
                                    ReimbursementViewModel _reimbursementDocument = new ReimbursementViewModel();
                                    _reimbursementDocument.Id = response;
                                    _reimbursementDocument.CreatedBy = GetUserSession().UserId;
                                    _reimbursementDocument.ExpenseDocumentName = _uploadFileResponse.FileName;
                                    response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursementDocument, 5);
                                }
                            }
                        }

                    //*******************************

                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Reimbursement));
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
            return View(_reimbursement);

        }



        [HttpPost]
        public IActionResult UpdateReimbursementStatus(WrapperReimbursementViewModel _reimbursement)
        {
            int response = 0;
            try
            {
                ReimbursementViewModel reimbursementViewModel = new ReimbursementViewModel();
                reimbursementViewModel.Id = _reimbursement.Id;
                reimbursementViewModel.StatusId = _reimbursement.StatusId;
                reimbursementViewModel.Comments = _reimbursement.Comments;
                reimbursementViewModel.ApprovedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateReimbursementData(reimbursementViewModel, 4);
                if (response > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Reimbursement));
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
            return View(_reimbursement);

        }

        public ActionResult _documentListPartial(int Id)
        {
            List<DocumentListViewModel> _documentList = new List<DocumentListViewModel>();
            _documentList = _dataAccessLayer.GetReimbursementDocumentData(6, Id).ToList();
            return PartialView("_documentListPartial", _documentList);
        }

        public IActionResult DownloadFile(string filename)
        {

            // Since this is just and example, I am using a local file located inside wwwroot
            //Afterwards file is converted into a stream
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload", filename);
            //var path = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.xlsx");
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", filename);
            // call GetFile Method in service and return

            // var dd= FileDownload(filename, "Upload");

        }

        public IActionResult DeleteFile(string filename, int id, int ReimId)
        {


            int response = 0;

            if (filename != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload", filename);
                System.IO.File.Delete(path);


                ReimbursementViewModel _reimbursementDocument = new ReimbursementViewModel();
                _reimbursementDocument.Id = id;
                _reimbursementDocument.CreatedBy = GetUserSession().UserId;
                _reimbursementDocument.ExpenseDocumentName = filename;
                response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursementDocument, 7);

                if (response > 0)
                {
                    TempData["successmessage"] = "Your file has been deleted successfully";
                    //return RedirectToAction(nameof(EditReimbursement), ReimId);
                    //return RedirectToAction("EditReimbursement", { Id = ReimId});
                    return RedirectToAction("EditReimbursement", new { Id = ReimId });
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

            }

            return RedirectToAction("EditReimbursement", new { Id = ReimId });

        }

        public IActionResult EditReimbursement(int Id)
        {
            int response = 0;
            ReimbursementViewModel _reimbursement = new ReimbursementViewModel();
            try
            {

                _reimbursement = _dataAccessLayer.GetReimbursementData(3, Id, 0, 0).FirstOrDefault();

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return View(_reimbursement);

        }

        [HttpPost]
        public IActionResult EditReimbursement(ReimbursementViewModel _reimbursement)
        {
            int response = 0;
            UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
            try
            {
                if (String.IsNullOrWhiteSpace(_reimbursement.SignatureDataUrl))
                {
                    //return;
                }
                else
                {
                    var base64Signature = _reimbursement.SignatureDataUrl.Split(",")[1];
                    var binarySignature = Convert.FromBase64String(base64Signature);
                    System.IO.File.WriteAllBytes("Upload_Signature-" + Guid.NewGuid() + ".png", binarySignature);
                }

                _reimbursement.ClaimBy = GetUserSession().UserId;

                _reimbursement.ExpenseDate = Convert.ToString(_reimbursement.ExpenseDate1);
                _reimbursement.ExpenseToDate = Convert.ToString(_reimbursement.ExpenseToDate1);
                //_reimbursement.StatusId = 17;
                response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursement, 2);
                if (response > 0)
                {

                    //*********************************
                    if (_reimbursement.ExpenseDocument != null)
                        foreach (var file in _reimbursement.ExpenseDocument)
                        {
                            _uploadFileResponse = UploadFile(file, "Upload");
                            if (_uploadFileResponse != null)
                            {
                                if (_uploadFileResponse.UploadSuccess == true)
                                {
                                    ReimbursementViewModel _reimbursementDocument = new ReimbursementViewModel();
                                    _reimbursementDocument.Id = response;
                                    _reimbursementDocument.CreatedBy = GetUserSession().UserId;
                                    _reimbursementDocument.ExpenseDocumentName = _uploadFileResponse.FileName;
                                    response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursementDocument, 5);
                                }
                            }
                        }

                    //*******************************

                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(Reimbursement));
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
            return View(_reimbursement);

        }

        public IActionResult DeleteReimbursement(int Id)
        {
            int response = 0;
            ReimbursementViewModel _reimbursement = new ReimbursementViewModel();
            try
            {
                _reimbursement.Id = Id;
                _reimbursement.CreatedBy = GetUserSession().UserId;
                response = _dataAccessLayer.AddUpdateReimbursementData(_reimbursement, 8);
                return RedirectToAction(nameof(Reimbursement));
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;
            }
            return RedirectToAction("Reimbursement", Id);

        }



    }
}
