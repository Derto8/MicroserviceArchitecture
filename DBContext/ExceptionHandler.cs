using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    static class ExceptionHandler
    {
        internal static string CatchEx(Exception ex)
        {
            if(ex.Data.Count > 0)
            {
                string exception = $"Исключение: {ex.Message}\n" +
                    $"Метод: {ex.TargetSite}\n" +
                    $"Трассировка стека: {ex.StackTrace}\n" +
                    $"{DateTime.UtcNow.ToLongDateString()}\n" +
                    $"Детали:";

                foreach (DictionaryEntry de in ex.Data)
                    exception += $"\nKey: {de.Key}, Value: {de.Value}";

                return exception;
            }
            else
            {
                string exception = $"Исключение: {ex.Message}\n" +
                    $"Метод: {ex.TargetSite}\n" +
                    $"Трассировка стека: {ex.StackTrace}\n" +
                    $"{DateTime.UtcNow.ToLongDateString()}";
                return exception;
            }
        }
    }
}
