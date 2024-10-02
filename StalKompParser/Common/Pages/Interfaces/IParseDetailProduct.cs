using StalKompParser.StalKompParser.Common.DTO.Product.DetailProduct;

namespace StalKompParser.StalKompParser.Common.Pages.Interfaces
{
    public interface IParseDetailProduct : IParseSearchProduct
    {
        List<Property> GetPropeties();

        List<Image> GetImages();

        List<Attachment> GetAttachments();
    }
}
