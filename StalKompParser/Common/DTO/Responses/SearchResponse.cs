using StalKompParser.StalKompParser.Common.DTO;

namespace StalKompParser.StalKompParser.Common.DTO.Responses
{
    public class SearchResponse
    {
        public App App { get; set; } = new();

        public List<Variant> Variants { get; set; } = new();

    }
}
