using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.AskMe.Data; 

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AskDbContext> {
    public AskDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<AskDbContext>();
        builder.UseSqlite(@"Data Source=.\bin\Debug\ask.design.db");
        return new AskDbContext(builder.Options);
    }
}
