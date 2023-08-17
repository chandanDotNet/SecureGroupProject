using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class LeaveManagement
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal BalanceLeave { get; set; }
        public decimal TakenLeave { get; set; }
        public Boolean IsActive { get; set; }
    }
}
