using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class TransferTypeMaster
    {
        [Key]
        public int Id { get; set; }
        public string TransferType { get; set; }
        public Boolean IsActive { get; set; }
       
    }
}
