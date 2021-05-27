using Bimodal.Test.Database;
using Hellang.Middleware.ProblemDetails;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Kledex.Store.EF.Sqlite.Extensions;
using Kledex.UI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bimodal.Test.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddProblemDetails();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Bimodal test API V1", Version = "v1" });
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ctx => 
                {
                    var problems = new ValidationProblemDetails(ctx.ModelState);
                    return new UnprocessableEntityObjectResult(problems);
                };
            });
            services.AddDbContext<AgencyContext>(options => 
            {
                options.UseSqlite(Configuration.GetConnectionString("DomainConnection"));
            });
            services
                .AddKledex(options => {
                    options.PublishEvents = true;
                    options.SaveCommandData = true;
                }, typeof(Customer), typeof(Booking))
                .AddSqliteStore(options => {
                    options.ConnectionString = Configuration.GetConnectionString("EventsConnection");
                })
                .AddUI();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseKledex().EnsureDomainDbCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("v1/swagger.json", "Bimodal test API V1");
            });
            app.UseProblemDetails();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
