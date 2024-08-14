using BSR.Models;
using Newtonsoft.Json;
using System;

namespace BSR.Services;

public class AddressService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AddressService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<string>> GetAmericanStates()
    {
        var states = new List<string>();

        var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://countriesnow.space/api/v0.1/countries/states")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { country = "United States" }), System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await httpClient.SendAsync(request);
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

    public async Task<List<string>> GetCitiesInState(string state)
    {
        var cities = new List<string>();

        var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://countriesnow.space/api/v0.1/countries/state/cities")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { country = "United States", state = state }), System.Text.Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await httpClient.SendAsync(request);
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

    public async Task<List<string>> GetStatesWithDelayAsync(int index)
    {
        Console.WriteLine("Request " + index);

        // Simulate a 2-second delay
        await Task.Delay(2000);

        // Return a mock list of states after the delay
        return new List<string> { "State 1", "State 2", "State 3" };
    }
}