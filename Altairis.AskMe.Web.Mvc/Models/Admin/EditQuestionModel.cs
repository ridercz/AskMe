using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.Mvc.Models.Admin;

public class EditQuestionModel {
    [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
    public string QuestionText { get; set; } = string.Empty;

    public string? AnswerText { get; set; }

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
    public string? EmailAddress { get; set; }

    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new HashSet<SelectListItem>();
}
