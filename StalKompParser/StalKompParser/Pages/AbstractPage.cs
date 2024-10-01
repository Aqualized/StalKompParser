using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;

namespace StalKompParser.StalKompParser.StalKompParser.Pages
{
    abstract public class AbstractPage<T> where T : IProduct
    {
        protected readonly PageCreationContext _context;
        public AbstractPage(PageCreationContext context)
        {
            _context = context;
        }
        public abstract Task<List<T>> Parse();
        //public static async Task<AbstractPage?> TryCreate(string pageHtml, CancellationToken token)
        //{

        //    var parser = new HtmlParser();
        //    var document = await parser.ParseDocumentAsync(pageHtml, token);

        //    return new AbstractPage(document);//тут фабрика работает и должна выдасть определенный клласс?
        //}


    }
}
