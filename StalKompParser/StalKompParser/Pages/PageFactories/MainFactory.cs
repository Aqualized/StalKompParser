using AngleSharp.Html.Parser;
using Microsoft.OpenApi.Any;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;

namespace StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories
{
    /// <summary>
    /// MainFactory - класс главной фабрики, регистрирующей IPageFactory
    /// </summary>
    public class MainFactory : IMainPageFactory
    {
        

        public async Task<AbstractPage<DetailProduct>> CreateProductPage(PageCreationContext context, CancellationToken token)
        {
            if (context.CanLoadAttachments == null)
                throw new ArgumentNullException("document Matched");
            return new StalKompProductPage(context);
        }

        public async Task<AbstractPage<SearchProduct>> CreateListPage(PageCreationContext context, CancellationToken token)
        {
            //ignores canLoadAttachments
            return new StalKompListPage(context);
        }
    }

}
