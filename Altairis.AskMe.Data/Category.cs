namespace Altairis.AskMe.Data;

public class Category {

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Question> Questions { get; set; } = [];

}
