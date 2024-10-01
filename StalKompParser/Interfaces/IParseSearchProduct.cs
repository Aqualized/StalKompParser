namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IParseSearchProduct
    {
        string GetCode();
        string GetName();
        decimal GetPrice();
        string GetPriceCurrency();
        decimal? GetQuantityCurrent();
        decimal? GetQuantityInStock();
        string GetLink();
        string? GetCatalogPath();
    }
}
