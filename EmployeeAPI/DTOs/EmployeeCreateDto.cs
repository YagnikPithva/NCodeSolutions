using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.DTOs
{
    public class EmployeeCreateDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, Range(18, 100)]
        public int Age { get; set; }

        [Required, Phone]
        public string MobileNumber { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Address { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Designation { get; set; } = string.Empty;

        [Range(0, 10000000)]
        public decimal Salary { get; set; }
    }
}
