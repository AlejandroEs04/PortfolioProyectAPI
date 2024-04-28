namespace PortfolioAPI.Models
{
    public partial class Employee 
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set;} = "";
        public int RolID { get; set; }
        public decimal Salary { get; set; }
        public bool Active { get; set; } = true;
    }
}