namespace BetterRead.Shared.Domain.Author
{
    public class AuthorSeries
    {
        public string CollectionName { get; set; }
        public Book.Book[] CollectionBooks { get; set; }
    }
}