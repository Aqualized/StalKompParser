using StalKompParser.StalKompParser.Models.DTO.Product.Search;

namespace StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct
{
    public class DetailProduct : SearchProduct
    {
        public List<Property> Properties { get; set; } = [];
        public List<Image> Images { get; set; } = [];
        public List<Attachment> Attachments { get; set; } = [];
    }
}
