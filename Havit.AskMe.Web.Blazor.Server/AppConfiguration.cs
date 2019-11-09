namespace Havit.AskMe.Web.Blazor.Server {
	public class AppConfiguration {
		public int PageSize { get; set; }
		public string JwtIssuer { get; set; }
		public string JwtAudience { get; set; }
		public string JwtSecurityKey { get; set; }
		public int JwtExpirationInDays { get; set; }
	}
}
