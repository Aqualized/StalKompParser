using Microsoft.Extensions.Options;
using StalKompParser.StalKompParser.Configurations;
using StalKompParser.StalKompParser.Helpers;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO.Product.SearchProduct;
using StalKompParser.StalKompParser.Models.DTO.Requests;
using StalKompParser.StalKompParser.Models.DTO.Responses;
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
    public async Task<List<SearchResponse>> ParseSearch(SearchRequest request, CancellationToken token)
    {
       List<string> phrases = request.SearchPhrases;

        var listOfLists = await ParallelHelper.RunInParallelWithLimit(phrases, async phrase =>
        {
            return await InternalParse(phrase, token);
        }, 5, token);

        return listOfLists.SelectMany(responseList => responseList).ToList();
    }

    public Task<List<DetailResponse>> ParseDetail(DetailRequest request, CancellationToken token)
    {
        List<string> phrases = request.ProductLinks;

        var listOfLists = await ParallelHelper.RunInParallelWithLimit(phrases, async phrase =>
        {
            return await InternalParse(phrase, token);
        }, 5, token);

        return listOfLists.SelectMany(responseList => responseList).ToList();
    }

    private async Task<List<SearchResponse>> InternalParse(string phrase, CancellationToken token)
    {
        var responseList = new List<SearchResponse>();
        ushort numberOfPage = 1;
        var catalogString = await _pageLoader.GetPageByPhrase(phrase, numberOfPage, token);
        if (string.IsNullOrEmpty(catalogString))
        {
            responseList.Add(
                new SearchResponse() 
                {
                    Phrase = phrase, 
                    Products = [],
                    ErrorMessage = "Internal error handling catalog page loading"
                }
            );
            return responseList;
        }
        var catalogPage = await StalKompListPage.TryCreate(catalogString, numberOfPage, token);
        if (catalogPage is null)
        {
            responseList.Add(
                new SearchResponse() 
                {
                    Phrase = phrase, 
                    Products = [],
                    ErrorMessage = "Internal error handling catalog page creation"
                }
            );
            return responseList;
        }
        var product = await ProductsParse(phrase, catalogPage, token);
        if (product is null || product.Count == 0)
        {
            responseList.Add(new SearchResponse()
            {
                Phrase = phrase,
                Products = []
            });
            return responseList;
        }

        responseList.Add(new SearchResponse()
        {
            Phrase = phrase,
            Products = product
        });

        return responseList;
    }
    private async Task<List<SearchProduct>> ProductsParse(string phrase, StalKompListPage catalogPage, CancellationToken token)
    {
        var productLinks = catalogPage.GetProductLinks();

        return (await ParallelHelper.RunInParallelWithLimit(productLinks, async link =>
        {
            var productString = await _pageLoader.GetPageByLink(link, token);
            if (productString is null)
                return null;
                
            var productPage = await StalKompProductPage.TryCreate(productString, link, token);
            return productPage?.ParseProduct();
        }, 5, token)).Where(product => product is not null).Cast<SearchProduct>().ToList();
    }
    
}