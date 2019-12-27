using System.Linq;
using System.Threading.Tasks;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;
using HtmlAgilityPack;

namespace BetterRead.Shared.Repository
{
    public class BookInfoRepository : IBookInfoRepository
    {
        public async Task<BookInfo> GetBookInfo(int bookId)
        {
            var url = string.Format(BookUrlPatterns.General, bookId);
            var htmlDocument = await new HtmlWeb().LoadFromWebAsync(url);
            var documentNode = htmlDocument.DocumentNode;

            var bookInfo = new BookInfo
            {
                BookId = bookId,
                Url = url,
                Name = GetBookInfoNameFromPage(bookId, documentNode),
                Author = GetBookAuthorFromPage(documentNode),
                ImageUrl = string.Format(BookUrlPatterns.Cover, bookId)
            };
            
            return bookInfo;
        }
        
        private static string GetBookAuthorFromPage(HtmlNode node) =>
            node.SelectNodes("a")
                .First(a => a.GetAttributeValue("href", string.Empty).Contains("author="))
                .GetAttributeValue("title", string.Empty);

        private static string GetBookInfoNameFromPage(int bookId, HtmlNode node) =>
            node.SelectNodes("a")
                .Where(n => n.GetAttributeValue("href", string.Empty).Contains($"read_book.php?id={bookId}"))
                .Select(a => a.GetAttributeValue("title", string.Empty))
                .First();
    }
}