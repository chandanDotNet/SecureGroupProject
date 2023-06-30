using System;
using System.ComponentModel.DataAnnotations;

namespace SecureGroup.Models
{
    public class WHStockTransferManage
    {

        [Key]
        public int Id { get; set; }
        public int TransferType { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public int ProductId { get; set; }
        public int SubProductId { get; set; }
        public decimal TransferQuantity { get; set; }
        public DateTime TransferDate { get; set; }
        public int TransferBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public Boolean IsActive { get; set; }
    }
}
