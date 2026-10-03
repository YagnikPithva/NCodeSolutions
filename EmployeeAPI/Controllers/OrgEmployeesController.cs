using EmployeeAPI.Common;
using EmployeeAPI.DTOs;
using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EmployeeAPI.Controllers;



[ApiController]
[Route("api/org/employees")]
[Authorize(Roles = Roles.Organization)]
public class OrgEmployeesController : ControllerBase
{
    private readonly IEmployeeService _employee;

    public OrgEmployeesController(IEmployeeService employeeService)
    {
        _employee = employeeService;
    }


    private int OrgId => User.GetInt("orgId")!.Value;


    [HttpGet]
    public async Task<ActionResult<List<EmployeeDTO>>> GetAll() =>
          Ok(await _employee.GetByOrganizationAsync(OrgId));


    [HttpPost]
    public async Task<ActionResult<EmployeeDTO>> Create(EmployeeCreateDto dto)
    {
        try
        {
            var created = await _employee.CreateAsync(OrgId, dto);
            if (created is null) return Conflict(new { message = "Email already exists." });
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine(e.InnerException?.Message);
            Console.WriteLine(e.InnerException?.StackTrace);
            return BadRequest(e.Message);
        }
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EmployeeCreateDto dto)
    {
        return (await _employee.UpdateAsync(OrgId, id, dto)) == OpResult.Success ? NoContent() : NotFound();
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDTO>> Get(int id)
    {
        var employee = await _employee.GetOwnedAsync(id, OrgId);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
      (await _employee.DeleteAsync(OrgId, id)) == OpResult.Success ? NoContent() : NotFound();


}
