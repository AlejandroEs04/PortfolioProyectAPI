namespace PortfolioAPI.Models
{
    public partial class ProductSpecification
    {
        public int ProductID { get; set; }
        public int SpecificationID { get; set; }
        public string SpecificationDesc { get; set; } = "";
    }
}