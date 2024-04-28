namespace PortfolioAPI.Models 
{
    public partial class Sale 
    {
        public int SaleID { get; set; }
        public DateTime SaleDate { get; set; }
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusID { get; set; }
        public bool Active { get; set; }
    }
}