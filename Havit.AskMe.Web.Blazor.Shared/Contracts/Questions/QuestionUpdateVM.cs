using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Questions
{
    public class QuestionUpdateVM
    {
		public bool Success { get; set; }

		public string[] Errors { get; set; }
	}
}
