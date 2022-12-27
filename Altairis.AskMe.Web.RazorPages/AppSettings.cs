namespace Altairis.AskMe.Web.RazorPages;

public class AppSettings {
    public const string DatabaseTypeSqlServer = "SqlServer";
    public const string DatabaseTypeSqlite = "Sqlite";

    public string DatabaseType { get; set; } = DatabaseTypeSqlite;

    public int PageSize { get; set; }

}
