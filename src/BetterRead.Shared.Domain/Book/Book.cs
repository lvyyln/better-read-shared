﻿using System.Collections.Generic;

namespace BetterRead.Shared.Domain.Book
{
    public class Book
    {
        public int Id => Info.BookId;
        public BookInfo Info { get; set; }
        public IEnumerable<Sheet> Sheets { get; set; }
        public IEnumerable<Content> Contents { get; set; }
        public IEnumerable<Note> Notes { get; set; }
    }
}