namespace Altairis.AskMe.Web.Mvc.Models.Home;

public class FirstRunModel {

    [Required(ErrorMessage = "Není zadáno uživatelské jméno"), MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Není zadáno heslo"), MaxLength(100), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Není zadána kontrola hesla"), Compare(nameof(Password), ErrorMessage = "Heslo a kontrola hesla nesouhlasí"), MaxLength(100), DataType(DataType.Password)]
    public string PasswordConfirmation { get; set; } = string.Empty;

    public bool SeedDemoData { get; set; }

}
