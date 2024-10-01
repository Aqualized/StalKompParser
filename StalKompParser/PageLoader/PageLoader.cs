using Microsoft.Extensions.Options;
using StalKompParser.StalKompParser.Configurations;
using StalKompParser.StalKompParser.Interfaces;
using System.Net;

namespace StalKompParser.StalKompParser.PageLoader
{
    public class PageLoader : IPageLoader
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ParserSettings> _parserSettings;

        public PageLoader(IOptions<ParserSettings> parserSettings, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _parserSettings = parserSettings;
        }

        public string? GetSearchUrl(string phrase, ushort number, CancellationToken token)
        {
            var currentUrl = _parserSettings.Value.Url
                                    .Replace("{NUMBER}", number.ToString())
                                    .Replace("{PHRASE}", phrase);
            return currentUrl;
        }

        public string? GetProductUrl(string productTitle, CancellationToken token)
        {
            var currentUrl = _parserSettings.Value.ProductUrl.Replace("{PRODUCT}", productTitle);
            return currentUrl;
        }

        public async Task<string?> GetPageByLink(string url, CancellationToken token)
        {
            var response = await _httpClient.GetAsync(url, token);

            if (response is { StatusCode: HttpStatusCode.OK })
                return await response
                    .Content
                    .ReadAsStringAsync(token);

            return null;
        }
    }
}