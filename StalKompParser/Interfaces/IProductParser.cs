using StalKompParser.StalKompParser.Models.DTO.Requests;
using StalKompParser.StalKompParser.Models.DTO.Responses;

namespace StalKompParser.StalKompParser.Interfaces
{
    public interface IProductParser
    {
        public Task<List<SearchResponse>> ParseSearch(SearchRequest request, CancellationToken token);
        public Task<List<DetailResponse>> ParseDetail(DetailRequest request, CancellationToken token);
    }
}
