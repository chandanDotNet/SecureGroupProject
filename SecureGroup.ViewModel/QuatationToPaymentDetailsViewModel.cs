using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class QuatationToPaymentDetailsViewModel
    {

        public int QuatationToPaymentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string QuatationFileName { get; set; }
        public IFormFile QuatationFile { get; set; }
        public int POId { get; set; }

        public decimal QuatationAmount { get; set; }
        public decimal PIAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal FinalAmount { get; set; }
       
        public string PIFileName { get; set; }
        public IFormFile PIFile { get; set; }
        public int PaymentId { get; set; }
        public string VFinelBillFileName { get; set; }
        public IFormFile VFinelBillFile { get; set; }
        public Boolean IsQuatationComplete { get; set; }
        public Boolean IsPOComplete { get; set; }
        public Boolean IsPIComplete { get; set; }
        public Boolean IsPaymentComplete { get; set; }
        public Boolean IsVFinelBillComplete { get; set; }
        public int CreatedBy { get; set; }
        public IList<SelectListItem> VendorList { get; set; }


    }

    public class RaperQuatationToPaymentDetailsViewModel
    {
        public int UserId { get; set; }
        public string QuatationFileName { get; set; }
        public IFormFile QuatationFile { get; set; }
        public string PIFileName { get; set; }
        public IFormFile PIFile { get; set; }

        public List<QuatationToPaymentDetailsViewModel> quatationToPaymentDetailsViewModels { get; set; }

    }
}
