using Microsoft.Extensions.Options;
using StalKompParser.StalKompParser.Configurations;
using StalKompParser.StalKompParser.Models;
using StalKompParser.StalKompParser.Models.HttpModels;
using StalKompParser.StalKompParser.PageLoader;

namespace StalKompParser.StalKompParser.StalKompParser;

public class ProductParser: IProductParser
{
    private readonly IPageLoader _pageLoader;
    private readonly IOptions<ParserSettings> _parserSettings;

    public ProductParser(IPageLoader pageLoader, IOptions<ParserSettings> parserSettings)
    {
        _pageLoader = pageLoader;
        _parserSettings = parserSettings;
    }
    public async Task<List<StalKompResponse>> Parse(StalKompRequest request, CancellationToken token)
    {
        List<string> phrases = request.SearchPhrases;

        var responseList = new List<StalKompResponse>();

        foreach (var phrase in phrases)
        {
            var product = await ProductsParse(phrase, token);
            if (product is null)
            {
                responseList.Add(
                    new StalKompResponse() 
                    {
                        Searched = phrase, 
                        Products = [] 
                    }
                );
                continue;
            }
            responseList.Add(
                new StalKompResponse()
                {
                    Searched = phrase,
                    Products = product
                }
            );
        }
        return responseList;
    }
    private async Task<List<StalKompProduct>> ProductsParse(string phrase, CancellationToken token)
    {
        ushort numberOfPage = 1;
        var catalogString = await _pageLoader.GetPageByPhrase(phrase, numberOfPage, token);
        if (string.IsNullOrEmpty(catalogString))
            return null!;

        var catalogPage = await StalKompListPage.TryCreate(catalogString, numberOfPage, token);
        if (catalogPage is null)
            return null!;

        var productsList = new List<StalKompProduct>();

        var product_links = catalogPage.GetProductLinks();
        foreach (var link in product_links)
        {
            var productString = await _pageLoader.GetPageByLink(link, token);
            if (productString is null) 
                continue;

            var productPage = await StalKompProductPage.TryCreate(productString, link, token);
            if (productPage is null)
                return null!;

            var product = productPage.ParseProduct();
            if (product is not null)
                productsList.Add(product);
        }

        return productsList;
    }

}