using StalKompParser.StalKompParser.Common.Pages.PageContext;

namespace StalKompParser.StalKompParser.Common.PageFactories
{
    public interface IPageFactory<T>
    {
        T Create(PageCreationContext context);
    }
}
