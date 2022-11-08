using Microsoft.EntityFrameworkCore;
namespace Selenium.Database
{
    public class AccountContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }

        public AccountContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Accounts.db");
        }
    }
}
