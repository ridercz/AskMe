using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Account
{
    public class ChangePasswordVM
    {
		public bool Succeeded { get; set; }

		public string[] Errors { get; set; }
	}
}
