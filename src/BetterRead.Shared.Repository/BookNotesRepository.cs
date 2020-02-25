﻿using System.Collections.Generic;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterRead.Shared.Common.Constants;
using BetterRead.Shared.Domain.Book;
using BetterRead.Shared.Repository.Abstractions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

 namespace BetterRead.Shared.Repository
 {
     public class BookNotesRepository : IBookNotesRepository
     {
         private readonly HtmlWeb _htmlWeb;

         public BookNotesRepository()
         {
             _htmlWeb = new HtmlWeb {OverrideEncoding = Encoding.GetEncoding("windows-1251")};
         }

         public async Task<IEnumerable<Note>> GetNotesAsync(int bookId)
         {
             var url = string.Format(BookUrlPatterns.Notes, bookId);

             var htmlDocument = await _htmlWeb.LoadFromWebAsync(url);
             var documentNode = htmlDocument.DocumentNode.QuerySelectorAll("td.MsoNormal").FirstOrDefault()?.ChildNodes;
             
             var temp = documentNode
                 .SplitOn(node => node.Name == "a")
                 .ToList()
                 .Select(g => g.Where(node => !(node is HtmlTextNode)))
                 .Where(g => g.Any());
             var notes = temp.Select(note => ConvertNote(note));

             return notes;
         }

         public Note ConvertNote(IEnumerable<HtmlNode> note) =>
             new Note()
             {
                 Id = Convert.ToInt32(note.FirstOrDefault()?.InnerText),
                 Contents = note.Select(nt => nt.InnerText).ToList()
             };
     }

     public static class Spliter
     {
         public static IEnumerable<IEnumerable<T>> SplitOn<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
             source.Aggregate(new List<List<T>> {new List<T>()},
                 (list, value) =>
                 {
                     if (predicate(value))
                         list.Add(new List<T>());
                     else
                         list.Last().Add(value);
                     return list;
                 });
     }
 }