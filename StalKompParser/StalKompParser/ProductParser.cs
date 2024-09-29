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

        var listOfLists = await RunInParallelWithLimit(phrases, async phrase =>
        {
            return await ParsePhrase(phrase, token);
        }, 5, token);
    
        return listOfLists.SelectMany(responseList => responseList).ToList();
    }

    private async Task<List<StalKompResponse>> ParsePhrase(string phrase, CancellationToken token)
    {
        var responseList = new List<StalKompResponse>();
        ushort numberOfPage = 1;
        var catalogString = await _pageLoader.GetPageByPhrase(phrase, numberOfPage, token);
        if (string.IsNullOrEmpty(catalogString))
        {
            responseList.Add(
                new StalKompResponse() 
                {
                    Searched = phrase, 
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
                new StalKompResponse() 
                {
                    Searched = phrase, 
                    Products = [],
                    ErrorMessage = "Internal error handling catalog page creation"
                }
            );
            return responseList;
        }
        var product = await ProductsParse(phrase, catalogPage, token);
        if (product is null || product.Count == 0)
        {
            responseList.Add(new StalKompResponse()
            {
                Searched = phrase,
                Products = []
            });
            return responseList;
        }

        responseList.Add(new StalKompResponse()
        {
            Searched = phrase,
            Products = product
        });

        return responseList;
    }
    private async Task<List<StalKompProduct>> ProductsParse(string phrase, StalKompListPage catalogPage, CancellationToken token)
    {
        var productLinks = catalogPage.GetProductLinks();

        return (await RunInParallelWithLimit(productLinks, async link =>
        {
            var productString = await _pageLoader.GetPageByLink(link, token);
            if (productString is null)
                return null;
                
            var productPage = await StalKompProductPage.TryCreate(productString, link, token);
            return productPage?.ParseProduct();
        }, 5, token)).Where(product => product is not null).Cast<StalKompProduct>().ToList();
    }
    private async Task<List<TResult>> RunInParallelWithLimit<TItem, TResult>(
        IEnumerable<TItem> items, 
        Func<TItem, Task<TResult>> taskFactory, 
        int maxParallelism, 
        CancellationToken token)
    {
        var semaphore = new SemaphoreSlim(maxParallelism);

        var tasks = items.Select(async item =>
        {
            await semaphore.WaitAsync(token);
            try
            {
                return await taskFactory(item);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToList();

        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }
}