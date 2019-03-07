using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.AskMe.Web.DotVVM.Dto {
    public class UnansweredQuestionDto {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string QuestionText { get; set; }

        public string DisplayName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
