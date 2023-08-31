
using System.Net.NetworkInformation;
using System.Text.Json;

namespace IntraVisionTestTask.Extensions
{
    /// <summary>
    /// расширения для сессии
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Добавление новой записи в сесиию произвольного
        /// типа данных
        /// </summary>
        /// <typeparam name="T">Произвольный тип данных</typeparam>
        /// <param name="session">Хранилище данных юзера (сессия)</param>
        /// <param name="key">Ключ к хранилищу</param>
        /// <param name="value">параметр который засовываем в сессию</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }

        /// <summary>
        /// Получаем значение из сесии
        /// </summary>
        /// <typeparam name="T">Произвольный тип данных</typeparam>
        /// <param name="session">Хранилище данных юзера (сессия)</param>
        /// <param name="key">Ключ к хранилищу</param>
        /// <returns>Значение произвольного типа данных из сесии</returns>
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
