using System.Collections.Generic;

namespace BetterRead.Shared.Domain.Book
{
    public class Sheet
    {
        public int Id { get; set; }
        public IEnumerable<SheetContent> SheetContents { get; set; }
    }
}