namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IProduct
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public decimal? QuantityCurrent { get; set; }
        public decimal? QuantityInStock { get; set; }
        public string Link { get; set; }
        public string? CatalogPath { get; set; }
    }
}
