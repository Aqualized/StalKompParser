using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;
using StalKompParser.StalKompParser.StalKompParser.Pages;
using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;

namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IPageFactory<T>
    {
        T Create(PageCreationContext context);
    }
}
