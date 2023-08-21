using DBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DBContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> UsersTable { get; set; }
        public DbSet<Coins> CoinsTable { get; set; }
        public DbSet<Drinks> DrinksTable { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}