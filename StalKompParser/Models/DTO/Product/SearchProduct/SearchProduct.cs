using Microsoft.AspNetCore.Mvc.ViewComponents;
using StalKompParser.StalKompParser.Interfaces;

namespace StalKompParser.StalKompParser.Models.DTO.Product.SearchProduct
{
    public class SearchProduct
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public string PriceCurrency { get; set; } = string.Empty;
        public decimal? QuantityCurrent { get; set; }
        public decimal? QuantityInStock { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? CatalogPath { get; set; }
    }
}
