using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.HttpModels
{
    public class StalKompRequest
    {
        [Required]
        [MinLength(1)]
        public List<string> SearchPhrases { get; set; } = [];

    }
}
