using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class SessionSettings
    {
        public static IServiceCollection AddSessionSettings(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = "Application.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.IsEssential = true;
            });
            return services;
        }
    }
}
