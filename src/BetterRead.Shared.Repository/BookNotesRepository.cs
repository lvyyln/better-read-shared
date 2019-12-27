using System.Collections.Generic;
using System.Threading.Tasks;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;

namespace BetterRead.Shared.Repository
{
    public class BookNotesRepository : IBookNotesRepository
    {
        public Task<List<Note>> GetNotes(int bookId)
        {
            var url = string.Format(BookUrlPatterns.Notes, bookId);
            
            throw new System.NotImplementedException();
        }
    }
}