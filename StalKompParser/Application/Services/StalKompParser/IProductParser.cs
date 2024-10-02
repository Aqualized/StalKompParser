using StalKompParser.StalKompParser.Common.DTO.Requests;
using StalKompParser.StalKompParser.Common.DTO.Responses;

namespace StalKompParser.StalKompParser.Application.Services.StalKompParser
{
    public interface IProductParser
    {
        public Task<SearchResponse> ParseSearch(SearchRequest request, CancellationToken token);
        public Task<DetailResponse> ParseDetail(DetailRequest request, CancellationToken token);
    }
}
