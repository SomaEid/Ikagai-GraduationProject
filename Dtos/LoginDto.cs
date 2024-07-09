using Ikagai.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Ikagai.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "NationalId Is Required")]
        [NationalId(ErrorMessage = "InValid NationalId")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
    }
}
