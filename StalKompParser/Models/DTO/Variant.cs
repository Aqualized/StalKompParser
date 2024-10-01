using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;
using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.DTO
{
    public class Variant
    {
        public string Phrase { get; set; } = string.Empty;
        public List<SearchProduct> Products { get; set; } = [];
    }
}
