using Microsoft.AspNetCore.Mvc;
using StalKompParser.StalKompParser.Application.Services.StalKompParser;
using StalKompParser.StalKompParser.Common.DTO.Requests;
using System.Threading;

namespace StalKompParser.StalKompParser.Application.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class StalController : ControllerBase
    {
        private readonly IProductParser _parser;

        public StalController(IProductParser parser)
        {
            _parser = parser;
        }

        [HttpPost("search")]
        public async Task<IActionResult> PostSearch([FromBody] SearchRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _parser.ParseSearch(request, HttpContext.RequestAborted);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request timed out");
            }
        }

        [HttpPost("details")]
        public async Task<IActionResult> PostDetails([FromBody] DetailRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _parser.ParseDetail(request, HttpContext.RequestAborted);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request timed out");
            }
        }
    }

}