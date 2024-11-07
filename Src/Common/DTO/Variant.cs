using StalKompParser.StalKompParser.Common.DTO.Product.Search;

namespace StalKompParser.StalKompParser.Common.DTO
{
    public class Variant
    {
        public string Phrase { get; set; } = string.Empty;
        public List<SearchProduct> Products { get; set; } = [];
    }
}
