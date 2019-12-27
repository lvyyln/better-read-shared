namespace BetterRead.Shared.Domain.Book
{
    public class BookInfo
    {
        public BookInfo() { }
        
        public BookInfo(int bookId, string name, string author, string url, string imageUrl)
        {
            BookId = bookId;
            Name = name;
            Author = author;
            Url = url;
            ImageUrl = imageUrl;
        }

        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}