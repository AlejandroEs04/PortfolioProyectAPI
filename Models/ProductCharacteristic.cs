namespace PortfolioAPI.Models
{
    public partial class ProductCharacteristic
    {
        public int ProChaID { get; set; }
        public int ProductID { get; set; }
        public string CharacteristicDesc { get; set; } = "";
    }
}