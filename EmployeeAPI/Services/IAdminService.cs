using EmployeeAPI.Common;
using static EmployeeAPI.DTOs.AdminDtos;
namespace EmployeeAPI.Services; 

public interface IAdminService
{

    Task<OrganizationDto?> CreateOrganizationAsync(OrganizationCreateDto dto);  // null => conflict

    Task<List<OrganizationDto>> GetOrganizationsAsync();

    Task<OpResult> CreateEmployeeAccountAsync(EmployeeAccountCreateDto dto);

}
