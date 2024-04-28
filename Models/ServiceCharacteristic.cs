namespace PortfolioAPI.Models
{
    public partial class ServiceCharacteristic
    {
        public int SerChaID { get; set; }
        public int ServiceID { get; set; }
        public string CharacteristicDesc { get; set; } = "";
    }
}