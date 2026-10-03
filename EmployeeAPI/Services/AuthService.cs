using EmployeeAPI.Data;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeAPI.DTOs.AuthDtos;

namespace EmployeeAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITokenService _tokenService;


        public AuthService(AppDbContext appDbContext, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _appDbContext = appDbContext;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var (token, expires) = _tokenService.Create(user);
            return new AuthResponseDto(token, user.Username, user.Role, expires);
        }

    }
}
