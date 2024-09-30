using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.DTO.Requests
{
    public class SearchRequest
    {
        [Required(ErrorMessage = "RequestApp is required.")]
        public App App { get; set; } = new();

        [Required(ErrorMessage = "At least one phrase is required.")]
        [MinLength(1)]
        public List<string> SearchPhrases { get; set; } = [];

        [Range(1, 100, ErrorMessage = "MaxProductsCount must be between 1 and 100.")]
        public uint MaxProductsCount { get; set; } = 15;

        [Range(30, 300, ErrorMessage = "WaitTimeout must be between 30 and 300 seconds.")]
        public uint WaitTimeout { get; set; } = 60;

    }
}
