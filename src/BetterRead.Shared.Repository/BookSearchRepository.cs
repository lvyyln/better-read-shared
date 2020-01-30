using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace BetterRead.Shared.Repository
{
    public class BookSearchRepository : IBookSearchRepository
    {
        private readonly HtmlWeb _htmlWeb;

        public BookSearchRepository()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _htmlWeb = new HtmlWeb { OverrideEncoding = Encoding.GetEncoding("windows-1251") };
        }

        public async Task<IEnumerable<BookInfo>> GetSearchBooksByName(string name)
        {
            var uri = new Uri(BookUrlPatterns.Search);
            var node = await GetNode(uri, name);
            var books = GetBooks(node);

            return books;
        }

        private async Task<HtmlNode> GetNode(Uri uri, string name)
        {
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var query = QueryHelpers.ParseQuery(uri.Query);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
            var qb = new QueryBuilder(items)
            {
                { "search", GetEncoding(name)}
            };
            var searchQuery = baseUri + qb.ToQueryString();
            var htmlDocument = await _htmlWeb.LoadFromWebAsync(searchQuery);
            var documentNode = htmlDocument.DocumentNode;

            return documentNode;
        }

        private static IEnumerable<BookInfo> GetBooks(HtmlNode node) =>
            node
                .QuerySelectorAll("ul.let_ul")
                .First()
                .QuerySelectorAll("li[style]")
                .Select(GetContentFromNode)
                .ToList();

        private static BookInfo GetContentFromNode(HtmlNode node) =>
            new BookInfo
            {
                Name = GetBookName(node),
                Url = GetBookUrl(node),
                Author = GetBookAuthor(node),
                BookId = GetBookId(GetBookUrl(node))
            };

        private static string GetBookName(HtmlNode node) =>
            node.QuerySelectorAll("a").ElementAt(0).InnerText.Trim();

        private static string GetBookUrl(HtmlNode node) =>
            $"{BookUrlPatterns.BaseUrl}" +
            $"{node.QuerySelectorAll("a").ElementAt(0).GetAttributeValue("href", string.Empty)}";

        private static string GetBookAuthor(HtmlNode node) =>
            node.QuerySelectorAll("a").ElementAt(1).InnerText.Trim();
       
        private static int GetBookId(string url)
        {
            var uri = new Uri(url);
            var queryId = HttpUtility.ParseQueryString(uri.Query).Get("id");

            return int.Parse(queryId);
        }

        private string GetEncoding(string name)=>
            HttpUtility.UrlEncode(name, Encoding.GetEncoding(1251));

    }
}
