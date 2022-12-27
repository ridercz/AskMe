using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages;

public class FirstRunModel : PageModel {
    private readonly AskDbContext dc;
    private readonly UserManager<ApplicationUser> userManager;

    public FirstRunModel(AskDbContext dc, UserManager<ApplicationUser> userManager) {
        this.dc = dc;
        this.userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required(ErrorMessage = "Není zadáno uživatelské jméno"), MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Není zadáno heslo"), MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Není zadána kontrola hesla"), Compare(nameof(Password), ErrorMessage = "Heslo a kontrola hesla nesouhlasí"), MaxLength(100), DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; } = string.Empty;

        public bool SeedDemoData { get; set; }

    }

    public async Task<ActionResult> OnGetAsync() => await this.dc.Users.AnyAsync() ? this.NotFound() : this.Page();

    public async Task<ActionResult> OnPostAsync() {
        if (await this.dc.Users.AnyAsync()) return this.NotFound();

        // Create new user
        if (this.ModelState.IsValid) {
            var result = await this.userManager.CreateAsync(new ApplicationUser { UserName = this.Input.UserName }, password: this.Input.Password);
            if (result.Succeeded) {
                if (this.Input.SeedDemoData) this.dc.Seed();
                return this.RedirectToPage("Index");
            }
            foreach (var item in result.Errors) {
                this.ModelState.AddModelError(string.Empty, $"{item.Description} [{item.Code}]");
            }
        }
        return this.Page();
    }

}
