using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.Mvc.Controllers {
    public class HomeController : Controller {
        private readonly AskDbContext _dc;
        private readonly AppConfiguration _cfg;

        // Constructor

        public HomeController(AskDbContext dc, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this._dc = dc;
            this._cfg = optionsSnapshot.Value;

        }
        [Route("{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Index(int pageNumber) {
            var q = this._dc.Questions
                .Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered);
            var model = await PagedModel.CreateAsync(q, pageNumber, this._cfg.PageSize);
            return this.View(model);
        }
    }
}
