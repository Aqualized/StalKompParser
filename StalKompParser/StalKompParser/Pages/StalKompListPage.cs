using AngleSharp.Html.Dom;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;
using StalKompParser.StalKompParser.StalKompParser.Pages.Elements;
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;

namespace StalKompParser.StalKompParser.StalKompParser.Pages
{
    public class StalKompListPage : AbstractPage<SearchProduct>
    {
        public StalKompListPage(PageCreationContext context)
            : base(context)
        { 
        }

        public override async Task<List<SearchProduct>> Parse()
        {
            List<SearchProduct> resultList = [];

            var productsList = _context.Document.QuerySelector("ul.products");
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
