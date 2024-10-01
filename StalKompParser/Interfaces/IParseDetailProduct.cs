using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;

namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IParseDetailProduct: IParseSearchProduct
    {
        List<Property> GetPropeties();

        List<Image> GetImages();

        List<Attachment> GetAttachments();
    }
}
