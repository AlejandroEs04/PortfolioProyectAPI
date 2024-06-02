namespace PortfolioAPI.Dtos
{
    public partial class PlanAddDto 
    {
        public string PlanName { get; set; } = "";
        public decimal Price { get; set; }
        public string LogoURL { get; set; } = "";
    }
}