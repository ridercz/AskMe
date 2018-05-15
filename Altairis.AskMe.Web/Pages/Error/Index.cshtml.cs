using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.AskMe.Web.Pages.Error {
    public class IndexModel : PageModel {

        public int ErrorCode { get; set; }

        public void OnGet(int errorCode) {
            this.ErrorCode = errorCode;
        }
    }
}