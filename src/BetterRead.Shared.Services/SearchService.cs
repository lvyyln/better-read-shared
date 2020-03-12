using System.Linq;
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
            var books = result.Where(rs => rs.formattedUrl.Contains(BookUrl) && rs.title.Contains(bookName));
            return null;
        }

        public Task<Author[]> SearchAuthorAsync(string bookName)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthorSeries[]> SearchSeriesBookAsync(string bookName)
        {
            throw new System.NotImplementedException();
        }
    }
}