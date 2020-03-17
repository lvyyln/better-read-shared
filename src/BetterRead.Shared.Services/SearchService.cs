using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BetterRead.Shared.Domain.Author;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Domain.Search;
using BetterRead.Shared.Services.Abstractions;

namespace BetterRead.Shared.Services
{
    public class SearchService : ISearchService
    {
        private readonly string _addressForAuthors =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.com&start={0}0&cselibv=none&cx=008365236763766494822:dhpowygqvyl&q={1}&cse_tok=AJvRUv0NKoTynhl-Sn0bhU-14b0f:1584434671724&sort=&exp=csqr,cc&oq={1}&gs_l=partner-generic.12...1730.4522.0.5523.8.8.0.0.0.0.127.701.2j6.8.0.gsnos%2Cn%3D13...0.2808j1572510j15j1...1.34.partner-generic..8.0.0.LY3HY0_R7Go&callback=google.search.cse.api14046";

        private readonly string _addressForBooks =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.com&start={0}0&cselibv=none&cx=008365236763766494822:c5pfn8bxmlg&q={1}&cse_tok=AJvRUv1pmsmWMN7LqWzZL7pYC5de:1584434833137&sort=&exp=csqr,cc&oq={1}&gs_l=partner-generic.12...3637.3980.0.4943.4.4.0.0.0.0.180.400.2j2.4.0.gsnos%2Cn%3D13...0.1319j916197j7...1.34.partner-generic..4.0.0.8K3qFAm3zb4&callback=google.search.cse.api14374";

        private readonly string _addressForSeries =
            "https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.com&start={0}0&cselibv=none&cx=008365236763766494822:cx2radstijl&q={1}&cse_tok=AJvRUv1pLtWnTy_LPrVnLBCAX3QK:1584434896709&sort=&exp=csqr,cc&oq={1}&gs_l=partner-generic.12...2462.4565.0.5221.9.9.0.0.0.0.126.914.1j8.9.0.gsnos%2Cn%3D13...0.2440j1058550j12j1...1.34.partner-generic..9.0.0.1-_zTSkvGiM&callback=google.search.cse.api17384";

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
            var booksData = await Search(bookName, _addressForBooks, BookUrl);
            return booksData.Select(book => new Book()
            {
                Info = new BookInfo()
                {
                    Name = book.titleNoFormatting.Split("- читать")[0],
                    Author = book.titleNoFormatting.Split("Автор:")[0],
                    Url = book.formattedUrl
                }
            }).ToArray();
        }

        public async Task<Author[]> SearchAuthorAsync(string authorName)
        {
            var authorsData = await Search(authorName, _addressForAuthors, AuthorUrl);
            return authorsData.Select(book => new Author()
            {
                AuthorName = book.titleNoFormatting.Split("-")[0],
                AuthorId = book.formattedUrl.Split("=")[1],
            }).ToArray();
        }

        public async Task<AuthorSeries[]> SearchSeriesBookAsync(string seriesName)
        {
            var booksData = await Search(seriesName, _addressForBooks, BookUrl);
            return booksData.Select(series => new AuthorSeries()
            {
                CollectionName = series.titleNoFormatting,
                CollectionUrl = series.formattedUrl
            }).ToArray();
        }

        private async Task<Result[]> Search(string name, string address, string urlType)
        {
            var result = await _fetchService.GetDataAsync(name, address);
            return result.Where(rs =>
                rs.formattedUrl.ToLower().Contains(urlType) &&
                rs.titleNoFormatting.ToLower().Contains((name).ToLower())).ToArray();
        }
    }
}