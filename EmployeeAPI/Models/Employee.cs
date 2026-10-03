namespace EmployeeAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime CreatedAt {  get; set; } = DateTime.Now;


        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}