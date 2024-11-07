using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using StalKompParser.StalKompParser.Common.PageFactories;
using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.Pages
{
    abstract public class AbstractPage<TThisClass, TParseOutput>
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
