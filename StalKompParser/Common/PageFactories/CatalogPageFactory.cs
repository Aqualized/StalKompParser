using StalKompParser.StalKompParser.Common.Pages;
using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.PageFactories
{
    public class CatalogPageFactory : IPageFactory<CatalogPage>
    {
        public CatalogPage Create(PageCreationContext context)
        {
            return new CatalogPage(context);
        }
    }
}
