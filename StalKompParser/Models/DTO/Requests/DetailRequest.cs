using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.DTO.Requests
{
    public class DetailRequest
    {

        [Required(ErrorMessage = "RequestApp is required.")]
        public App App { get; set; } = new();

        [Required(ErrorMessage = "CanLoadAttachments is required.")]
        public bool CanLoadAttachments { get; set; } = false;

        [Required(ErrorMessage = "ProductLinks is required.")]
        public List<string> ProductLinks { get; set; } = [];
    }
}
