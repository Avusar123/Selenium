using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
