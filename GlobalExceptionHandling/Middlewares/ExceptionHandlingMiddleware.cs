using GlobalExceptionHandling.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlobalExceptionHandling.Middlewares
{
    /// <summary>
    /// Мидлварь глобальной обработки ошибок
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Метод, который проверяет, возникает ли ошибка при запросе
        /// </summary>
        /// <param name="httpContext">Контекст запроса</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(
                    httpContext,
                    ex.Message,
                    ex.StackTrace,
                    ex.Data);
            }
        }

        /// <summary>
        /// Хендлер ошибок
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <param name="exMsg">Сообщение ошибки</param>
        /// <param name="stackTrace">StackTrace ошибки</param>
        /// <param name="exData">Дополнительные данные об ошибке</param>
        private async Task HandleExceptionAsync(
            HttpContext context,
            string exMsg,
            string stackTrace,
            IDictionary exData)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = context.Response.StatusCode;

            if(exData.Count > 0)
            {
                List<string?> exDataKeys = new List<string?>();
                List<string?> exDataValues = new List<string?>();

                foreach (DictionaryEntry de in exData)
                {
                    exDataKeys.Add(de.Key.ToString());
                    exDataValues.Add(de.Value.ToString());
                }

                ErrorDTO errorDTO = new ErrorDTO()
                {
                    Message = exMsg,
                    StatusCode = context.Response.StatusCode,
                    StackTrace = stackTrace,
                    ExDataKeys = exDataKeys,
                    ExDataValues = exDataValues
                };

                string result = JsonSerializer.Serialize(errorDTO);

                await response.WriteAsync(result);
            }
            else
            {
                ErrorDTO errorDTO = new ErrorDTO()
                {
                    Message = exMsg,
                    StatusCode = context.Response.StatusCode,
                    StackTrace = stackTrace
                };

                string result = JsonSerializer.Serialize(errorDTO);

                _logger.LogError(result);
                await response.WriteAsync(result);
            }
        }
    }
}
