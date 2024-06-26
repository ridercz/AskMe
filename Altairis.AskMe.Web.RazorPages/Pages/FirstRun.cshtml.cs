using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages;

public class FirstRunModel(AskDbContext dc, UserManager<ApplicationUser> userManager) : PageModel {
    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        [Required(ErrorMessage = "Nen� zad�no u�ivatelsk� jm�no"), MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nen� zad�no heslo"), MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nen� zad�na kontrola hesla"), Compare(nameof(Password), ErrorMessage = "Heslo a kontrola hesla nesouhlas�"), MaxLength(100), DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; } = string.Empty;

        public bool SeedDemoData { get; set; }

    }

    public async Task<ActionResult> OnGetAsync() => await dc.Users.AnyAsync() ? this.NotFound() : this.Page();

    public async Task<ActionResult> OnPostAsync() {
        if (await dc.Users.AnyAsync()) return this.NotFound();

        // Create new user
        if (this.ModelState.IsValid) {
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = this.Input.UserName }, password: this.Input.Password);
            if (result.Succeeded) {
                if (this.Input.SeedDemoData) dc.Seed();
                return this.RedirectToPage("Index");
            }
            foreach (var item in result.Errors) {
                this.ModelState.AddModelError(string.Empty, $"{item.Description} [{item.Code}]");
            }
        }
        return this.Page();
    }

}
