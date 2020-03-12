using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BetterRead.Shared.Domain.Search
{
    public class Page
    {
        public int label { get; set; }
        public string start { get; set; }
    }

    public class Cursor
    {
        public int currentPageIndex { get; set; }
        public string estimatedResultCount { get; set; }
        public string moreResultsUrl { get; set; }
        public string resultCount { get; set; }
        public string searchResultTime { get; set; }
        public List<Page> pages { get; set; }
    }

    public class Context
    {
        public string title { get; set; }
        public string total_results { get; set; }
    }

    public class CseImage
    {
        public string src { get; set; }
    }

    public class Metatags
    {
        public string msapplicationTileimage { get; set; }
        public string msapplicationTilecolor { get; set; }
        public string appleMobileWebAppTitle { get; set; }
        public string msapplicationConfig { get; set; }
        public string applicationName { get; set; }
    }

    public class CseThumbnail
    {
        public string src { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class RichSnippet
    {
        public CseImage cseImage { get; set; }
        public Metatags metatags { get; set; }
        public CseThumbnail cseThumbnail { get; set; }
    }

    public class Result
    {
        public string cacheUrl { get; set; }
        public string clicktrackUrl { get; set; }
        public string content { get; set; }
        public string contentNoFormatting { get; set; }
        public string title { get; set; }
        public string titleNoFormatting { get; set; }
        public string formattedUrl { get; set; }
        public string unescapedUrl { get; set; }
        public string url { get; set; }
        public string visibleUrl { get; set; }
        public RichSnippet richSnippet { get; set; }
    }

    public class SearchResult
    {
        public Cursor cursor { get; set; }
        public Context context { get; set; }
        public List<Result> results { get; set; }
    }
}