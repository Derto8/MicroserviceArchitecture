using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.AuthDTOs
{
    /// <summary>
    /// DTO, создал для того, чтобы передавать данные
    /// между микросервисом AuthMicroservic и сервисом IntraVisionTestTask
    /// </summary>
    public class JWT
    {
        public string access_token { get; set; }
        public Guid userId { get; set; }
    }
}
