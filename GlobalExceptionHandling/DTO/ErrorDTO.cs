using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalExceptionHandling.DTO
{
    /// <summary>
    /// DTO ошибки
    /// </summary>
    public class ErrorDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public List<string?> ExDataKeys { get; set; }
        public List<string?> ExDataValues { get; set; }
    }
}
