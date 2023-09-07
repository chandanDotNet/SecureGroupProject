using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class QuatationToPaymentDetails
    {
        [Key]
        public int QuatationToPaymentId { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public string QuatationFileName { get; set; }
        public int POId { get; set; }
        public string PIFileName { get; set; }
        public int PaymentId { get; set; }
        public Boolean IsQuatationComplete { get; set; }
        public Boolean IsPOComplete { get; set; }
        public Boolean IsPIComplete { get; set; }
        public Boolean IsPaymentComplete { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }

    }
}
