using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace QuickType
{
    public partial class SearchResult
    {
        [JsonProperty("cursor")]
        public Cursor Cursor { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("results")]
        public List<Result> SearchResults { get; set; }

        [JsonProperty("findMoreOnGoogle")]
        public FindMoreOnGoogle FindMoreOnGoogle { get; set; }
    }

    public partial class Context
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("total_results")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TotalResults { get; set; }
    }

    public partial class Cursor
    {
        [JsonProperty("currentPageIndex")]
        public long CurrentPageIndex { get; set; }

        [JsonProperty("estimatedResultCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long EstimatedResultCount { get; set; }

        [JsonProperty("moreResultsUrl")]
        public Uri MoreResultsUrl { get; set; }

        [JsonProperty("resultCount")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ResultCount { get; set; }

        [JsonProperty("searchResultTime")]
        public string SearchResultTime { get; set; }

        [JsonProperty("pages")]
        public Page[] Pages { get; set; }
    }

    public partial class Page
    {
        [JsonProperty("label")]
        public long Label { get; set; }

        [JsonProperty("start")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Start { get; set; }
    }

    public partial class FindMoreOnGoogle
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("cacheUrl")]
        public Uri CacheUrl { get; set; }

        [JsonProperty("clicktrackUrl")]
        public Uri ClicktrackUrl { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("contentNoFormatting")]
        public string ContentNoFormatting { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("titleNoFormatting")]
        public string TitleNoFormatting { get; set; }

        [JsonProperty("formattedUrl")]
        public string FormattedUrl { get; set; }

        [JsonProperty("unescapedUrl")]
        public Uri UnescapedUrl { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("visibleUrl")]
        public string VisibleUrl { get; set; }

        [JsonProperty("richSnippet")]
        public RichSnippet RichSnippet { get; set; }
    }

    public partial class RichSnippet
    {
        [JsonProperty("cseImage")]
        public CseImage CseImage { get; set; }

        [JsonProperty("cseThumbnail")]
        public CseThumbnail CseThumbnail { get; set; }
    }

    public partial class CseImage
    {
        [JsonProperty("src")]
        public Uri Src { get; set; }
    }

    public partial class CseThumbnail
    {
        [JsonProperty("src")]
        public Uri Src { get; set; }

        [JsonProperty("width")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Width { get; set; }

        [JsonProperty("height")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Height { get; set; }
    }

    public partial class SearchResult
    {
        public static SearchResult FromJson(string json) => JsonConvert.DeserializeObject<SearchResult>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SearchResult self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
