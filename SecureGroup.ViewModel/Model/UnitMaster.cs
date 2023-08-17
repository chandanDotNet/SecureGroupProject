using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class UnitMaster
    {
        [Key]
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
