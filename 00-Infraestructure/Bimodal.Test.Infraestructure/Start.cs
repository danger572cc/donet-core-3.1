using Bimodal.Test.Infraestructure.Database;
using Bimodal.Test.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bimodal.Test.Infraestructure
{
    [OnStartUp(Module = "Infrastructure", Order = int.MinValue)]
    public class Start
    {
        public Start(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AgencyContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DomainConnection"));
            });

            services
                .AddCustomKledex(options => {
                    options.PublishEvents = true;
                    options.SaveCommandData = true;
                }, typeof(Customer), typeof(Booking))
                .AddEventSqliteStore(options =>
                {
                   options.ConnectionString = configuration.GetConnectionString("EventsConnection");
                });
        }
    }
}
