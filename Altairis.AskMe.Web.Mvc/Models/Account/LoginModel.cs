namespace Altairis.AskMe.Web.Mvc.Models.Account;

public class LoginModel {
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
