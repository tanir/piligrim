using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
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

            services.AddMvc();

            services.AddProductsDbContext(this.Configuration.GetConnectionString("products-db"));

            services.AddScoped<IProductsRepository, ProductsRepository>();

            services.AddScoped<IOrdersRepository, OrdersRepository>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ICategoriesProvider, StaticCategoriesProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggingConfig = this.Configuration.GetSection("Logging");
            loggerFactory.AddConsole().AddFile(loggingConfig);

            if (env.IsDevelopment() || this.Configuration.GetValue<bool>("displayErrors"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                LoginPath = new PathString("/Login/"),
                AccessDeniedPath = new PathString("/Login/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvcWithDefaultRoute();

            //DbInitializer.Initialize(app.ApplicationServices.GetService<StoreDbContext>());
        }
    }
}
