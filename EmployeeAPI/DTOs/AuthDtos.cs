using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.DTOs
{
    public static class AuthDtos
    {

        public class LoginDto
        {
            [Required] public string Username { get; set; } = string.Empty;
            [Required] public string Password { get; set; } = string.Empty;
        }

        public record AuthResponseDto(string Token, string Username, string Role, DateTime ExpiresAt);
    }
}
