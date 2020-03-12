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
        private readonly string BookUrl = ($"loveread.ec/view_global.php?id=");
        private readonly IFetchService _fetchService;

        public SearchService(IFetchService fetchService)
        {
            _fetchService = fetchService;
        }

        public async Task<Book[]> SearchBooksAsync(string bookName)
        {
            var result = await _fetchService.GetDataAsync(bookName);
            var booksData = result.Where(rs => rs.formattedUrl.Contains(BookUrl) && rs.titleNoFormatting.Contains(bookName));
            
            return booksData.Select(book => new Book()
            {
                Info = new BookInfo()
                {
                    Name = book.titleNoFormatting,
                    Author = book.contentNoFormatting
                }
            }).ToArray();
        }

        public Task<Author[]> SearchAuthorAsync(string bookName)
        {
            throw new System.NotImplementedException();
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