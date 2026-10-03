using EmployeeAPI.DTOs;
using EmployeeAPI.Common;

namespace EmployeeAPI.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>> GetAllAsync();
        Task<EmployeeDTO?> GetByIdAsync(int id);
        Task<EmployeeDTO?> CreateAsync(int orgId, EmployeeCreateDto dto);   // null => duplicate email
        Task<OpResult> UpdateAsync(int orgId, int id, EmployeeCreateDto dto);
        Task<OpResult> DeleteAsync(int orgId, int id);
        Task<List<EmployeeDTO>> GetByOrganizationAsync(int organizationId);
        Task<EmployeeDTO?> GetOwnedAsync(int id, int orgId);

    }
}
