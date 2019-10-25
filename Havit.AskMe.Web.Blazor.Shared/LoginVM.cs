using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class LoginVM
    {
		public bool Successful { get; set; }
		public string Token { get; set; }
		public string Error { get; set; }
	}
}
