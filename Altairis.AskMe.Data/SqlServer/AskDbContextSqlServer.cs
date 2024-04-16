using Microsoft.EntityFrameworkCore.Design;

namespace Altairis.AskMe.Data.SqlServer;

public class AskDbContextSqlServer(DbContextOptions options) : AskDbContext(options) {
}

public class AskDbContextSqlServerDesignTimeFactory : IDesignTimeDbContextFactory<AskDbContextSqlServer> {

    public AskDbContextSqlServer CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<AskDbContext>();
        builder.UseSqlServer(@"SERVER=.\SqlExpress;DATABASE=AskMe.design;TRUSTED_IDENTITY=yes;TRUSTSERVERCERTIFICATE=yes;");
        return new AskDbContextSqlServer(builder.Options);
    }

}