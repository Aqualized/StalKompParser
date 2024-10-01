using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;

namespace StalKompParser.StalKompParser.StalKompParser.Pages
{
    abstract public class AbstractPage<TThisClass,TParseOutput>
    {
        protected readonly PageCreationContext _context;
        public AbstractPage(PageCreationContext context)
        {
            _context = context;
        }
        public static async Task<TThisClass> TryCreate(PageCreationContext context, IPageFactory<TThisClass> pageFabric, CancellationToken token)
        {
            if (context.PageHtml is null)
                return default;

            var htmlParser = new HtmlParser();
            var document = await htmlParser.ParseDocumentAsync(context.PageHtml);
            if (document is null)
                return default;

            context.Document = document;
            return pageFabric.Create(context);
        }
        public abstract Task<TParseOutput> Parse();
    }
}
