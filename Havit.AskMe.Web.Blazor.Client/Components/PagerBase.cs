using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Components
{
    public class PagerBase : ComponentBase
    {
        [Parameter]
        protected PagingInfo Data { get; set; }

        [Parameter]
        private EventCallback<PageChangingEventArgs> OnPageChanging { get; set; }

        protected Task ChangePage(UIMouseEventArgs e, int pageNumber)
        {
			var arg = new PageChangingEventArgs() { NewPageNumber = pageNumber };
			return OnPageChanging.InvokeAsync(arg);
        }

        public class PageChangingEventArgs
        {
            public int NewPageNumber { get; internal set; }
        }
    }
}
