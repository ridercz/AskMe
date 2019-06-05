using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
	public class CategoryClientFacade : ICategoryClientFacade
	{
		private readonly HttpClient httpClient;

		public CategoryClientFacade(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public Task<List<ListItemVM>> GetAll()
		{
			// TODO caching
			return httpClient.GetJsonAsync<List<ListItemVM>>($"api/categories");
		}
	}
}
