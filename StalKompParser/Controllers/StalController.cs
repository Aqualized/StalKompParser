using Microsoft.AspNetCore.Mvc;
using StalKompParser.StalKompParser.Models.HttpModels;
using StalKompParser.StalKompParser.StalKompParser;

namespace StalKompParser.StalKompParser.Controllers
{
    [Route("api/StalKomp/products")]
    [ApiController]
    public class StalController : ControllerBase
    {
        private readonly IProductParser _parser;

        public StalController(IProductParser parser)
        {
            _parser = parser;
        }

        [HttpPost("parse")]
        public async Task<IActionResult> GetCards([FromBody] StalKompRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _parser.Parse(request, HttpContext.RequestAborted);
            return Ok(response);
        }
    }

}