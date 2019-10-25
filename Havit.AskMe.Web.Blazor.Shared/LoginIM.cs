using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class LoginIM
    {
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
