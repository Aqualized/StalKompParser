using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.SearchProduct;
using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.DTO
{
    public class Variants
    {
        public string Phrase { get; set; } = string.Empty;
        public List<SearchProduct> Products { get; set; } = [];
    }
}
