using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Havit.AskMe.Web.Blazor.Server.Controllers {
	public class CategoryController : Controller {
		private readonly AskDbContext askDbContext;

		public CategoryController(AskDbContext askDbContext) {
			this.askDbContext = askDbContext;
		}

		[HttpGet("api/categories")]
		public async Task<ActionResult<List<ListItemVM>>> GetCategories() {
			// consider server-side caching / response-caching
			return Ok(await askDbContext.Categories.Select(c => new ListItemVM(c.Id, c.Name)).ToListAsync());
		}
	}
}
