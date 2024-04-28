namespace PortfolioAPI.Models
{
    public partial class Plan 
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; } = "";
        public decimal Price { get; set; }
        public string LogoURL { get; set; } = "";
        public bool Active { get; set; } = true;
    }
}