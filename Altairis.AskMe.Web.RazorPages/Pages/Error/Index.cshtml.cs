namespace Altairis.AskMe.Web.RazorPages.Pages.Error {
    public class IndexModel : PageModel {

        public int ErrorCode { get; set; }

        public void OnGet(int errorCode) => this.ErrorCode = errorCode;
    }
}