namespace PortfolioAPI.Models
{
    public partial class Suscription
    {
        public int ServiceID { get; set; }
        public int PlanID { get; set; }
        public DateTime SuscriptionDate { get; set; }
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusID { get; set; }
    }
}