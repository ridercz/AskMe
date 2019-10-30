using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.AskMe.Web.Blazor.Client.Components
{
    public class PagerBase : ComponentBase
    {
        [Parameter]
        public PagingInfo Data { get; set; }

        [Parameter]
        public EventCallback<PageChangingEventArgs> OnPageChanging { get; set; }

        protected Task ChangePage(MouseEventArgs e, int pageNumber)
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
