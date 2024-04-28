namespace PortfolioAPI.Models
{
    public partial class ProductImage
    {
        public int ProImgID { get; set; }
        public int ProductID { get; set; }
        public string ImageURL { get; set; } = "";
    }
}