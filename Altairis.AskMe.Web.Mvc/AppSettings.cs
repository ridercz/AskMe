namespace Altairis.AskMe.Web.Mvc;

public class AppSettings {
    public const string DatabaseTypeSqlServer = "SqlServer";
    public const string DatabaseTypeSqlite = "Sqlite";

    public string DatabaseType { get; set; } = DatabaseTypeSqlite;

    public int PageSize { get; set; } = 10;

    public string Title { get; set; } = "ASKme";

    public string Subtitle { get; set; } = "zeptej se mě na co chceš, já na co chci odpovím";

}
