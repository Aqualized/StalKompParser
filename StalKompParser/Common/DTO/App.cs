using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Common.DTO
{
    public class App
    {
        [Required(ErrorMessage = "AppId is required.")]
        public string AppId { get; set; } = string.Empty;

        [Required(ErrorMessage = "AppSecret is required.")]
        public string AppSecret { get; set; } = string.Empty;
    }
}
