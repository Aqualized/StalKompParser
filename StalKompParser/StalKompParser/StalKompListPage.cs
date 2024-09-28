using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace StalKompParser.StalKompParser.StalKompParser
{
    public class StalKompListPage
    {
        private readonly IHtmlDocument _document;
        public readonly ushort PageNumber; //>0  to keep track of pagination (in case) 

        private StalKompListPage(IHtmlDocument document, ushort pageNumber)
        {
            _document = document;
            PageNumber = pageNumber;
        }

        public static async Task<StalKompListPage?> TryCreate(string pageHtml, ushort pageNumber, CancellationToken token)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(pageHtml, token);

            return new StalKompListPage(document, pageNumber);
        }

        public List<string> GetProductLinks()
        {
            var products = _document.QuerySelectorAll(".woocommerce-loop-product__title a") ??
                throw new ArgumentException("Can't parse this page. Class .woocommerce-loop-product__title was not present in the page");

            if (products.Length == 0)
                return new List<string>();

            var links = new List<string>();

            foreach (var product in products)
            {
                var productLink = (product as IHtmlAnchorElement)?.Href;
                if (!string.IsNullOrWhiteSpace(productLink))
                {
                    links.Add(productLink.Trim());
                }
            }

            return links;
        }
    }
}
