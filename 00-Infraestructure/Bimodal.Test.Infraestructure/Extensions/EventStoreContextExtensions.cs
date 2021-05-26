using Kledex.Extensions;
using Kledex.Store.EF;
using Kledex.Store.EF.Configuration;
using Kledex.Store.EF.Extensions;
using Kledex.Store.EF.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Bimodal.Test.Infraestructure.Extensions
{
    public static class EventStoreContextExtensions
    {
        public static IKledexServiceBuilder AddDomainSqliteStore(this IKledexServiceBuilder builder)
        {
            return AddEventSqliteStore(builder, opt => 
            {
                
            });
        }

        public static IKledexServiceBuilder AddEventSqliteStore(this IKledexServiceBuilder builder, Action<DatabaseOptions> configureOptions)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            builder.Services.Configure(configureOptions);

            var sp = builder.Services.BuildServiceProvider();
            var dbOptions = sp.GetService<IOptions<DatabaseOptions>>().Value;

            builder.AddEFStore();

            builder.Services.AddDbContext<DomainDbContext>(options =>
            {
                options.UseSqlite(dbOptions.ConnectionString);
            });

            builder.Services.AddTransient<IDatabaseProvider, SqliteDatabaseProvider>();

            return builder;
        }

        #region private methods
        private static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddFilter(DbLoggerCategory.Database.Command.Name,
                                     Microsoft.Extensions.Logging.LogLevel.Trace));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
        #endregion
    }
}
