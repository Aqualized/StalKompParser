using StalKompParser.StalKompParser.Models.HttpModels;

namespace StalKompParser.StalKompParser.StalKompParser
{
    public interface IProductParser
    {
        public Task<List<StalKompResponse>> Parse(StalKompRequest request, CancellationToken token);
    }
}
