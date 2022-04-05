namespace Altairis.AskMe.Web.Mvc.Models.Admin;

public class ChangePasswordModel {
    [Required, DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), MinLength(12)]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
