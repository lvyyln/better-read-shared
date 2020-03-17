using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Author;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Domain.Search;
using BetterRead.Shared.Services.Abstractions;

namespace BetterRead.Shared.Services
{
    public class SearchService : ISearchService
    {
        private readonly IFetchService _fetchService;

        public SearchService(IFetchService fetchService)
        {
            _fetchService = fetchService;
        }

        public async Task<List<Book>> SearchBooksAsync(string bookName)
        {
            var booksData = await Search(bookName, ApiUrls.addressForBooks, SearchPatterns.BookUrl);
            return booksData.Select(book => new Book()
            {
                Info = new BookInfo()
                {
                    Name = book.titleNoFormatting.Split("- читать")[0],
                    Author = book.titleNoFormatting.Split("Автор:")[0],
                    Url = book.formattedUrl
                }
            }).ToList();
        }

        public async Task<List<Author>> SearchAuthorAsync(string authorName)
        {
            var authorsData = await Search(authorName, ApiUrls.addressForAuthors, SearchPatterns.AuthorUrl);
            return authorsData.Select(book => new Author()
            {
                AuthorName = book.titleNoFormatting.Split("-")[0],
                AuthorId = book.formattedUrl.Split("=")[1],
            }).ToList();
        }

        public async Task<List<AuthorSeries>> SearchSeriesBookAsync(string seriesName)
        {
            var seriesData = await Search(seriesName, ApiUrls.addressForSeries, SearchPatterns.SeriesUrl);
            return seriesData.Select(series => new AuthorSeries()
            {
                CollectionName = series.titleNoFormatting,
                CollectionUrl = series.formattedUrl
            }).ToList();
        }

        private async Task<List<Result>> Search(string name, string address, string urlType)
        {
            var result = await _fetchService.GetDataAsync(name, address);
            return result.Where(rs =>
                rs.formattedUrl.ToLower().Contains(urlType) &&
                rs.titleNoFormatting.ToLower().Contains((name).ToLower())).ToList();
        }
    }
}