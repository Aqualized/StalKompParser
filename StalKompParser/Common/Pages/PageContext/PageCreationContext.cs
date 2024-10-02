using AngleSharp.Html.Dom;

namespace StalKompParser.StalKompParser.Common.Pages.PageContext
{
    public class PageCreationContext
    {
        public string? PageHtml { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool? CanLoadAttachments { get; set; }
        public IHtmlDocument? Document { get; set; }
    }
}
