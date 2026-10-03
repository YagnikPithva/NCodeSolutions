namespace EmployeeAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";   // "User" or "Admin"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Set only when Role == Organization
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        // Set only when Role == Employee (links the login to an existing employee record)
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
