using StalKompParser.StalKompParser.Interfaces;

namespace StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories
{
    public class ProductPageFactory : IPageFactory<ProductPage>
    {
        public ProductPage Create(PageCreationContext context)
        {
            return new ProductPage(context);
        }
    }
}
