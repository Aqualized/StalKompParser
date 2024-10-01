using System.ComponentModel.DataAnnotations;

namespace StalKompParser.StalKompParser.Models.DTO.Requests
{
    public class DetailRequest
    {

        [Required(ErrorMessage = "RequestApp is required.")]
        public App App { get; set; }

        [Required(ErrorMessage = "CanLoadAttachments is required.")] //не работает просто на bool
        public bool CanLoadAttachments { get; set; }

        [Required(ErrorMessage = "ProductLinks is required.")]
        public List<string> ProductLinks { get; set; }
    }
}
