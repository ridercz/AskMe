using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.Extensions.Storage;
using Blazor.Extensions.Storage.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Havit.AskMe.Web.Blazor.Client.Infrastructure
{
	public class ApiAuthenticationStateProvider : AuthenticationStateProvider, IApiAuthenticationStateProvider
	{
		private const string StorageKey = "AuthenticationToken";

		private readonly ILocalStorage localStorage;
		private readonly ISessionStorage sessionStorage;
		private string tokenCache;

		public ApiAuthenticationStateProvider(
			ILocalStorage localStorage,
			ISessionStorage sessionStorage)
		{
			this.localStorage = localStorage;
			this.sessionStorage = sessionStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			return new AuthenticationState(await GetCurrentClaimsPrincipalAsync());
		}

		public async Task<ClaimsPrincipal> GetCurrentClaimsPrincipalAsync()
		{
			var token = await GetTokenAsync();

			return GetClaimsPrincipalFromToken(token);
		}

		private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				return new ClaimsPrincipal(new ClaimsIdentity());
			}

			return new ClaimsPrincipal(new ClaimsIdentity(ExtractClaimsFromJwt(token), "jwt"));
		}

		public async Task<string> GetTokenAsync()
		{
			return tokenCache ?? await sessionStorage.GetItem<string>(StorageKey) ?? await localStorage.GetItem<string>(StorageKey);
		}

		public async Task SetAuthenticatedUserAsync(string token, bool rememberMe)
		{
			await SetToken(token, rememberMe);

			var authState = new AuthenticationState(GetClaimsPrincipalFromToken(token));
			NotifyAuthenticationStateChanged(Task.FromResult(authState));
		}

		public Task SignOutAsync()
		{
			return SetAuthenticatedUserAsync(token: null, rememberMe: false);
		}

		private async ValueTask SetToken(string token, bool rememberMe)
		{
			tokenCache = token;
			if (token is null)
			{
				await sessionStorage.RemoveItem(StorageKey);
				await localStorage.RemoveItem(StorageKey);
			}
			else if (rememberMe)
			{
				await localStorage.SetItem(StorageKey, token);
				await sessionStorage.RemoveItem(StorageKey);
			}
			else
			{
				await sessionStorage.SetItem(StorageKey, token);
				await localStorage.RemoveItem(StorageKey);
			}
		}

		private IEnumerable<Claim> ExtractClaimsFromJwt(string token)
		{
			var claims = new List<Claim>();
			var payload = token.Split('.')[1];
			var jsonBytes = LoadBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

			if (roles != null)
			{
				if (roles.ToString().Trim().StartsWith("["))
				{
					var extractedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

					foreach (var parsedRole in extractedRoles)
					{
						claims.Add(new Claim(ClaimTypes.Role, parsedRole));
					}
				}
				else
				{
					claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
				}

				keyValuePairs.Remove(ClaimTypes.Role);
			}

			claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

			return claims;
		}

		private byte[] LoadBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}
