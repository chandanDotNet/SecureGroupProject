using Microsoft.EntityFrameworkCore;

using SecureGroup.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Site = System.Security.Policy.Site;
using Task = SecureGroup.ViewModel.Models.Task;

namespace SecureGroup.Concrete
{
    public class MsSqlDBContext : DbContext
    {

        public MsSqlDBContext(DbContextOptions<MsSqlDBContext> options) : base(options)
        {
        }

        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<CityMaster> CityMaster { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<WareHouse> WareHouse { get; set; }
        public DbSet<WareHouseAdminDetails> WareHouseAdminDetails { get; set; }
        public DbSet<ProductMaster> ProductMaster { get; set; }
        public DbSet<SubProductMaster> SubProductMaster { get; set; }
        public DbSet<UnitMaster> UnitMaster { get; set; }
        public DbSet<SupplierDetails> SupplierDetails { get; set; }
        public DbSet<WHStockProduct> WHStockProduct { get; set; }
        public DbSet<WHStockTransferManage> WHStockTransferManage { get; set; }
        public DbSet<TransferTypeMaster> TransferTypeMaster { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<SysVal> SysVal { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<TaskUpdate> TaskUpdate { get; set; }
        public DbSet<Attendance> Attendance { get; set; }

       
    }
}
