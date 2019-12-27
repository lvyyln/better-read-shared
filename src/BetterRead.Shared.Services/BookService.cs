using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;
using BetterRead.Shared.Services.Abstractions;

namespace BetterRead.Shared.Services
{
    public class BookService : IBookService
    {
        private readonly IBookInfoRepository _infoRepository;
        private readonly IBookSheetsRepository _sheetsRepository;
        private readonly IBookContentsRepository _contentsRepository;
        private readonly IBookNotesRepository _notesRepository;

        public BookService(
            IBookInfoRepository infoRepository,
            IBookSheetsRepository sheetsRepository, 
            IBookContentsRepository contentsRepository, 
            IBookNotesRepository notesRepository)
        {
            _sheetsRepository = sheetsRepository;
            _infoRepository = infoRepository;
            _contentsRepository = contentsRepository;
            _notesRepository = notesRepository;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await GetBookAsync(bookId);
        }

        public async Task<Book> GetBookByUrlAsync(string url)
        {
            var bookId= GetBookId(url);
            return await GetBookAsync(bookId);
        }
        
        private static int GetBookId(string url)
        {
            var uri = new Uri(url);
            var queryId = HttpUtility.ParseQueryString(uri.Query).Get("id");
            
            return int.Parse(queryId);
        }
        
        private async Task<Book> GetBookAsync(int bookId)
        {
            var info = _infoRepository.GetBookInfoAsync(bookId);
            var sheets = _sheetsRepository.GetSheetsAsync(bookId);
            var contents = _contentsRepository.GetContentsAsync(bookId);
            //var notes = _notesRepository.GetNotesAsync(bookId);
            
            await Task.WhenAll(info, sheets, contents);
            
            return new Book
            {
                Info = await info,
                Sheets = await sheets,
                Contents = await contents,
                //Notes = await notes
            };
        }
    }
}