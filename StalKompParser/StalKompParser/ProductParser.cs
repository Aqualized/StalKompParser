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
using StalKompParser.StalKompParser.StalKompParser.Pages.PageFactories;
using System;

namespace StalKompParser.StalKompParser.StalKompParser;

public class ProductParser: IProductParser
{
    private readonly IPageLoader _pageLoader;
    private readonly IOptions<ParserSettings> _parserSettings;
    private readonly IMainPageFactory _mainFactory;

    public ProductParser(IPageLoader pageLoader, IOptions<ParserSettings> parserSettings, IMainPageFactory factory)
    {
        _pageLoader = pageLoader;
        _parserSettings = parserSettings;
        _mainFactory = factory;
    }
    public async Task<SearchResponse> ParseSearch(SearchRequest request, CancellationToken token)
    {
        List<string> phrases = request.SearchPhraseList;
        ushort pageNumber = 1;

        //Берет распаллерует и возвращает лист variants, сопоставив ссылку с ответом
        var variants = await ParallelHelper.RunInParallelWithLimit(phrases, async phrase =>
        {
           return await InternalSearch(phrase, pageNumber, token);
        }, 5, token);

        var app = request.App;//Беру апп чонить делаю и отдаю
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
            return await InternalDetail(link,(bool)request.CanLoadAttachments, token);
        }, 5, token);

        var app = request.App;//Беру апп чонить делаю и отдаю
        return new DetailResponse() 
        { 
            App = app,
            Products = listOfProducts
        };
    }

    /// <summary>
    /// Метод парсящий страницу по ссылке и возвращающий лист Toutput рассчитанный на (SearchProduct || DetailProduct)
    /// в случае неудачи возвращает пустой лист или ошибку при неправильном Toutput
    /// </summary>
    /// <param name="url"> ссылка на страницу</param>
    /// <param name="token"> токен отмены для назначения времени ожидания</param>
    /// <returns>Toutput product</returns>

    private async Task<Variant> InternalSearch(string phrase, ushort pageNumber, CancellationToken token)
    {
        var link = _pageLoader.GetSearchUrl(phrase, pageNumber, token);

        var pageHtml = await _pageLoader.GetPageByLink(link, token);
        if (string.IsNullOrEmpty(pageHtml))
        {
            return CreateEmptyVariant(phrase);
        }

        var parser = new HtmlParser();
        var document = parser.ParseDocument(pageHtml);
        var context = new PageCreationContext()
        {
            Document = document,
            Url = link,
        };
        var page = await _mainFactory.CreateListPage(context, token);
        if (page is null)
        {
            return CreateEmptyVariant(phrase);
        }

        var product = await page.Parse();
        if (product is null)
        {
            return CreateEmptyVariant(phrase);
        }

        return new Variant()
        {
            Phrase = phrase,
            Products = product
        };
    }
    private static Variant CreateEmptyVariant(string phrase)
    {
        return new Variant()
        {
            Phrase = phrase,
            Products = []
        };
    }
    private async Task<DetailProduct> InternalDetail(string link, bool canLoadAttachments, CancellationToken token)
    {
        var pageHtml = await _pageLoader.GetPageByLink(link, token);
        if (string.IsNullOrEmpty(pageHtml))
        {
            return CreateEmptyDetail(link);
        }

        var parser = new HtmlParser();
        var document = parser.ParseDocument(pageHtml);
        var context = new PageCreationContext()
        {
            Document = document,
            Url = link,
            CanLoadAttachments = canLoadAttachments
        };

        var page = await _mainFactory.CreateProductPage(context, token);
        if (page is null)
        {
            return CreateEmptyDetail(link);
        }

        var product = await page.Parse();
        if (product is null)
        {
            return CreateEmptyDetail(link);
        }

        var resList = product.Select(product => product).ToList();
        return resList.First();
    }

    private static DetailProduct CreateEmptyDetail(string url)
    {
        return new DetailProduct()
        {
            Link = url,
        };
    }
}