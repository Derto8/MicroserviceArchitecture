using DBContext.DTOs;
using DBContext.Enums;
using DBContext.Interfaces;
using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.RepositoryServices
{
    public class BuyDrinkRepository : IBuyDrinkRepository
    {
        private ApplicationContext _context;
        private ILogger<BuyDrinkRepository> _logger;

        public BuyDrinkRepository(ApplicationContext context,
            ILogger<BuyDrinkRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Метод покупки напитка,
        /// изменяет количество монет в бд и отдаёт сдачу (если цена за напиток не превышает баланс пользоватля),
        /// удаляет 1 шт. напитка из бд
        /// </summary>
        /// <param name="userCoins">DTO данных пользователя</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Task</returns>
        public async Task BuyDrinkAsync(UserBuyDrink userCoins, CancellationToken cancellationToken)
        {
            Drinks drink = await GetDrinkAsync(userCoins.IdDrink, cancellationToken);
            if(drink.Price <= userCoins.Balance)
            {
                //получаем массив чисел из CoinDenominationsEnum
                int[] valuesEnum = (int[])Enum.GetValues(typeof(CoinDenominationsEnum));

                await Algorithm(valuesEnum.OrderDescending().ToArray(), userCoins.Balance, drink.Price, cancellationToken);

                await ChangeAmountDrink(drink, cancellationToken);
            }
        }

        /// <summary>
        /// Алгоритм перебирает числа из enum, и делит разницу между ценой напитка и
        /// балансом на число из CoinDenominationsEnum во время перебора.
        /// Если результат деления не является целым, то результат округляем в меньшую сторону и 
        /// вызываем метод ChangeAmountCoin, после вычитаем из разницы округлённый результат деления 
        /// умноженный на число из конкретное число из enum
        /// Если же результат деления целый, то вызываем метод ChangeAmountCoin
        /// </summary>
        /// <param name="valuesEnum">массив всех enum чисел</param>
        /// <param name="balance">баланс юзера</param>
        /// <param name="price">цена напитка</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Task</returns>
        private async Task Algorithm(int[] valuesEnum, int balance, int price, CancellationToken cancellationToken)
        {
            //разница между ценой и балансом
            var priceDrink = Convert.ToDouble(price);
            for(int i = 0; i < valuesEnum.Length; i++)
            {
                //находим количество монет
                double amount = priceDrink / valuesEnum[i];
                Coins coin = await GetCoinByDenomination(valuesEnum[i], cancellationToken);

                //если делится без остатка
                if (priceDrink % valuesEnum[i] == 0)
                {
                    amount = priceDrink / valuesEnum[i];
                    await ChangeAmountCoin(coin, amount, cancellationToken);
                    return;
                }
                else
                {
                    //если есть остаток то округляем в меньшую стороно до целочиленного
                    amount = Math.Floor(amount);
                    await ChangeAmountCoin(coin, amount, cancellationToken);
                    //из разницы вычитаем число на которое делили, умноженное на количество делений
                    priceDrink -= valuesEnum[i] * amount;
                }
            }
        }

        /// <summary>
        /// Получаем монету по его свойству Denomination
        /// </summary>
        /// <param name="denomination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Task<Coins></returns>
        private async Task<Coins> GetCoinByDenomination(int denomination, CancellationToken cancellationToken)
        {
            return await _context.CoinsTable.Where(c => (int)c.Denomination == denomination).FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<Drinks> GetDrinkAsync(Guid idDrink, CancellationToken cancellationToken)
        {
            return await _context.DrinksTable.Where(c => c.Id.Equals(idDrink)).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Метод изменения в бд проперти Amount (количество) в большую сторону
        /// у конкретной монеты
        /// </summary>
        /// <param name="coin">Модель монеты</param>
        /// <param name="amount">На сколько нужно изменить количество</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Task</returns>
        private async Task ChangeAmountCoin(Coins coin, double amount, CancellationToken cancellationToken)
        {
            _context.Entry(coin).State = EntityState.Modified;
            coin.Amount += (int)amount;

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// При покупке напитка, изменяем количество в бд
        /// на единицу меньше.
        /// </summary>
        /// <param name="drink">Модель напитка</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Task</returns>
        private async Task ChangeAmountDrink(Drinks drink, CancellationToken cancellationToken)
        {
            _context.Entry(drink).State = EntityState.Modified;
            drink.Amount -= 1;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
