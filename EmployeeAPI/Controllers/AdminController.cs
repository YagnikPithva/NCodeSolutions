using EmployeeAPI.Common;
using EmployeeAPI.DTOs;
using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EmployeeAPI.DTOs.AdminDtos;
using static EmployeeAPI.Models.Roles;

namespace EmployeeAPI.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ControllerBase
{

    private readonly IAdminService _admin;
    private readonly IEmployeeService _employee;

    public AdminController(IAdminService adminService, IEmployeeService employeeService)
    {
        _admin = adminService;
        _employee = employeeService;
    }


    [HttpPost("organizations")]
    public async Task<ActionResult<OrganizationDto>> CreateOrganization(OrganizationCreateDto dto)
    {
        var org = await _admin.CreateOrganizationAsync(dto);
        return org is null
            ? Conflict(new { message = "Organization name or username already exists." })
            : StatusCode(StatusCodes.Status201Created, org);
    }


    [HttpGet("organizations")]
    public async Task<ActionResult<List<OrganizationDto>>> GetOrganizations() =>
       Ok(await _admin.GetOrganizationsAsync());


    [HttpGet("employees")]
    public async Task<ActionResult<List<EmployeeDTO>>> GetEmployees()
    {
        return Ok(await _employee.GetAllAsync());
    }



    [HttpPost("employee-accounts")]
    public async Task<IActionResult> CreateEmployeeAccount(EmployeeAccountCreateDto dto)
    {
        return await _admin.CreateEmployeeAccountAsync(dto) switch
        {
            OpResult.Success => StatusCode(StatusCodes.Status201Created, new { message = "Employee login created." }),
            OpResult.NotFound => NotFound(new { message = "Employee record not found." }),
            _ => Conflict(new { message = "Employee already has a login, or username is taken." })
        };
    }


}
