using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class WHStockProductDetailsViewModel
    {
        public int WareHouseId { get; set; }
        public string WareHouseName { get; set; }        
        public int ProductId { get; set; }
        public string ProductName { get; set; }      
        public int SubProductId { get; set; }
        public string SubProductName { get; set; }       
        public decimal ProductAddQty { get; set; }        
        public decimal StockIn { get; set; }        
        public decimal StockOut { get; set; }        
        public decimal TotalQty { get; set; }        
        public string TransferType { get; set; }
        
    }

    public class WHProductPurchaseInOutDetailsViewModel
    {
        public int Id { get; set; }
        public int WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SubProductId { get; set; }
        public string SubProductName { get; set; }
        public decimal ProductQty { get; set; }       
        public string TransferType { get; set; }
        public string TransferDetails { get; set; }
        public string PurchaseFrom { get; set; }
        public string Date { get; set; }

    }
}
