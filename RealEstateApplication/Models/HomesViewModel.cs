namespace RealEstateApplication.Models
{
    public class HomesViewModel
    {
        public List<Home>? Homes { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }
    }
}
