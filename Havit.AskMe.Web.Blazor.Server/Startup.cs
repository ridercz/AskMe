using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Havit.AskMe.Web.Blazor.Server
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfigurationRoot _config;

        public Startup(IWebHostEnvironment env)
        {
            this._environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this._config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            // CUSTOM SERVICES
            // 

            // Configure DB context
            services.AddDbContext<AskDbContext>(options => {
                options.UseSqlite(this._config.GetConnectionString("AskDB"));
            });

            // Load configuration
            services.Configure<AppConfiguration>(this._config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AskDbContext context)
        {
            // Migrate database to last version
            context.Database.Migrate();

			context.Seed(); // DEMO PURPOSES, REMOVE AS NEEDED

			// Seed initial data if in development environment
			if (env.IsDevelopment())
            {
                // Create categories
                context.Seed();

                //// Create default user
                //if (!userManager.Users.Any())
                //{
                //    var adminUser = new ApplicationUser { UserName = "admin" };
                //    var r = userManager.CreateAsync(adminUser, "pass.word123").Result;
                //    if (r != IdentityResult.Success)
                //    {
                //        var errors = string.Join(", ", r.Errors.Select(x => x.Description));
                //        throw new Exception("Seeding default user failed: " + errors);
                //    }
                //}
            }

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

			app.UseClientSideBlazorFiles<Client.Startup>();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
			});
		}
    }
}
