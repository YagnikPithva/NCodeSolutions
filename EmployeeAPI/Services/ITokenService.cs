using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{

    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) Create(User user);
    }
}
