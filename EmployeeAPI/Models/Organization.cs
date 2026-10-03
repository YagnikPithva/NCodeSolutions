namespace EmployeeAPI.Models
{
    public class Organization
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = [];
    }
}
