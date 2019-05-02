using System;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public abstract class PagedViewModel<TItem> : MasterPageViewModel {
        public PagedViewModel(IHostingEnvironment env, IOptionsSnapshot<AppConfiguration> config) : base(env) {
            this.Data.PagingOptions.PageSize = config.Value.PageSize;
        }

        protected abstract IQueryable<TItem> DataSource { get; }

        public GridViewDataSet<TItem> Data { get; set; } = new GridViewDataSet<TItem>();

        public override async Task PreRender() {
            await base.PreRender();

            var pageNumber = Convert.ToInt32(this.Context.Parameters["pageNumber"]);
            this.Data.PagingOptions.PageIndex = pageNumber == 0 ? 0 : pageNumber - 1;
            this.Data.LoadFromQueryable(this.DataSource);
        }

    }
}
