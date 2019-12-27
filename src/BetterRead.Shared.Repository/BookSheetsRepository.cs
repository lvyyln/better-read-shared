using System.Collections.Generic;
using System.Threading.Tasks;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;

namespace BetterRead.Shared.Repository
{
    public class BookSheetsRepository : IBookSheetsRepository
    {
        public Task<IEnumerable<Sheet>> GetSheetsAsync(int bookId)
        {
            throw new System.NotImplementedException();
        }
    }
}