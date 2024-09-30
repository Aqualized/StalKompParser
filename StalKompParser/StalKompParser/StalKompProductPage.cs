using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using StalKompParser.StalKompParser.Models.DTO.Product.SearchProduct;
using System.ComponentModel;

namespace StalKompParser.StalKompParser.StalKompParser
{
    public class StalKompProductPage
    {
        private readonly IHtmlDocument _document;
        private readonly string _url;
        private StalKompProductPage(IHtmlDocument document, string url)
        {
            _document = document;
            _url = url;
        }

        public static async Task<StalKompProductPage?> TryCreate(string pageHtml, string url, CancellationToken token)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(pageHtml, token);

            return new StalKompProductPage(document, url);
        }
        public SearchProduct ParseProduct()
        {
            var product = _document.QuerySelector(".summary") ??
                throw new ArgumentException("Can't parse this page. Class .summary  was not present in the page");

            var item = new StalKompItem(product, _url);
            return item.TryParse();
        }
    }
}
