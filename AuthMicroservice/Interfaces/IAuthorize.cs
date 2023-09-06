using System.Net;

namespace AuthMicroservice.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий методы авторизации и регистрации юзера
    /// </summary>
    public interface IAuthorize
    {
        /// <summary>
        /// Метод авторизации юзера
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="cancellationToken">Токен отмены задачи</param>
        /// <returns>JSON-файл с jwt и id записи в бд пользователя</returns>
        Task<IResult> AuthorizationMethod(string login, string password, CancellationToken cancellationToken);
        /// <summary>
        /// Метод регистрации юзера
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="cancellationToken">токен отмены задачи</param>
        Task RegistrationMethod(string login, string password, CancellationToken cancellationToken);
    }
}
