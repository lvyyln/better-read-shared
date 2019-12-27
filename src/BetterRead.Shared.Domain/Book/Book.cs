using System.Collections.Generic;

namespace BetterRead.Shared.Domain.Book
{
    public class Book
    {
        public Book()
        {
            Sheets = new List<Sheet>();
            Contents = new List<Content>();
            Notes = new List<Note>();
        }
        
        public int Id => Info.BookId;
        public BookInfo Info { get; set; }
        public List<Sheet> Sheets { get; set; }
        public List<Content> Contents { get; set; }
        public List<Note> Notes { get; set; }
    }
}