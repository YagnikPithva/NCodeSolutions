namespace EmployeeAPI.DTOs
{
    public record EmployeeDTO
    (
        int Id,
        string FullName,
        int Age,
        string Email,
        string MobileNumber,
        string Address,
        string Designation,
        decimal Salary,
        int OrganizationId,
        string OrganizationName
    );
}
