using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security
{
	public class ApiAuthenticationStateProvider : AuthenticationStateProvider, IApiAuthenticationStateProvider
	{
		private const string StorageKey = "AuthenticationToken";

		private readonly HttpClient httpClient;
		private readonly LocalStorage localStorage;

		private string tokenCache;

		public ApiAuthenticationStateProvider(
			LocalStorage localStorage)
		{
			this.localStorage = localStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			return new AuthenticationState(await GetCurrentClaimsPrincipalAsync());
		}

		public async Task<ClaimsPrincipal> GetCurrentClaimsPrincipalAsync()
		{
			var token = await GetToken();

			return GetClaimsPrincipalFromToken(token);
		}

		private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
		{
			if (String.IsNullOrWhiteSpace(token))
			{
				return new ClaimsPrincipal(new ClaimsIdentity());
			}

			return new ClaimsPrincipal(new ClaimsIdentity(ExtractClaimsFromJwt(token), "jwt"));
		}

		public async ValueTask<string> GetToken()
		{
			return tokenCache ?? await localStorage.GetItem<string>(StorageKey);
		}

		public async Task SetAuthenticatedUser(string token)
		{
			await SetToken(token);

			var authState = new AuthenticationState(GetClaimsPrincipalFromToken(token));
			NotifyAuthenticationStateChanged(Task.FromResult(authState));
		}

		private ValueTask SetToken(string token)
		{
			tokenCache = token;
			return localStorage.SetItem(StorageKey, token);
		}

		private IEnumerable<Claim> ExtractClaimsFromJwt(string token)
		{
			var claims = new List<Claim>();
			var payload = token.Split('.')[1];
			var jsonBytes = LoadBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

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
