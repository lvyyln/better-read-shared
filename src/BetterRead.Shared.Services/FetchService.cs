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
        private readonly string _startaddress =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=ru&source=gcsc&gss=.com&cselibv=8b2252448421acb3&cx=partner-pub-5481960811500074:3977310822&q={0}&safe=off&cse_tok=AJvRUv1B3w_6h6iGtS3IaoTQ2NXT:1584003897984&exp=csqr,cc&callback=google.search.cse.api12155";

        private readonly string _address =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.me&start={0}0&cselibv=8b2252448421acb3&cx=partner-pub-5481960811500074:3977310822&q={1}&safe=off&cse_tok=AJvRUv0KmyTUgBeBGzMYNT9ksVvz:1584003797024&exp=csqr,cc&callback=google.search.cse.api16860";

        private List<string> _check = new List<string>(); 
        public async Task<Result[]> GetDataAsync(string searchTerm)
        {
            var url = String.Format(_startaddress, searchTerm);
            var pagesCount = await GetPagesCountAsync(url);
            Task<SearchResult>[] answer = new Task<SearchResult>[pagesCount];
            
            for (int i = 0; i < pagesCount; i++)
            {
                answer[i] = GetPageAsync(String.Format(_address, i, searchTerm));
            }
            
            Task.WaitAll(answer);
            return answer.SelectMany(ans => ans.Result.results).ToArray();
        }

        private async Task<SearchResult> GetPageAsync(string url)
        {
            using var client = new HttpClient();
            var jsonAnswer = DeleteCallBackName(await client.GetStringAsync(url));
            var result = JsonConvert.DeserializeObject<SearchResult>(jsonAnswer);
            _check.Add(url); 
            return result;
        }

        private async Task<int> GetPagesCountAsync(string url)
        {
            using var client = new HttpClient();
            var jsonAnswer = DeleteCallBackName(await client.GetStringAsync(url));
            return JsonConvert.DeserializeObject<SearchResult>(jsonAnswer).cursor.pages.Count;
        }

        private string DeleteCallBackName(string jsonAnswer)
        {
            jsonAnswer = jsonAnswer.Remove(0, jsonAnswer.IndexOf("{", StringComparison.Ordinal));
            return jsonAnswer.Remove(jsonAnswer.LastIndexOf(";", StringComparison.Ordinal) - 1, 2);
        }
    }
}