using EmployeeAPI.Data;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeAPI.DTOs.AdminDtos;
using EmployeeAPI.Common;
namespace EmployeeAPI.Services;

public class AdminService : IAdminService
{

    private readonly AppDbContext _appDbContext;
    private readonly ITokenService _tokenService;

    public AdminService(AppDbContext appDbContext, ITokenService tokenService)
    {
        _appDbContext = appDbContext;
        _tokenService = tokenService;
    }


    public async Task<OrganizationDto?> CreateOrganizationAsync(OrganizationCreateDto dto)
    {
        if (await _appDbContext.Organizations.AnyAsync(o => o.Name == dto.Name) || await _appDbContext.Users.AnyAsync(u => u.Username == dto.Username))
            return null;

        var org = new Organization { Name = dto.Name };
        _appDbContext.Organizations.Add(org);
        _appDbContext.Users.Add(new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = Roles.Organization,
            Organization = org
        });
        await _appDbContext.SaveChangesAsync();

        return new OrganizationDto(org.Id, org.Name, 0);
    }

    public Task<List<OrganizationDto>> GetOrganizationsAsync() =>
        _appDbContext.Organizations.AsNoTracking()
            .Select(o => new OrganizationDto(o.Id, o.Name, o.Employees.Count))
            .ToListAsync();

    public async Task<OpResult> CreateEmployeeAccountAsync(EmployeeAccountCreateDto dto)
    {
        if (!await _appDbContext.Employees.AnyAsync(e => e.Id == dto.EmployeeId))
            return OpResult.NotFound;

        if (await _appDbContext.Users.AnyAsync(u => u.EmployeeId == dto.EmployeeId || u.Username == dto.Username))
            return OpResult.Conflict;

        _appDbContext.Users.Add(new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = Roles.Employee,
            EmployeeId = dto.EmployeeId
        });
        await _appDbContext.SaveChangesAsync();
        return OpResult.Success;
    }

}
