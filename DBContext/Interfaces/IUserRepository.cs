using DBContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий методы для взаимодействия с таблицей Users
    /// </summary>
    public interface IUserRepository: IBaseRepository<Users>
    {
        /// <summary>
        /// Метод регистрации юзера
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        Task RegistrationAsync(string login, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Метод авторизации юзера
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>Модель данных юзера</returns>
        Task<Users> AuthorizationAsync(string login, string pass, CancellationToken cancellationToken);
    }
}
