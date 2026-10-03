using EmployeeAPI.Common;
using EmployeeAPI.DTOs;
using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EmployeeAPI.Common.OpResult;
using static EmployeeAPI.DTOs.EmployeeDTO;
using static EmployeeAPI.Models.Roles;

namespace EmployeeAPI.Controllers;

[ApiController]
[Route("api/me")]
[Authorize(Roles = $"{Roles.Employee},{Roles.Admin}")]
public class MeController : ControllerBase
{
    private readonly IEmployeeService _employee;

    public MeController(IEmployeeService employeeService)
    {
        _employee = employeeService;
    }

    private int? EmpId => User.GetInt("empId");
    private bool IsAdminRole => User.IsInRole(Roles.Admin);

    [HttpGet]
    public async Task<ActionResult<EmployeeDTO>> GetMine()
    {
        if (IsAdminRole)
        {
            return Ok("You are an admin!");
        }

        if (EmpId is null) return Forbid();

        var e = await _employee.GetByIdAsync(EmpId!.Value);
        return e is null ? NotFound() : Ok(e);
    }

}
