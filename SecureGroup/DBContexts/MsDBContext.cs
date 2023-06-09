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
        public DbSet<UserViewModel> User { get; set; }
        public DbSet<WareHouse> WareHouse { get; set; }
        public DbSet<WareHouseAdminDetails> WareHouseAdminDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster");
            modelBuilder.Entity<CityMaster>().ToTable("CityMaster");
            modelBuilder.Entity<StateMaster>().ToTable("StateMaster");
            modelBuilder.Entity<UserViewModel>().ToTable("User");
            modelBuilder.Entity<WareHouse>().ToTable("WareHouse");
            modelBuilder.Entity<WareHouseAdminDetails>().ToTable("WareHouseAdminDetails");
        }

        
    }
}
