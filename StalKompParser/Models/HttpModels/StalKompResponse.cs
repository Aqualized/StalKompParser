namespace StalKompParser.StalKompParser.Models.HttpModels
{
    public class StalKompResponse
    {
        public string Searched { get; set; } = string.Empty;
        public List<StalKompProduct> Products { get; set; } = [];
    }
}
