using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BetterRead.Shared.Domain.Search;
using BetterRead.Shared.Services.Abstractions;
using Newtonsoft.Json;

namespace BetterRead.Shared.Services
{
    public class FetchService : IFetchService
    {
        public async Task<Result[]> GetDataAsync(string searchTerm, string address)
        {
            var url = String.Format(address, 1, searchTerm);
            var pagesCount = await GetPagesCountAsync(url);
            Task<SearchResult>[] answer = new Task<SearchResult>[pagesCount];

            for (int i = 0; i < pagesCount; i++)
            {
                answer[i] = GetPageAsync(String.Format(address, i, searchTerm));
            }

            Task.WaitAll(answer);
            return answer.SelectMany(ans => ans.Result.results).ToArray();
        }

        private async Task<SearchResult> GetPageAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var jsonAnswer = DeleteCallBackName(await client.GetStringAsync(new Uri(url)));
                return JsonConvert.DeserializeObject<SearchResult>(jsonAnswer);
            }
        }

        private async Task<int> GetPagesCountAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var jsonAnswer = DeleteCallBackName(await client.GetStringAsync(url));
                return JsonConvert.DeserializeObject<SearchResult>(jsonAnswer).cursor.pages.Count;
            }
        }

        private string DeleteCallBackName(string jsonAnswer)
        {
            jsonAnswer = jsonAnswer.Remove(0, jsonAnswer.IndexOf("{", StringComparison.Ordinal));
            return jsonAnswer.Remove(jsonAnswer.LastIndexOf(";", StringComparison.Ordinal) - 1, 2);
        }
    }
}