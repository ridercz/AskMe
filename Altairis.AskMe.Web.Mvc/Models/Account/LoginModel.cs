using System.ComponentModel.DataAnnotations;

namespace Altairis.AskMe.Web.Mvc.Models.Account {
    public class LoginModel {
        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
