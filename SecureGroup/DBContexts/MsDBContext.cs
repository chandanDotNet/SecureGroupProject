using Microsoft.EntityFrameworkCore;
using SecureGroup.ViewModel;

namespace SecureGroup.DBContexts
{
    public class MsDBContext : DbContext
    {

        public MsDBContext(DbContextOptions<MsDBContext> options) : base(options)
        {

        }

        public DbSet<UserViewModel> User { get; set; }

    }
}
