using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
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

            if (Context.Parameters.TryGetValue("pageNumber", out var pageIndex)) {
                if (pageIndex != "") {
                    this.Data.PagingOptions.PageIndex = Convert.ToInt32(pageIndex) - 1;
                }
            } else {
                this.Data.PagingOptions.PageIndex = 0;
            }
            this.Data.LoadFromQueryable(this.DataSource);
        }

    }
}
