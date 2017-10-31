using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.AskMe.Web {
    public class Startup {
        private readonly IHostingEnvironment _environment;
        private readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env) {
            this._environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this._config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<AskDbContext>(options => {
                options.UseSqlite(_config.GetConnectionString("AskDB"));
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AskDbContext context) {
            // Show detailed errors in development environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Migrate database to last version
            context.Database.Migrate();

            // Seed if in development environment
            if(env.IsDevelopment()) {
                context.Seed();
            }

        }

    }
}
