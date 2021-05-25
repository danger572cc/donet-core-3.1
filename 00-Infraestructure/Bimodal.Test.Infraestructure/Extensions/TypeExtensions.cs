using Kledex;
using Kledex.Configuration;
using Kledex.Domain;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Linq;

namespace Bimodal.Test.Infraestructure.Extensions
{
    public static class TypeExtensions
    {
        public static IKledexServiceBuilder AddCustomKledex(this IServiceCollection services, Action<MainOptions> setupAction, params Type[] types)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var typeList = types.ToList();
            typeList.Add(typeof(IDispatcher));

            services.Scan(s => s
                .FromAssembliesOf(typeList)
                    .AddClasses()
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddAutoMapper(typeList);

            services.Configure(setupAction);

            return new KledexServiceBuilder(services);
        }
    }
}
