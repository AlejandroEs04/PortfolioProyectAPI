namespace PortfolioAPI.Models
{
    public partial class Auth 
    {
        public int UserID { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}