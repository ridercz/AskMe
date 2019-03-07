using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.DotVVM.Dto;
using AutoMapper;

namespace Altairis.AskMe.Web.DotVVM {
    public static class MapperConfig {
        public static void Configure() {
            Mapper.Initialize(m => {
                m.CreateMap<Question, AnsweredQuestionDto>();
                m.CreateMap<Question, UnansweredQuestionDto>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
