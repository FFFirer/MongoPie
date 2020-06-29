using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MongoPie.Services;

namespace MongoPie.Extensions
{
    public class GlobalIOC
    {
        private ServiceCollection services { get; set; }
        public ServiceProvider serviceProvider { get; set; }
        private GlobalIOC()
        {
            services = new ServiceCollection();
            AddServices();
            serviceProvider = services.BuildServiceProvider();
        }

        public static GlobalIOC Instance { get; } = new GlobalIOC();

        public void AddServices()
        {
            services.AddScoped<IMongoConnectionManager, LocalMongoConnectionManager>();
        }
    }
}
