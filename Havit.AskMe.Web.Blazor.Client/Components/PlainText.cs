using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.AskMe.Web.Blazor.Client.Components {
	public class PlainText : ComponentBase {
		[Parameter]
		public string Text { get; set; }

		[Parameter]
		public bool HtmlEncode { get; set; } = true;

		[Parameter]
		public string ContainerElementName { get; set; } = string.Empty;

		[Parameter]
		public string ParagraphElementName { get; set; } = "p";

		protected override void BuildRenderTree(RenderTreeBuilder builder) {
			if (string.IsNullOrWhiteSpace(this.Text)) {
				return;
			}

			// container open
			if (!string.IsNullOrWhiteSpace(this.ContainerElementName)) {
				builder.OpenElement(0, this.ContainerElementName);
			}

			// content
			var paragraphs = this.Text.Split('\r', '\n');
			foreach (var line in paragraphs) {
				if (string.IsNullOrWhiteSpace(line)) {
					continue;
				}
				builder.OpenElement(1, this.ParagraphElementName);
				if (this.HtmlEncode) {
					builder.AddContent(2, line);
				} else {
					builder.AddMarkupContent(2, line);
				}
				builder.CloseElement(); // </p>
			}

			// container close
			if (!string.IsNullOrWhiteSpace(this.ContainerElementName)) {
				builder.CloseElement();
			}
		}
	}
}
