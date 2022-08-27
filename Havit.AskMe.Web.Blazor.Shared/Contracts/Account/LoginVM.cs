namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Account {
	public class LoginVM {
		public bool Successful { get; set; }
		public string Token { get; set; }
		public string Error { get; set; }
	}
}
