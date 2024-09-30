namespace StalKompParser.StalKompParser.Models.DTO.Responses
{
    public class SearchResponse
    {
        public App App { get; set; } = new();

        public Variants Variants { get; set; } = new();

    }
}
