namespace StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct
{
    public class DetailProduct : SearchProduct
    {
        public Properties Properties { get; set; } = new();
        public Images Images { get; set; } = new();
        public Attachments Attachments { get; set; } = new();
    }
}
