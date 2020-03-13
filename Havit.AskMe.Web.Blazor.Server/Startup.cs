using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Havit.AskMe.Web.Blazor.Server {
	public class Startup {
		private readonly IWebHostEnvironment _environment;
		private readonly IConfigurationRoot _config;

		public Startup(IWebHostEnvironment env) {
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
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddResponseCompression(opts => {
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
			var appConfiguration = _config.Get<AppConfiguration>();

			// Configure identity and authentication
			services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
				options.Password.RequiredLength = 12;
				options.Password.RequiredUniqueChars = 4;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Name;
			})
				.AddEntityFrameworkStores<AskDbContext>()
				.AddDefaultTokenProviders();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options => {
					options.TokenValidationParameters = new TokenValidationParameters() {
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = appConfiguration.JwtIssuer,
						ValidAudience = appConfiguration.JwtAudience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JwtSecurityKey))
					};
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AskDbContext context, UserManager<ApplicationUser> userManager) {
			// Migrate database to last version
			context.Database.Migrate();

			context.Seed(); // DEMO PURPOSES, REMOVE AS NEEDED

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

			app.UseResponseCompression();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => {
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
