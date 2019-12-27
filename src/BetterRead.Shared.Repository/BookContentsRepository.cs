using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;
using HtmlAgilityPack;

namespace BetterRead.Shared.Repository
{
    public class BookContentsRepository : IBookContentsRepository
    {
        public async Task<List<Content>> GetContents(int bookId)
        {
            var url = string.Format(BookUrlPatterns.Contents, bookId);
            var htmlDocument = await new HtmlWeb().LoadFromWebAsync(url);

            return GetContentsFromNode(htmlDocument.DocumentNode);
        }
        
        private static List<Content> GetContentsFromNode(HtmlNode node) =>
            node
                .SelectNodes("#oglav_link > li > a")
                .Select(GetContentFromNode)
                .ToList();

        private static Content GetContentFromNode(HtmlNode node) =>
            new Content
            {
                Link = node.GetAttributeValue("href", string.Empty),
                Text = node.InnerText
            };
    }
}