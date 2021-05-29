using AutoMapper;
using Bimodal.Test.Api.Core.Automapper;
using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Database;
using Hellang.Middleware.ProblemDetails;
using Kledex.Exceptions;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Kledex.Store.EF.Sqlite.Extensions;
using Kledex.UI.Extensions;
using Kledex.Validation.FluentValidation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

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
            services.AddProblemDetails(options => 
            {
                options.Map<ValidationException>(ex =>
                {
                    return ex.ToValidationProblemDetails();
                });
                options.Map<Exception>(ex => {
                    return ex.ToProblemDetails();
                });
            });
            services.AddDbContext<AgencyContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DomainConnection"));
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ctx =>
                {
                    var problems = new ValidationProblemDetails(ctx.ModelState)
                    {
                        Instance = ctx.HttpContext.Request.Path
                    };
                    return new UnprocessableEntityObjectResult(problems);
                };
            });
            services
                .AddKledex(options => {
                    options.PublishEvents = true;
                    options.SaveCommandData = true;
                }, typeof(Customer), typeof(Booking))
                .AddSqliteStore(options => {
                    options.ConnectionString = Configuration.GetConnectionString("EventsConnection");
                })
                .AddFluentValidation(options =>
                {
                    options.ValidateAllCommands = false;
                })
                .AddUI();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Bimodal API V1", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
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
                c.SwaggerEndpoint("v1/swagger.json", "Bimodal API V1");
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