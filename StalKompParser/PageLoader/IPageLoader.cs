namespace StalKompParser.StalKompParser.PageLoader;

public interface IPageLoader
{
    Task<string?> GetPageByPhrase(string phrase, ushort number, CancellationToken token);
    Task<string?> GetPageByLink(string url, CancellationToken token);
}