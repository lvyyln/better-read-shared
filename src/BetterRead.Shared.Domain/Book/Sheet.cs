using System.Collections.Generic;

namespace BetterRead.Shared.Domain.Book
{
    public class Sheet
    {
        public Sheet()
        {
            SheetContents = new List<SheetContent>();
        }

        public int Id { get; set; }
        public List<SheetContent> SheetContents { get; set; }
    }
}