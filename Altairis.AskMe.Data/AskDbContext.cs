using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Data {
    public class AskDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
        private const int SeedCategoryCount = 5;            // Number of categories

        // Constructor

        public AskDbContext(DbContextOptions<AskDbContext> options)
            : base(options) { }

        // Entities

        public DbSet<Question> Questions { get; set; }

        public DbSet<Category> Categories { get; set; }

        // Seeding

        public void Seed() {
            // Add categories
            if (!this.Categories.Any()) {
                for (int i = 1; i <= SeedCategoryCount; i++) {
                    this.Categories.Add(new Category { Name = $"Category {i}" });
                }
            }
        }
    }
}
