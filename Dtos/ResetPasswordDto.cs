using System.ComponentModel.DataAnnotations;

namespace Ikagai.Dtos
{
    public class ResetPasswordDto
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
