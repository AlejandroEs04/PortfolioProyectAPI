namespace PortfolioAPI.Models
{
    public partial class Product 
    {
        public int ProductID { get; set; } 
        public string ProductName { get; set; } = "";
        public string ProductDesc { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public decimal UnitCost { get; set; }
        public string Model { get; set; } = "";
        public bool Active { get; set; } = true;
    }
}