using System.ComponentModel.DataAnnotations;

namespace api_QLHH.Core.DTOs.Responses
{
    public class AccountResponseDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";    }
}
