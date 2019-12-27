using System.Threading.Tasks;
using BetterRead.Shared.Domain.Book;

namespace BetterRead.Shared.Repository.Abstractions
{
    public interface IBookInfoRepository
    {
        Task<BookInfo> GetBookInfoAsync(int bookId);
    }
}