using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using MapPinAPI.Middleware;
using MapPinAPI.Models;
using MapPinAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IMapPinRepository, MapPinRepository>();
            //services.AddScoped<IFavouriteMapPinRepository, FavouriteMapPinRepository>();
            //services.AddScoped<IEvaluationRepository, EvaluationRepository>();

            services.AddControllers();

            services.AddDbContext<MapPinContext>(o => o.UseSqlite("Data source = MapPin.db"));
        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapPinAPI", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterType<UserRepository>().As<IUserRepository>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor))
                .InstancePerDependency();

            builder.RegisterType<MapPinRepository>().As<IMapPinRepository>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor))
                .InstancePerDependency();

            builder.RegisterType<FavouriteMapPinRepository>().As<IFavouriteMapPinRepository>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor))
                .InstancePerDependency();

            builder.RegisterType<EvaluationRepository>().As<IEvaluationRepository>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor))
                .InstancePerDependency();

            builder.Register(x => Log.Logger).SingleInstance();
            builder.RegisterType<LoggingInterceptor>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapPinAPI v1"));
            }

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseMiddleware<ErrorHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
