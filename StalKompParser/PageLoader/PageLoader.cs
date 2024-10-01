using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using StalKompParser.StalKompParser.Configurations;
using StalKompParser.StalKompParser.Interfaces;
using System.Net;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            // number 0 == 1 на странице сталь комплекта, поэтому решил ограничить на всякий от дублежа данных
            // тупо забил тут думать и решил просто так оставить
            if (number == 0)
                return null;

            var currentUrl = _parserSettings.Value.Url
                                    .Replace("{NUMBER}", number.ToString())
                                    .Replace("{PHRASE}", phrase);


            return currentUrl;
        }

        public string? GetProductUrl(string productTitle, CancellationToken token)
        {
            var currentUrl = _parserSettings.Value.ProductUrl
                                    .Replace("{PRODUCT}", productTitle);
            return currentUrl;
        }

        public async Task<string?> GetPageByLink(string url, CancellationToken token)
        {

            // add user-agent header
            //_httpClient.DefaultRequestHeaders.Add("User-Agent", _parserSettings.Value.UserAgent);

            var response = await _httpClient.GetAsync(url, token);

            // get html as string 
            if (response is { StatusCode: HttpStatusCode.OK })
                return await response
                    .Content
                    .ReadAsStringAsync(token);

            return null;
        }
    }
}