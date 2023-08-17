using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class Reimbursement
    {
        [Key]
        public int Id { get; set; }
        public string ExpenseFor { get; set; }
        public string TotalExpenseAmount { get; set; }
        public string ClaimAmount { get; set; }
        public string ApprovedAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseDocument { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int StatusId { get; set; }
        public int ClaimBy { get; set; }
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }

    }
}
