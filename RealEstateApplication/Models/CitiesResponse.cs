namespace RealEstateApplication.Models
{
    public class CitiesResponse
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<string> Data { get; set; }
    }

    public class CityRequest
    {
        public string State { get; set; }
    }
}
