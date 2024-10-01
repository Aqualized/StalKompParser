using AngleSharp.Dom;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;

namespace StalKompParser.StalKompParser.StalKompParser.Pages.Elements
{
    public class ListPageElement : IParseSearchProduct
    {
        private readonly IElement _item;

        public ListPageElement(IElement item)
        {
            _item = item;
        }

        public async Task<SearchProduct> ParseItem()
        {
            return new SearchProduct()
            {
                Code = GetCode(),
                Name = GetName(),
                Price = GetPrice(),
                PriceCurrency = GetPriceCurrency(),
                QuantityCurrent = GetQuantityCurrent(),
                QuantityInStock = GetQuantityInStock(),
                Link = GetLink(),
                CatalogPath = GetCatalogPath()
            };
        }

        public string GetCode()
        {
            var articleElement = _item.QuerySelector("span.sku");
            return articleElement?.TextContent.Trim() ?? string.Empty;
        }

        public string GetName()
        {
            var titleElement = _item.QuerySelector("h2.woocommerce-loop-product__title");
            return titleElement?.TextContent.Trim() ?? string.Empty;
        }
        public decimal GetPrice()
        {
            return 0;
        }

        public string GetPriceCurrency()
        {
            return "RUB";
        }

        public decimal? GetQuantityCurrent()
        {
            return null;
        }
        public decimal? GetQuantityInStock()
        {
            return null;
        }

        public string GetLink()
        {
            var url = _item.QuerySelector("a.woocommerce-LoopProduct-link")?.GetAttribute("href");
            return url;
        }
        public string? GetCatalogPath()
        {
            return null;
        }
    }
}
