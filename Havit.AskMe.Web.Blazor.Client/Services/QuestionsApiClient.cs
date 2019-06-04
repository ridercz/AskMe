using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
    public class QuestionsApiClient : IQuestionsApiClient
    {
        private readonly HttpClient httpClient;

        public QuestionsApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<CollectionDataResult<List<QuestionListItemVM>>> GetQuestionsAsync(QuestionListQueryFilter filter)
        {
            return httpClient.GetJsonAsync<CollectionDataResult<List<QuestionListItemVM>>>($"api/questions?pageIndex={filter.PageIndex}&answered={filter.Answered}");
        }
    }
}
