global using System.ComponentModel.DataAnnotations;
global using Altairis.AskMe.Data;
global using Altairis.AskMe.Web.RazorPages;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore;
using Altairis.AskMe.Data.Sqlite;
using Altairis.AskMe.Data.SqlServer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container *****************************************************************/

// Configure DB context (Sqlite or SQL server
if (AppSettings.DatabaseTypeSqlite.Equals(builder.Configuration["DatabaseType"], StringComparison.OrdinalIgnoreCase)) {
    builder.Services.AddDbContext<AskDbContext, AskDbContextSqlite>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AskDB_Sqlite")));
} else if (AppSettings.DatabaseTypeSqlServer.Equals(builder.Configuration["DatabaseType"], StringComparison.OrdinalIgnoreCase)) {
    builder.Services.AddDbContext<AskDbContext, AskDbContextSqlServer>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AskDB_SqlServer")));
} else {
    throw new NotSupportedException($"Specified database type '{builder.Configuration["DatabaseType"]}' is not supported.");
}

// Configure Razor Pages
builder.Services.AddRazorPages(options => options.Conventions.AuthorizeFolder("/Admin"));

// Configure identity and authentication
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AskDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

// Load configuration
builder.Services.Configure<AppSettings>(builder.Configuration);

/* Configure pipeline ****************************************************************************/

var app = builder.Build();
using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<AskDbContext>();
using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

// Migrate database to last version
context.Database.Migrate();

// Enable static file caching for one year
app.UseStaticFiles(new StaticFileOptions {
    OnPrepareResponse = ctx => ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000")
});

// Use other middleware
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapRazorPages();
app.MapControllers();

await app.RunAsync();
