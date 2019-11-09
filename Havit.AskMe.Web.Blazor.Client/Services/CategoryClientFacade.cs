using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services {
	public class CategoryClientFacade : ICategoryClientFacade {
		private readonly HttpClient httpClient;

		public CategoryClientFacade(HttpClient httpClient) {
			this.httpClient = httpClient;
		}

		public Task<List<ListItemVM>> GetAll() {
			// consider client-side caching
			return httpClient.GetJsonAsync<List<ListItemVM>>($"api/categories");
		}
	}
}
