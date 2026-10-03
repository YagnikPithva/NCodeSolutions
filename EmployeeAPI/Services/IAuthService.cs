using static EmployeeAPI.DTOs.AuthDtos;

namespace EmployeeAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
