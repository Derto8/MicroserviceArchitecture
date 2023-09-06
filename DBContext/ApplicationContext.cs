using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DBContext
{
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Проперти таблицы Users
        /// </summary>
        public DbSet<Users> UsersTable { get; set; }
        /// <summary>
        /// Проперти таблицы Coins
        /// </summary>
        public DbSet<Coins> CoinsTable { get; set; }
        /// <summary>
        /// Проперти таблицы Drinks
        /// </summary>
        public DbSet<Drinks> DrinksTable { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// При создании бд добавление тестовых данных в таблицы
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(
                new Users[]
                {
                    new Users
                    {
                        Id = Guid.NewGuid(),
                        Login = "adm",
                        Password = "adm",
                        Role = Enums.RoleEnum.Admin
                    },
                    new Users
                    {
                        Id = Guid.NewGuid(),
                        Login = "user",
                        Password = "user",
                        Role = Enums.RoleEnum.User
                    },
                }
            );

            modelBuilder.Entity<Coins>().HasData(
                new Coins[]
                {
                    new Coins()
                    {
                        Id = Guid.NewGuid(),
                        Denomination = Enums.CoinDenominationsEnum.one,
                        Amount = 6,
                        IsBlocked = false,
                    },

                    new Coins()
                    {
                        Id = Guid.NewGuid(),
                        Denomination = Enums.CoinDenominationsEnum.two,
                        Amount = 20,
                        IsBlocked = true,
                    },

                    new Coins()
                    {
                        Id = Guid.NewGuid(),
                        Denomination = Enums.CoinDenominationsEnum.five,
                        Amount = 4,
                        IsBlocked = false,
                    },

                    new Coins()
                    {
                        Id = Guid.NewGuid(),
                        Denomination = Enums.CoinDenominationsEnum.ten,
                        Amount = 9,
                        IsBlocked = false,
                    }
                }
            );

            modelBuilder.Entity<Drinks>().HasData(
                new Drinks[]
                {
                    new Drinks()
                    {
                        Id = Guid.NewGuid(),
                        Name = "cola",
                        Price = 30.99m,
                        Amount = 20,
                        Img = ""
                    },

                    new Drinks()
                    {
                        Id = Guid.NewGuid(),
                        Name = "sprite",
                        Price = 50,
                        Amount = 10,
                        Img = ""
                    },

                    new Drinks()
                    {
                        Id = Guid.NewGuid(),
                        Name = "dr. pepper",
                        Price = 80,
                        Amount = 30,
                        Img = ""
                    }
                }
            );
        }
    }
}