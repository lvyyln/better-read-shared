using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BetterRead.Shared.Domain.Author;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Services.Abstractions;

namespace BetterRead.Shared.Services
{
    public class SearchService : ISearchService
    {
        private readonly string _addressForAuthors =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.ru&start={0}0&cselibv=none&cx=008365236763766494822:c5pfn8bxmlg&q={1}&cse_tok=AJvRUv1xTa0B7YCZHNIFBIqy-Xnb:1584090375118&sort=&exp=csqr,cc&callback=google.search.cse.api10657";

        private readonly string _addressForBooks =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.ru&start={0}0&cselibv=none&cx=008365236763766494822:c5pfn8bxmlg&q={1}&cse_tok=AJvRUv2_9bYbKKY4qqOc7UqogZHQ:1584092502410&sort=&exp=csqr,cc&callback=google.search.cse.api9589";


        private readonly string BookUrl = ("loveread.ec/view_global.php?id=");
        private readonly string AuthorUrl = ("loveread.ec/biography-author.php?author=");
        private readonly IFetchService _fetchService;

        public SearchService(IFetchService fetchService)
        {
            _fetchService = fetchService;
        }

        public async Task<Book[]> SearchBooksAsync(string bookName)
        {
            var result = await _fetchService.GetDataAsync(bookName, _addressForBooks);
            var booksData = result.Where(rs =>
                rs.formattedUrl.Contains(BookUrl) && rs.titleNoFormatting.Contains(bookName));

            return booksData.Select(book => new Book()
            {
                Info = new BookInfo()
                {
                    Name = book.titleNoFormatting,
                    Author = book.contentNoFormatting
                }
            }).ToArray();
        }

        public async Task<Author[]> SearchAuthorAsync(string authorName)
        {
            var result = await _fetchService.GetDataAsync(authorName, _addressForAuthors);
            var authorsData = result.Where(rs =>
                rs.formattedUrl.Contains(AuthorUrl) && rs.titleNoFormatting.Contains(authorName));

            return authorsData.Select(book => new Author()
            {
                AuthorName = book.titleNoFormatting,
                AuthorId = book.formattedUrl
            }).ToArray();
        }

        public Task<AuthorSeries[]> SearchSeriesBookAsync(string bookName)
        {
            throw new System.NotImplementedException();
        }

        private string GetTitle(string regexString, string toMatch)
        {
            var regex = new Regex(regexString);
            return regex.Matches(toMatch)[0].Value;
        }
    }
}