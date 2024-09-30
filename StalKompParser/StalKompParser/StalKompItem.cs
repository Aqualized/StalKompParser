using AngleSharp.Dom;
using StalKompParser.StalKompParser.Models.DTO.Product.SearchProduct;
using System;


namespace StalKompParser.StalKompParser.StalKompParser
{
    public class StalKompItem
    {
        private readonly IElement _item;
        private readonly string _url;

        public StalKompItem(IElement item, string url)
        {
            _item = item;
            _url = url;
        }

        public SearchProduct TryParse()
        {
            return new SearchProduct()
            {
                Url = _url,
                ProductTitle = GetProductTitle(),
                Stock = GetStock(),
                Price = GetPrice(),
                Article = GetArticle(),
                Categories = GetCategories(),
            };

        }
        private string GetProductTitle()
        {
            var titleElement = _item.QuerySelector("h1.product_title.entry-title");
            return titleElement?.TextContent.Trim() ?? string.Empty;
        }

        private string GetStock()
        {
            var stockElement = _item.QuerySelector("div.wplb-stock");
            return stockElement?.TextContent.Trim() ?? string.Empty;
        }

        private string GetPrice()
        {
            var priceElement = _item.QuerySelector("p.price");
            return priceElement?.TextContent.Trim() ?? string.Empty;
        }

        private string GetArticle()
        {
            var articleElement = _item.QuerySelector("span.sku");
            return articleElement?.TextContent.Trim() ?? string.Empty;
        }

        private List<string> GetCategories()
        {
            var categoriesElement = _item.QuerySelector(".posted_in");

            if(categoriesElement == null) 
                return [];

            var categoryLinks = categoriesElement.QuerySelectorAll("a");
            
            var categories = new List<string>();
            foreach (var link in categoryLinks)
            {
                categories.Add(link.TextContent.Trim());
            }
            
            return categories;
        }
    

    }
}
