global using Altairis.AskMe.Data;
global using Altairis.AskMe.Web.RazorPages;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container *****************************************************************/

// Configure DB context
builder.Services.AddDbContext<AskDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("AskDB"));
});

// Configure Razor Pages
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Admin");
});

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

var app = builder.Build();

/* Configure pipeline ****************************************************************************/

// Show detailed errors in development environment
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
    app.UseBrowserLink();
}

using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<AskDbContext>();
using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

// Migrate database to last version
context.Database.Migrate();

// Seed initial data if in development environment
if (app.Environment.IsDevelopment()) {
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
    endpoints.MapRazorPages();
});

await app.RunAsync();
