using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class QuestionListQueryFilter : QueryFilterBase
    {
        public bool? Answered { get; set; }
    }
}
