using System.Collections.Generic;
using System.Threading.Tasks;
using BetterRead.Shared.Domain.Book;

namespace BetterRead.Shared.Repository.Abstractions
{
    public interface IBookNotesRepository
    {
        Task<List<Note>> GetNotes(int bookId);
    }
}