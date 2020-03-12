using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Havit.AskMe.Web.Blazor.Client.Infrastructure {
	public interface IApiAuthenticationStateProvider {
		Task<AuthenticationState> GetAuthenticationStateAsync();
		Task<ClaimsPrincipal> GetCurrentClaimsPrincipalAsync();
		Task<string> GetTokenAsync();
		Task SetAuthenticatedUserAsync(string token, bool rememberMe);
		Task SignOutAsync();
	}
}