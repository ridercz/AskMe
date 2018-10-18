using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.AskMe.Web.TagHelpers {
    [HtmlTargetElement(Attributes ="for-login-status")]
    public class ForLoginStatusTagHelper : TagHelper {
        private readonly HttpContext _httpContext;

        public ForLoginStatusTagHelper(IHttpContextAccessor httpContextAccessor) {
            this._httpContext = httpContextAccessor.HttpContext;
        }

        public bool ForLoginStatus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);

            if(this._httpContext.User.Identity.IsAuthenticated != this.ForLoginStatus) {
                output.SuppressOutput();
            }
        }

    }
}
