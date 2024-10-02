using StalKompParser.StalKompParser.Common.Pages;
using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.PageFactories
{
    public class ProductPageFactory : IPageFactory<ProductPage>
    {
        public ProductPage Create(PageCreationContext context)
        {
            return new ProductPage(context);
        }
    }
}
