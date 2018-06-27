using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.TagHelpers {
    [HtmlTargetElement(Attributes ="for-login-status")]
    public class ForLoginStatusTagHelper : TagHelper {
        private readonly HttpContext _httpContext;

        public ForLoginStatusTagHelper(IHttpContextAccessor httpContextAccessor) {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public bool ForLoginStatus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);

            if(_httpContext.User.Identity.IsAuthenticated != this.ForLoginStatus) {
                output.SuppressOutput();
            }
        }

    }
}
