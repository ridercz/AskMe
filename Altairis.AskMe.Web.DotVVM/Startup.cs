using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.AskMe.Web.DotVVM {
    public class Startup {
        private readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env) {
            // Set CWD to content root (needed when AspNetCoreHostingModel=InProcess)
            Environment.CurrentDirectory = env.ContentRootPath;

            // Load configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this._config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            // Configure DB context
            services.AddDbContext<AskDbContext>(options => {
                options.UseSqlite(this._config.GetConnectionString("AskDB"));
            });

            // Configure identity and authentication
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AskDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Events.OnRedirectToLogin = context => DotvvmAuthenticationHelper.ApplyRedirectResponse(context.HttpContext, context.RedirectUri);
            });

            // Load configuration
            services.Configure<AppConfiguration>(this._config);
            services.AddDotVVM<DotvvmStartup>();

            // AutoMapper config
            MapperConfig.Configure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AskDbContext context, UserManager<ApplicationUser> userManager) {
            // Show detailed errors in development environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }

            // Migrate database to last version
            context.Database.Migrate();

            // Seed initial data if in development environment
            if (env.IsDevelopment()) {
                // Create categories
                context.Seed();

                // Create default user
                if (!userManager.Users.Any()) {
                    var adminUser = new ApplicationUser { UserName = "admin" };
                    var r = userManager.CreateAsync(adminUser, "pass.word123").Result;
                    if (r != IdentityResult.Success) {
                        var errors = string.Join(", ", r.Errors.Select(x => x.Description));
                        throw new Exception("Seeding default user failed: " + errors);
                    }
                }
            }

            // Enable static file caching for one year
            app.UseStaticFiles(new StaticFileOptions {
                OnPrepareResponse = ctx => {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                }
            });

            // Use other middleware
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseAuthentication();
            app.UseDotVVM<DotvvmStartup>();
        }
    }
}
