using Newtonsoft.Json;
using RealEstateApplication.Models;
using System.Net.Http;

namespace RealEstateApplication.Services
{
    public class AddressService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AddressService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<string> GetCitiesInState(string state)
        {
            var cities = new List<string>();

            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://countriesnow.space/api/v0.1/countries/state/cities")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { country = "United States", state = state }), System.Text.Encoding.UTF8, "application/json")
            };

            try
            {
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var citiesResponse = JsonConvert.DeserializeObject<CitiesResponse>(responseContent);

                    return citiesResponse.Data;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch cities: {response.ReasonPhrase}");
                    return cities;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch cities.");
                return cities;
            }
        }

        public List<string> GetAmericanStates()
        {
            var states = new List<string>();

            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://countriesnow.space/api/v0.1/countries/states")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { country = "United States" }), System.Text.Encoding.UTF8, "application/json")
            };

            try
            {
                var response = httpClient.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var statesResponse = JsonConvert.DeserializeObject<StatesResponse>(responseContent);

                    return statesResponse.Data.States.Select(state => state.Name).ToList();
                }
                else
                {
                    Console.WriteLine($"Failed to fetch states: {response.ReasonPhrase}");
                    return states;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch states.");
                return states;
            }
        }
    }
}
