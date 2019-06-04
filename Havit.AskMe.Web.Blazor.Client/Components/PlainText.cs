using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Havit.AskMe.Web.Blazor.Client.Components
{
    public class PlainText : ComponentBase
    {
        [Parameter]
        private string Text { get; set; }

        [Parameter]
        private bool HtmlEncode { get; set; } = true;

        [Parameter]
        private string ContainerElementName { get; set; } = string.Empty;

        [Parameter]
        private string ParagraphElementName { get; set; } = "p";

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                return;
            }

            // container open
            if (!string.IsNullOrWhiteSpace(this.ContainerElementName))
            {
                builder.OpenElement(0, this.ContainerElementName);
            }

            // content
            var paragraphs = this.Text.Split('\r', '\n');
            foreach (var line in paragraphs)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                builder.OpenElement(1, this.ParagraphElementName);
                if (this.HtmlEncode)
                {
                    builder.AddContent(2, line);
                }
                else
                {
                    builder.AddMarkupContent(2, line);
                }
                builder.CloseElement(); // </p>
            }

            // container close
            if (!string.IsNullOrWhiteSpace(this.ContainerElementName))
            {
                builder.CloseElement();
            }
        }
    }
}
