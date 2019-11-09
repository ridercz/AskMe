using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.RazorPages.TagHelpers {
    [HtmlTargetElement("time", Attributes = "value")]
    public class TimeTagHelper : TagHelper {

        public DateTime? Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (!this.Value.HasValue) {
                // Value is not specified
                if (output.Content.IsEmptyOrWhiteSpace) output.Content.SetContent("nikdy");
            } else {
                // Value is specified
                var dateValue = this.Value.Value;

                // Add datetime attribute if not already present
                if (context.AllAttributes["datetime"] == null) {
                    output.Attributes.Add("datetime", dateValue.ToString("s"));
                }

                // Add title attribute if not already present
                if (context.AllAttributes["title"] == null) {
                    output.Attributes.Add("title", $"{dateValue:D} {dateValue:T}");
                }

                // Set content if not present
                if (output.Content.IsEmptyOrWhiteSpace) {
                    if (dateValue.Date == DateTime.Today) {
                        output.Content.SetContent($"dnes, {dateValue:t}");
                    } else if (dateValue.Date == DateTime.Today.AddDays(-1)) {
                        output.Content.SetContent($"včera, {dateValue:t}");
                    } else if (dateValue.Date == DateTime.Today.AddDays(1)) {
                        output.Content.SetContent($"zítra, {dateValue:t}");
                    } else {
                        output.Content.SetContent($"{dateValue:d}, {dateValue:t}");
                    }
                }
            }

            base.Process(context, output);
        }
    }
}
