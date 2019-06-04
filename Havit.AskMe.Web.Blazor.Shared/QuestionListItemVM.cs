using System;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class QuestionListItemVM
    {
        public int QuestionId { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateCreated { get; set; }
        public string DisplayName { get; set; }
        public string QuestionText { get; set; }
        public DateTime? DateAnswered { get; set; }
        public string AnswerText { get; set; }
    }
}