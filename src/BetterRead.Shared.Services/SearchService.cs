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
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.ru&start={0}0&cselibv=none&cx=008365236763766494822:dhpowygqvyl&q={1}&cse_tok=AJvRUv0lHJnI7HZMhyN3qN8S_yuP:1584282440908&sort=&exp=csqr,cc&callback=google.search.cse.api7547";

        private readonly string _addressForBooks =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.ru&start={0}0&cselibv=none&cx=008365236763766494822:c5pfn8bxmlg&q={1}&cse_tok=AJvRUv1x-EH_IrMLKDPjFoGpWk4T:1584281846966&sort=&exp=csqr,cc&callback=google.search.cse.api4900";

        private readonly string _addressForSeries =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.ru&start={0}0&cselibv=none&cx=008365236763766494822:cx2radstijl&q={1}&cse_tok=AJvRUv1GEKrFAnQEmR-GV628aHMq:1584283890313&sort=&exp=csqr,cc&callback=google.search.cse.api17514";

        private readonly string BookUrl = ("loveread.ec/view_global.php?id=");
        private readonly string AuthorUrl = ("loveread.ec/biography-author.php?author=");
        private readonly string SeriesUrl = ("loveread.me/series-books.php?id=");
        private readonly IFetchService _fetchService;

        public SearchService(IFetchService fetchService)
        {
            _fetchService = fetchService;
        }

        public async Task<Book[]> SearchBooksAsync(string bookName)
        {
            var result = await _fetchService.GetDataAsync(bookName, _addressForBooks);
            var booksData = result.Where(rs =>
                rs.formattedUrl.ToLower().Contains(BookUrl) &&
                rs.titleNoFormatting.ToLower().Contains((bookName).ToLower()));

            return booksData.Select(book => new Book()
            {
                Info = new BookInfo()
                {
                    Name = book.titleNoFormatting,
                    Author = book.contentNoFormatting,
                    Url = book.unescapedUrl
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
                AuthorId = book.formattedUrl,
            }).ToArray();
        }

        public async Task<AuthorSeries[]> SearchSeriesBookAsync(string seriesName)
        {
            var result = await _fetchService.GetDataAsync(seriesName, _addressForSeries);
            var booksData = result.Where(rs =>
                rs.formattedUrl.ToLower().Contains(SeriesUrl) &&
                rs.titleNoFormatting.ToLower().Contains((seriesName).ToLower()));

            return booksData.Select(book => new AuthorSeries()
            {
                CollectionName = book.titleNoFormatting,
                CollectionAuthor = book.contentNoFormatting,
                CollectionUrl = book.unescapedUrl
            }).ToArray();
        }
    }
}