using AngleSharp.Html.Dom;
using StalKompParser.StalKompParser.Common.DTO.Product.Search;
using StalKompParser.StalKompParser.Common.Pages.Elements;
using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.Pages
{
    public class CatalogPage : AbstractPage<CatalogPage, List<SearchProduct>>
    {
        public CatalogPage(PageCreationContext context)
            : base(context)
        {
        }

        public override async Task<List<SearchProduct>> Parse()
        {
            List<SearchProduct> resultList = [];

            var productsList = _context.Document!.QuerySelector("ul.products");
            if (productsList != null)
            {
                var productItems = productsList.QuerySelectorAll("li.product");
                if (productItems is null || productItems.Length == 0)
                {
                    return [];
                }

                foreach (var item in productItems)
                {
                    var element = new ListPageElement(item);
                    resultList.Add(await element.ParseItem());
                }
                return resultList;

            }
            return [];
        }

    }
}
