using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.TagHelpers {
    [HtmlTargetElement("plainText")]
    public class PlainTextTagHelper : TagHelper {
        private readonly HtmlEncoder _encoder;

        public PlainTextTagHelper(HtmlEncoder encoder) {
            _encoder = encoder;
        }

        public string Text { get; set; }

        public bool HtmlEncode { get; set; } = true;

        public string ContainerTagName { get; set; } = string.Empty;

        public string ParagraphFormatString { get; set; } = "<p>{0}</p>";

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);
            if (string.IsNullOrWhiteSpace(this.Text)) return;

            // Create HTML output
            var paragraphs = this.Text.Split(Environment.NewLine);
            var sb = new StringBuilder();
            foreach (var line in paragraphs) {
                sb.AppendLine(string.Format(this.ParagraphFormatString, this.HtmlEncode ? _encoder.Encode(line) : line));
            }

            // Return outpuut
            output.Content.SetHtmlContent(sb.ToString());
            output.TagName = this.ContainerTagName;
        }

    }
}
