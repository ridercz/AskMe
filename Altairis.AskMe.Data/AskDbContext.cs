global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
using NLipsum.Core;

namespace Altairis.AskMe.Data;

public abstract class AskDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
    private const int SEED_MAX_QUESTION_SENTENCES = 5;
    private const int SEED_MAX_ANSWER_PARAGRAPHS = 20;
    private const int SEED_MAX_NAME_WORDS = 4;

    // Constructor

    public AskDbContext(DbContextOptions options) : base(options) { }

    // Entities

    public DbSet<Question> Questions => this.Set<Question>();

    public DbSet<Category> Categories => this.Set<Category>();

    // Seeding

    public void Seed(int numberOfCategories = 5, int numberOfQuestions = 50, int anonymousRate = 75, int answeredRate = 50) {
        // Add categories
        if (!this.Categories.Any()) {
            for (int i = 1; i <= numberOfCategories; i++) {
                this.Categories.Add(new Category { Name = $"Category {i}" });
            }
            this.SaveChanges();
        }

        // Add questions
        if (!this.Questions.Any()) {
            var categoryIds = this.Categories.Select(c => c.Id).ToArray();
            var rng = new Random();
            var rtg = new LipsumGenerator();
            for (int i = 0; i < numberOfQuestions; i++) {
                var nq = new Question {
                    QuestionText = string.Join(" ", rtg.GenerateSentences(rng.Next(SEED_MAX_QUESTION_SENTENCES) + 1)),
                    DateCreated = DateTime.Now.AddHours(i - numberOfQuestions),
                    CategoryId = categoryIds[rng.Next(categoryIds.Length)]
                };
                if (nq.QuestionText.Length > 500) nq.QuestionText = string.Concat(nq.QuestionText.AsSpan(0, 499), ".");
                if (rng.Next(100) > anonymousRate) {
                    var name = rtg.GenerateWords(rng.Next(SEED_MAX_NAME_WORDS) + 1);
                    nq.DisplayName = string.Join(' ', name);
                    nq.EmailAddress = name[0].ToLower() + "@example.com";
                }
                if (rng.Next(100) <= answeredRate) {
                    nq.AnswerText = string.Join(Environment.NewLine, rtg.GenerateParagraphs(rng.Next(SEED_MAX_ANSWER_PARAGRAPHS) + 1));
                    nq.DateAnswered = nq.DateCreated.AddMinutes(rng.Next((int)DateTime.Now.Subtract(nq.DateCreated).TotalMinutes));
                }
                this.Questions.Add(nq);
            }
            this.SaveChanges();
        }


    }
}
