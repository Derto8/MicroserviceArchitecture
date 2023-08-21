using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public static class AddObjectsToDB
    {
        public static async Task Initial(ApplicationContext context)
        {
            Users admin = new Users
            {
                Id = Guid.NewGuid(),
                Login = "adm",
                Password = "adm",
                Role = Enums.RoleEnum.Admin
            };

            Users user = new Users
            {
                Id = Guid.NewGuid(),
                Login = "user",
                Password = "user",
                Role = Enums.RoleEnum.User
            };

            Coins coin1 = new Coins()
            {
                Id = Guid.NewGuid(),
                Denomination = Enums.CoinDenominationsEnum.one,
                Amount = 6,
                IsBlocked = false,
            };

            Coins coin2 = new Coins()
            {
                Id = Guid.NewGuid(),
                Denomination = Enums.CoinDenominationsEnum.two,
                Amount = 20,
                IsBlocked = true,
            };

            Coins coin3 = new Coins()
            {
                Id = Guid.NewGuid(),
                Denomination = Enums.CoinDenominationsEnum.five,
                Amount = 4,
                IsBlocked = false,
            };

            Coins coin4 = new Coins()
            {
                Id = Guid.NewGuid(),
                Denomination = Enums.CoinDenominationsEnum.ten,
                Amount = 9,
                IsBlocked = false,
            };

            Drinks drink1 = new Drinks()
            {
                Id = Guid.NewGuid(),
                Name = "cola",
                Price = 30.99m,
                Amount = 20,
                Img = ""
            };

            Drinks drink2 = new Drinks()
            {
                Id = Guid.NewGuid(),
                Name = "sprite",
                Price = 50,
                Amount = 10,
                Img = ""
            };

            Drinks drink3 = new Drinks()
            {
                Id = Guid.NewGuid(),
                Name = "dr. pepper",
                Price = 80,
                Amount = 30,
                Img = ""
            };

            if (!context.UsersTable.Any()) await context.AddRangeAsync(admin, user);
            if(!context.CoinsTable.Any()) await context.AddRangeAsync(coin1, coin2, coin3, coin4);
            if (!context.DrinksTable.Any()) await context.AddRangeAsync(drink1, drink2, drink3);

            await context.SaveChangesAsync();
        }
    }
}
