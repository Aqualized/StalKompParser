using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;
using StalKompParser.StalKompParser.StalKompParser.Pages;
using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;

namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IMainPageFactory
    {
        public Task<AbstractPage<DetailProduct>> CreateProductPage(PageCreationContext context, CancellationToken token);

        public Task<AbstractPage<SearchProduct>> CreateListPage(PageCreationContext context, CancellationToken token);
    }
}
