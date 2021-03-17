using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieDatabase.Configuration;
using MovieDatabase.Mappers;
using MovieDatabase.Middlewares;
using SimpleInjector;

namespace MovieDatabase
{
    public class Startup
    {
        private Container container;
        public Startup(IConfiguration configuration)
        {
            container = new Container();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            container.Options.ResolveUnregisteredConcreteTypes = false;

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieDatabase", Version = "v1" });
            });

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()

                       // Ensure activation of a specific framework type to be created by
                       // Simple Injector instead of the built-in configuration system.
                       // All calls are optional. You can enable what you need. For instance,
                       // ViewComponents, PageModels, and TagHelpers are not needed when you
                       // build a Web API.
                       .AddControllerActivation();
            });

            SimpleInjectorInitialize.AddServices(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieDatabase v1"));
            }

            app.UseHttpsRedirection();

            app.AddExceptionHandler(container);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSimpleInjector(container);
        }
    }
}
