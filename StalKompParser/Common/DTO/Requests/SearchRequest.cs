using System.ComponentModel.DataAnnotations;
using StalKompParser.StalKompParser.Common.DTO;

namespace StalKompParser.StalKompParser.Common.DTO.Requests
{
    public class SearchRequest
    {
        [Required(ErrorMessage = "RequestApp is required.")]
        public App App { get; set; }

        [Required(ErrorMessage = "At least one phrase is required.")]
        [MinLength(1)]
        public List<string> SearchPhraseList { get; set; }

        [Range(1, 100, ErrorMessage = "MaxProductsCount must be between 1 and 100.")]
        public uint MaxProductsCount { get; set; } = 15;

        [Range(30, 300, ErrorMessage = "WaitTimeout must be between 30 and 300 seconds.")]
        public uint WaitTimeout { get; set; } = 60;

    }
}
