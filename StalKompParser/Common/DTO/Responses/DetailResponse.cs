using StalKompParser.StalKompParser.Common.DTO;
using StalKompParser.StalKompParser.Common.DTO.Product.DetailProduct;

namespace StalKompParser.StalKompParser.Common.DTO.Responses
{
    public class DetailResponse
    {
        public App App { get; set; } = new();
        public List<DetailProduct> Products { get; set; } = [];
    }
}
