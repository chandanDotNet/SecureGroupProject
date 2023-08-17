using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class ReimbursementViewModel
    {


        public int Id { get; set; }
        public string ExpenseFor { get; set; }
        public string TotalExpenseAmount { get; set; }
        public string ClaimAmount { get; set; }
        public string ApprovedAmount { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseToDate { get; set; }
        public DateTime ExpenseDate1 { get; set; }
        public DateTime ExpenseToDate1 { get; set; }
        public List<IFormFile>? ExpenseDocument { get; set; }
        public string ExpenseDocumentName { get; set; }
        public string SignatureDataUrl { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string ApprovedDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ClaimBy { get; set; }
        public string ClaimByName { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
    }


    public class WrapperReimbursementViewModel
    {
        public int Id { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Status")]
        public int StatusId { get; set; }
        public string Comments { get; set; }
        public List<ReimbursementViewModel> ReimbursementList { get; set;}
        public IList<SelectListItem> StatusList { get; set; }
    }

    public class DocumentListViewModel
    {
        public int DocumentId { get; set; }
        public int ReimbursementId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public string UploadedDate { get; set; }
        
    }
}
