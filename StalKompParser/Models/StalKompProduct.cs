namespace StalKompParser.StalKompParser.Models
{
    public class StalKompProduct
    {
        public string Url { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string Stock { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Article { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = [];
    }
}
