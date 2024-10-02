using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using StalKompParser.StalKompParser.Common.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.Common.Pages.Interfaces;
using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.Pages
{
    public class ProductPage : AbstractPage<ProductPage, DetailProduct>, IParseDetailProduct
    {
        private readonly IElement _item;
        public ProductPage(PageCreationContext context)
            : base(context)
        {
            var product = _context.Document!.QuerySelector(".product-gallery-summary") ??
                throw new ArgumentException("Can't parse this page. Class .product-gallery-summary  was not present in the page");
            if (_context.CanLoadAttachments is null)
                throw new NullReferenceException("CanLoadAttachments was null");
            _item = product;
        }

        public override async Task<DetailProduct> Parse()
        {
            return new DetailProduct()
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
                Attachments = (bool)_context.CanLoadAttachments ? GetAttachments() : []
                //_context.CanLoadAttachments is always not null in this state, because of constructor
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
