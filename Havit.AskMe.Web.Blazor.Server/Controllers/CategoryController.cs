using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Havit.AskMe.Web.Blazor.Server.Controllers
{
	public class CategoryController : Controller
    {
		private readonly AskDbContext askDbContext;

		public CategoryController(AskDbContext askDbContext)
		{
			this.askDbContext = askDbContext;
		}

 		[HttpGet("api/categories")]
		public async Task<List<ListItemVM>> GetCategories()
		{
			// TODO server-side caching?
			return await askDbContext.Categories.Select(c => new ListItemVM(c.Id, c.Name)).ToListAsync();
		}
    }
}
