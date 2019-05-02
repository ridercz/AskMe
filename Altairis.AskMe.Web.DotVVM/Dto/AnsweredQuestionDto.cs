using System;

namespace Altairis.AskMe.Web.DotVVM.Dto {
    public class AnsweredQuestionDto {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateAnswered { get; set; }

        public string QuestionText { get; set; }

        public string DisplayName { get; set; }

        public string AnswerText { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
