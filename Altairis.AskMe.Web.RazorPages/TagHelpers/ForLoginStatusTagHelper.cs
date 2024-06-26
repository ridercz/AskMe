﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.RazorPages.TagHelpers;

[HtmlTargetElement(Attributes = "for-login-status")]
public class ForLoginStatusTagHelper(IHttpContextAccessor httpContextAccessor) : TagHelper {
    private readonly HttpContext httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentException("HTTP context not available.", nameof(httpContextAccessor));

    public bool ForLoginStatus { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        base.Process(context, output);

        if (this.httpContext.User?.Identity?.IsAuthenticated != this.ForLoginStatus) {
            output.SuppressOutput();
        }
    }

}
