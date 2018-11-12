using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.Mvc.Models.Home {
    public class QuestionsModel : PagedModel<Question> {

        public IEnumerable<SelectListItem> Categories { get; set; }

        public InputModel Input { get; set; }

        // Input model

        public class InputModel {
            [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
            public string QuestionText { get; set; }

            [MaxLength(100)]
            public string DisplayName { get; set; }

            [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
            public string EmailAddress { get; set; }

            public int CategoryId { get; set; }
        }

    }
}
