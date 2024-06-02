using PortfolioAPI.Models;

namespace PortfolioAPI.Dtos
{
    public partial class ProductAddDto
    {
        public string ProductName { get; set; } = "";
        public string ProductDesc { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitCost { get; set; }
        public string Model { get; set; } = "";
        public int CategoryID { get; set; }
        public bool Active { get; set; } = true;
        public IEnumerable<ProductSpecificationAddDto> Specifications { get; set; } = [];
        public IEnumerable<ProductImageAddDto> Images { get; set; } = [];
    }
}