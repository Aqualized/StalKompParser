using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;

namespace StalKompParser.StalKompParser.Models.DTO.Responses
{
    public class DetailResponse
    {
        public App App { get; set; } = new();
        public List<DetailProduct> Products { get; set; } = [];
    }
}
