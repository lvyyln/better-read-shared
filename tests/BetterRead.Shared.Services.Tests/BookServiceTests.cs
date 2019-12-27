using System.Text;
using System.Threading.Tasks;
using BetterRead.Shared.Repository;
using BetterRead.Shared.Repository.Abstractions;
using Xunit;

namespace BetterRead.Shared.Services.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public async Task GetBook_WhenValidUrl_ShouldNotBeNull()
        {
            //Assign
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            const string bookUrl = "http://loveread.ec/view_global.php?id=45105";
            var sut = GetSut();
            
            //Act
            var book = await sut.GetBookByUrlAsync(bookUrl);

            //Assert
            Assert.NotNull(book);
        }

        private static BookService GetSut(
            IBookInfoRepository infoRepository = null,
            IBookSheetsRepository sheetsRepository = null,
            IBookContentsRepository contentsRepository = null,
            IBookNotesRepository notesRepository = null)
        {
            if (infoRepository == null) infoRepository = new BookInfoRepository();
            if (sheetsRepository == null) sheetsRepository = new BookSheetsRepository();
            if (contentsRepository == null) contentsRepository = new BookContentsRepository();
            if (notesRepository == null) notesRepository = new BookNotesRepository();

            return new BookService(infoRepository, sheetsRepository, contentsRepository, notesRepository);
        }
    }
}