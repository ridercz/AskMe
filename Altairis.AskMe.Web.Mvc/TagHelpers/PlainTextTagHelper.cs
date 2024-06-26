﻿using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.Mvc.TagHelpers;

[HtmlTargetElement("plainText")]
public class PlainTextTagHelper(HtmlEncoder encoder) : TagHelper {
    public string? Text { get; set; }

    public bool HtmlEncode { get; set; } = true;

    public string ContainerTagName { get; set; } = string.Empty;

    public string ParagraphFormatString { get; set; } = "<p>{0}</p>";

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        base.Process(context, output);
        if (string.IsNullOrWhiteSpace(this.Text)) {
            output.SuppressOutput();
            return;
        }

        // Create HTML output
        var paragraphs = this.Text.Split('\r', '\n');
        var sb = new StringBuilder();
        foreach (var line in paragraphs) {
            if (string.IsNullOrWhiteSpace(line)) continue;
            sb.AppendLine(string.Format(this.ParagraphFormatString, this.HtmlEncode ? encoder.Encode(line) : line));
        }

        // Return outpuut
        var html = sb.ToString();
        if (string.IsNullOrWhiteSpace(html)) {
            output.SuppressOutput();
        } else {
            output.Content.SetHtmlContent(html);
            output.TagName = this.ContainerTagName;
        }
    }

}
