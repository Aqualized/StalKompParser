using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using Microsoft.Extensions.Options;
using StalKompParser.StalKompParser.Configurations;
using StalKompParser.StalKompParser.Helpers;
using StalKompParser.StalKompParser.Interfaces;
using StalKompParser.StalKompParser.Models.DTO;
using StalKompParser.StalKompParser.Models.DTO.Product.DetailProduct;
using StalKompParser.StalKompParser.Models.DTO.Product.Search;
using StalKompParser.StalKompParser.Models.DTO.Requests;
using StalKompParser.StalKompParser.Models.DTO.Responses;
using StalKompParser.StalKompParser.StalKompParser.Pages;
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;
using System;

namespace StalKompParser.StalKompParser.StalKompParser;

public class ProductParser : IProductParser
{
    private readonly IPageLoader _pageLoader;
    private readonly IOptions<ParserSettings> _parserSettings;
    public ProductParser(IPageLoader pageLoader, IOptions<ParserSettings> parserSettings)
    {
        _pageLoader = pageLoader;
        _parserSettings = parserSettings;
    }
    public async Task<SearchResponse> ParseSearch(SearchRequest request, CancellationToken token)
    {
        List<string> phrases = request.SearchPhraseList;
        ushort pageNumber = 1;

        var variants = await ParallelHelper.RunInParallelWithLimit(phrases, async phrase =>
        {
            return await InternalSearch(phrase, pageNumber, token);
        }, 5, token);

        var app = request.App;
        return new SearchResponse()
        {
            App = app,
            Variants = variants
        };
    }

    public async Task<DetailResponse> ParseDetail(DetailRequest request, CancellationToken token)
    {
        List<string> links = request.ProductLinks;

        var listOfProducts = await ParallelHelper.RunInParallelWithLimit(links, async link =>
        {
            return await InternalDetail(link, (bool)request.CanLoadAttachments, token);
        }, 5, token);

        var app = request.App;
        return new DetailResponse()
        {
            App = app,
            Products = listOfProducts
        };
    }

    private async Task<Variant> InternalSearch(string phrase, ushort pageNumber, CancellationToken token)
    {
        var link = _pageLoader.GetSearchUrl(phrase, pageNumber, token);
        var products = await InternalParse(
            link,
            canLoadAttachments: false,
            PageFactory: new CatalogPageFactory(),
            createEmpty: () => new List<SearchProduct>(),
            token);
        return new Variant()
        {
            Phrase = phrase,
            Products = products
        };
    }

    private async Task<DetailProduct> InternalDetail(string link, bool canLoadAttachments, CancellationToken token)
    {
        return await InternalParse(
            link,
            canLoadAttachments: canLoadAttachments,
            PageFactory: new ProductPageFactory(),
            createEmpty: () => CreateEmptyDetail(link),
            token);
    }

    /// <summary>
    /// Crazy InternalParse
    /// </summary>
    /// <typeparam name="TPageClass">TPageClass - Class that inherits AbstractPage<TPageClass,TParserOutput></typeparam>
    /// <typeparam name="TParserOutput">TParserOutput - output of TPageClass.Parse() method</typeparam>
    /// <param name="link">url to the page</param>
    /// <param name="canLoadAttachments">flag for downloading attachments in case TPageClass supports it</param>
    /// <param name="PageFactory">Factory: IPageFactory<typeparamref name="TPageClass"/></param>
    /// <param name="createEmpty">Method for creating empty TParserOutput for bad returns</param>
    /// <param name="token">token for timemanagemt</param>
    /// <returns>Task<TParserOutput></returns>
    private async Task<TParserOutput> InternalParse<TPageClass,TParserOutput>(
    string link,
    bool canLoadAttachments,
    IPageFactory<TPageClass> PageFactory,
    Func<TParserOutput> createEmpty,
    CancellationToken token)
        where TPageClass : AbstractPage<TPageClass, TParserOutput>
    {
        var pageHtml = await _pageLoader.GetPageByLink(link, token);
        if (string.IsNullOrEmpty(pageHtml))
        {
            return createEmpty();
        }

        var context = new PageCreationContext()
        {
            PageHtml = pageHtml,
            Url = link,
            CanLoadAttachments = canLoadAttachments,
        };

        var page = await AbstractPage<TPageClass, TParserOutput>.TryCreate(context, PageFactory, token); //почему это работает
        // а TPageClass.TryCreate нет? -__- чот под вечер не вдуплил
        if (page == null)
        {
            return createEmpty();
        }

        var product = await page.Parse();
        if (product == null)
        {
            return createEmpty();
        }

        return product;
    }

    private static DetailProduct CreateEmptyDetail(string url)
    {
        return new DetailProduct()
        {
            Link = url
        };
    }
}
