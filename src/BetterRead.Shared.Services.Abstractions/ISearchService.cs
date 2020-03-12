using System.Threading.Tasks;
using BetterRead.Shared.Domain.Author;
using BetterRead.Shared.Domain.Book;

namespace BetterRead.Shared.Services.Abstractions
{
    public interface ISearchService
    {
        Task<Book[]> SearchBooksAsync(string bookName);
        Task<Author[]> SearchAuthorAsync(string bookName);
        Task<AuthorSeries[]> SearchSeriesBookAsync(string bookName);
    }
}