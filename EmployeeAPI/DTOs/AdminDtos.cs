using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.DTOs;

public static class AdminDtos
{

    public record OrganizationDto(int Id, string Name, int EmployeeCount);

    public class OrganizationCreateDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    };

    public class EmployeeAccountCreateDto
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }
    };
}
