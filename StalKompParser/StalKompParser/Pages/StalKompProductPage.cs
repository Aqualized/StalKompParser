using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;

namespace StalKompParser.StalKompParser.StalKompParser.Pages
{
    public class StalKompProductPage : AbstractPage<DetailProduct>, IParseDetailProduct
    {
        private readonly IElement _item;
        public StalKompProductPage(PageCreationContext context)
            :base(context)
        {
            var product = _context.Document.QuerySelector(".product-gallery-summary") ??
                throw new ArgumentException("Can't parse this page. Class .product-gallery-summary  was not present in the page");
            _item = product;
        }

        public override async Task<List<DetailProduct>> Parse()
        {
            return new List<DetailProduct>
            {
                new DetailProduct()
                {
                    Code = GetCode(),
                    Name = GetName(),
                    Price = GetPrice(),
                    PriceCurrency = GetPriceCurrency(),
                    QuantityCurrent = GetQuantityCurrent(),
                    QuantityInStock = GetQuantityInStock(),
                    Link = GetLink(),
                    CatalogPath = GetCatalogPath(),
                    Properties = GetPropeties(),
                    Images = GetImages(),
                    Attachments = _context.CanLoadAttachments ? GetAttachments() : []
                }
            };
        }

        public string GetCode()
        {
            var articleElement = _item.QuerySelector("span.sku");
            return articleElement?.TextContent.Trim() ?? string.Empty;
        }

        public string GetName()
        {
            var titleElement = _item.QuerySelector("h1.product_title.entry-title");
            return titleElement?.TextContent.Trim() ?? string.Empty;
        }
        public decimal GetPrice()
        {
            var priceElement = _item.QuerySelector("p.price");
            string? price = priceElement?.TextContent.Trim();
            if (string.IsNullOrEmpty(price))
                return 0;
            return decimal.Parse(price);
        }

        public string GetPriceCurrency()
        {
            return "RUB";
        }

        public decimal? GetQuantityCurrent()
        {
            //сайт какаха не имеет чисел
            //var stockElement = _item.QuerySelector("div.wplb-stock");
            //return stockElement?.TextContent.Trim() ?? string.Empty;
            return null;
        }
        public decimal? GetQuantityInStock()
        {
            //var stockElement = _item.QuerySelector("div.wplb-stock");
            //return stockElement?.TextContent.Trim() ?? string.Empty;
            return null;
        }

        public string GetLink()
        {
            return _context.Url;
        }
        public string? GetCatalogPath()
        {
            return null;
        }

        public List<Property> GetPropeties()
        {
            return [];
        }

        public List<Image> GetImages()
        {
            return [];
        }

        public List<Attachment> GetAttachments() 
        {
            return [];
        }
    }
}
