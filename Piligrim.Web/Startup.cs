﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Piligrim.Core;
using Piligrim.Core.Categories;
using Piligrim.Core.Data;
using Piligrim.Core.Mail;
using Piligrim.Data;
using Piligrim.Web.Configuration;
using Piligrim.Web.Extensions;
using Piligrim.Web.Infrastructure;

namespace Piligrim.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        private IHostingEnvironment environment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();

            this.environment = env;

            env.ConfigureNLog("nlog.config");
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<MailConfiguration>(configuration =>
            {
                configuration.From = this.Configuration.GetSection("MailConfiguration:From").Get<MailAddress>();
                configuration.Smtp = this.Configuration.GetSection("MailConfiguration:Smtp").Get<Smtp>();
            });

            services.Configure<AppSettings>(this.Configuration);
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Login/");
                options.AccessDeniedPath = new PathString("/Login/Forbidden/");
            });

            var keysPath = Path.Combine(this.environment.ContentRootPath, "keys");

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysPath));

            services.AddMvc();

            services.AddProductsDbContext(this.Configuration.GetConnectionString("products-db"));

            services.AddScoped<IProductsRepository, ProductsRepository>();

            services.AddScoped<IOrdersRepository, OrdersRepository>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICategoriesProvider, StaticCategoriesProvider>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            if (env.IsDevelopment() || this.Configuration.GetValue<bool>("displayErrors"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            //DbInitializer.Initialize(app.ApplicationServices.GetService<StoreDbContext>());
        }
    }
}
