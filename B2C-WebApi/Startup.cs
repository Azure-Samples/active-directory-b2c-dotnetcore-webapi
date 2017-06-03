using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace B2CWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var jbo = new JwtBearerOptions
            {
                Authority = string.Format(Configuration["Authentication:AzureAd:AADInstance"], Configuration["Authentication:AzureAd:Tenant"], Configuration["Authentication:AzureAd:Policy"]),
                Audience = Configuration["Authentication:AzureAd:ClientId"]
            };

            jbo.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = authFailed
            };
            app.UseJwtBearerAuthentication(jbo);
            /*
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = string.Format(Configuration["Authentication:AzureAd:AADInstance"], Configuration["Authentication:AzureAd:Tenant"], Configuration["Authentication:AzureAd:Policy"]),
                Audience = Configuration["Authentication:AzureAd:ClientId"]                
            });
            */

            app.UseMvc();
        }

        private Task authFailed(AuthenticationFailedContext arg)
        {
            throw new NotImplementedException();
        }
    }
}
