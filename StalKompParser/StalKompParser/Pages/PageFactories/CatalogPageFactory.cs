using StalKompParser.StalKompParser.Interfaces;

namespace StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories
{
    public class CatalogPageFactory : IPageFactory<CatalogPage>
    {
        public CatalogPage Create(PageCreationContext context)
        {
            return new CatalogPage(context);
        }
    }
}
