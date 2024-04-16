using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.AskMe.Data.Sqlite;

public class AskDbContextSqlite(DbContextOptions options) : AskDbContext(options) {
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AskDbContextSqlite> {

    public AskDbContextSqlite CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<AskDbContextSqlite>();
        builder.UseSqlite(@"Data Source=.\bin\Debug\ask.design.db");
        return new AskDbContextSqlite(builder.Options);
    }

}