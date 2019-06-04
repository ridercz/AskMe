using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Havit.AskMe.Web.Blazor.Client.Components
{
    public class Time : ComponentBase
    {
        [Parameter]
        private DateTime? Value { get; set; }

        [Parameter]
        private RenderFragment ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "time");
            if (!this.Value.HasValue)
            {
                // Value is not specified
                builder.AddContent(1, "nikdy");
            }
            else
            {
                // Value is specified
                var dateValue = this.Value.Value;

                builder.AddAttribute(2, "datetime", dateValue.ToString("s"));
                builder.AddAttribute(3, "title", $"{dateValue:D} {dateValue:T}");

                // Set content if not present
                if (this.ChildContent == null)
                {
                    if (dateValue.Date == DateTime.Today)
                    {
                        builder.AddContent(4, $"dnes, {dateValue:t}");
                    }
                    else if (dateValue.Date == DateTime.Today.AddDays(-1))
                    {
                        builder.AddContent(5, $"včera, {dateValue:t}");
                    }
                    else if (dateValue.Date == DateTime.Today.AddDays(1))
                    {
                        builder.AddContent(6, $"zítra, {dateValue:t}");
                    }
                    else
                    {
                        builder.AddContent(7, $"{dateValue:d}, {dateValue:t}");
                    }
                }
                else
                {
                    builder.AddContent(8, this.ChildContent);
                }
            }
            builder.CloseElement();
        }
    }
}
