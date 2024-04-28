namespace PortfolioAPI.Models
{
    public partial class Service 
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set;} = "";
        public string ServiceDesc { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public bool Active { get; set; } = true;
    }
}