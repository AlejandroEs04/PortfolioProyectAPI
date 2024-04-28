namespace PortfolioAPI.Models
{
    public partial class ServicePlan 
    {
        public int ServiceID { get; set; }
        public int PlanID { get; set; }
        public bool Active { get; set; } = true;
    }
}