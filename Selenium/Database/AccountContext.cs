using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Selenium.Database
{
    public class AccountContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }

        private IConfiguration Configuration { get; set; }

        public AccountContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Postgres"));
        }
    }
}
