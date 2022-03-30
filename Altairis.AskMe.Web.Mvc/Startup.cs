﻿using System;
using System.Linq;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Altairis.AskMe.Web.Mvc {
    public class Startup {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            // Configure DB context
            services.AddDbContext<AskDbContext>(options => {
                options.UseSqlite(this.configuration.GetConnectionString("AskDB"));
            });

            // Configure MVC
            services.AddRouting();
            services.AddControllersWithViews(options => {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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
            });

            // Load configuration
            services.Configure<AppSettings>(this.configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AskDbContext context, UserManager<ApplicationUser> userManager) {
            // Show detailed errors in development environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseBrowserLink();
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
            app.UseRouting();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseAuthentication();
            app.UseAuthorization();

            // Map endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

    }
}
