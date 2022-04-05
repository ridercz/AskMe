namespace Altairis.AskMe.Data; 

public class Question {

    [Key]
    public int Id { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public DateTime? DateAnswered { get; set; }

    [Required, MaxLength(500)]
    public string QuestionText { get; set; }

    [MaxLength(100)]
    public string DisplayName { get; set; }

    [MaxLength(100), DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    public string AnswerText { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }

}
