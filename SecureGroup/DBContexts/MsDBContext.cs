using Microsoft.EntityFrameworkCore;
using SecureGroup.Models;
using SecureGroup.ViewModel;

namespace SecureGroup.DBContexts
{
    public class MsDBContext : DbContext
    {

        public MsDBContext(DbContextOptions<MsDBContext> options) : base(options)
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
        public DbSet<LogManagement> LogManagement { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Scheme> Scheme { get; set; }
        public DbSet<QuatationToPaymentDetails> QuatationToPaymentDetails { get; set; }
        public DbSet<OfficeAddress> OfficeAddress { get; set; }
        public DbSet<FailedLoginAttempts> FailedLoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster");
            modelBuilder.Entity<CityMaster>().ToTable("CityMaster");
            modelBuilder.Entity<StateMaster>().ToTable("StateMaster");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<WareHouse>().ToTable("WareHouse");
            modelBuilder.Entity<WareHouseAdminDetails>().ToTable("WareHouseAdminDetails");
            modelBuilder.Entity<ProductMaster>().ToTable("ProductMaster");
            modelBuilder.Entity<SubProductMaster>().ToTable("SubProductMaster");
            modelBuilder.Entity<UnitMaster>().ToTable("UnitMaster");
            modelBuilder.Entity<SupplierDetails>().ToTable("SupplierDetails");
            modelBuilder.Entity<WHStockProduct>().ToTable("WHStockProduct");
            modelBuilder.Entity<WHStockTransferManage>().ToTable("WHStockTransferManage");
            modelBuilder.Entity<TransferTypeMaster>().ToTable("TransferTypeMaster");
            modelBuilder.Entity<Site>().ToTable("Site");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<SysVal>().ToTable("SysVal");
            modelBuilder.Entity<Task>().ToTable("Task");
            modelBuilder.Entity<TaskUpdate>().ToTable("TaskUpdate");
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
            modelBuilder.Entity<LogManagement>().ToTable("LogManagement");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Scheme>().ToTable("Scheme");
            modelBuilder.Entity<QuatationToPaymentDetails>().ToTable("QuatationToPaymentDetails");
            modelBuilder.Entity<OfficeAddress>().ToTable("OfficeLocation");
            modelBuilder.Entity<FailedLoginAttempts>().ToTable("FailedLoginAttempts");
        }

        
    }
}
