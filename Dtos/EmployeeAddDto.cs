namespace PortfolioAPI.Dtos
{
    public partial class EmployeeAddDto 
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public int RolID { get; set; }
        public decimal Salary { get; set; }
    }
}