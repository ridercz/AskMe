using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security
{
	public interface IApiAuthenticationStateProvider
	{
		Task<AuthenticationState> GetAuthenticationStateAsync();
		Task<ClaimsPrincipal> GetCurrentClaimsPrincipalAsync();
		ValueTask<string> GetToken();
		Task SetAuthenticatedUser(string token);
	}
}