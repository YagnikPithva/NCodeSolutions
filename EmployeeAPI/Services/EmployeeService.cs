using EmployeeAPI.Common;
using EmployeeAPI.Data;
using EmployeeAPI.DTOs;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private static EmployeeDTO ToDto(Employee e) =>
            new(e.Id, e.FullName, e.Age, e.Email, e.MobileNumber, e.Address, e.Designation, e.Salary, e.OrganizationId, e.Organization?.Name ?? "");

        public async Task<EmployeeDTO?> GetByIdAsync(int id)
        {
            var e = await _appDbContext.Employees.AsNoTracking()
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(x => x.Id == id);

            return e is null ? null : ToDto(e);
        }

        public async Task<List<EmployeeDTO>> GetAllAsync()
        {
            var list = await _appDbContext.Employees.AsNoTracking()
                .Include(e => e.Organization)
                .ToListAsync();

            return list.Select(ToDto).ToList();
        }

        public async Task<EmployeeDTO?> GetEmployeeById(int id)
        {
            var e = await _appDbContext.Employees.AsNoTracking()
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(x => x.Id == id);

            return e is null ? null : ToDto(e);
        }

        public async Task<EmployeeDTO?> CreateAsync(int orgId, EmployeeCreateDto dto)
        {
            if (await _appDbContext.Employees.AnyAsync(x => x.Email == dto.Email)) return null;

            var e = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Age = dto.Age,
                MobileNumber = dto.MobileNumber,
                Address = dto.Address,
                Designation = dto.Designation,
                Salary = dto.Salary,
                OrganizationId = orgId
            };
            _appDbContext.Employees.Add(e);
            await _appDbContext.SaveChangesAsync();
            return await GetByIdAsync(e.Id);
        }

        public async Task<OpResult> UpdateAsync(int orgId, int id, EmployeeCreateDto dto)
        {
            var emp = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == orgId);
            if (emp is null) return OpResult.NotFound;

            if (await _appDbContext.Employees.AnyAsync(e => e.Email == dto.Email && e.Id != id))
                return OpResult.Conflict;

            emp.FullName = dto.FullName;
            emp.Email = dto.Email;
            emp.Age = dto.Age;
            emp.MobileNumber = dto.MobileNumber;
            emp.Address = dto.Address;
            emp.Designation = dto.Designation;
            emp.Salary = dto.Salary;
            await _appDbContext.SaveChangesAsync();
            return OpResult.Success;
        }

        public async Task<OpResult> DeleteAsync(int orgId, int id)
        {
            var emp = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == orgId);
            if (emp is null) return OpResult.NotFound;
            _appDbContext.Employees.Remove(emp);
            await _appDbContext.SaveChangesAsync();
            return OpResult.Success;
        }

        public async Task<List<EmployeeDTO>> GetByOrganizationAsync(int orgId)
        {
            var list = await _appDbContext.Employees.AsNoTracking()
                .Include(e => e.Organization)
                .Where(e => e.OrganizationId == orgId)
                .ToListAsync();

            return list.Select(ToDto).ToList();
        }

        public async Task<EmployeeDTO?> GetOwnedAsync(int id, int orgId)
        {
            var e = await _appDbContext.Employees.AsNoTracking()
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizationId == orgId);

            return e is null ? null : ToDto(e);
        }
    }
}
