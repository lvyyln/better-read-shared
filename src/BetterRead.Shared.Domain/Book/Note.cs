using System.Collections.Generic;

namespace BetterRead.Shared.Domain.Book
{
    public class Note
    {
        public int Id { get; set; }
        public List<string> Contents { get; set; }
    }
}