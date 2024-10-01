namespace StalKompParser.StalKompParser.Interfaces;

public interface IPageLoader
{
    string? GetSearchUrl(string phrase, ushort number, CancellationToken token);
    string? GetProductUrl(string productTitle, CancellationToken token);
    Task<string?> GetPageByLink(string url, CancellationToken token);
}