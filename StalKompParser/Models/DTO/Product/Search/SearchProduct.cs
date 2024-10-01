using Microsoft.AspNetCore.Mvc.ViewComponents;
using StalKompParser.StalKompParser.Interfaces;

namespace StalKompParser.StalKompParser.Models.DTO.Product.Search
{
    public class SearchProduct: IProduct
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string PriceCurrency { get; set; } = string.Empty;
        public decimal? QuantityCurrent { get; set; }
        public decimal? QuantityInStock { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? CatalogPath { get; set; }
    }
}
