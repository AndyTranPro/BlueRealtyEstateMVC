using Bogus;
using BSR.Models;

namespace BSR.Services;

public class HomeService
{
    private readonly HomeContext _context;

    public HomeService(HomeContext context)
    {
        _context = context;
    }

    public List<Home> GetHomes(int? minPrice, int? maxPrice, int? minArea, int? maxArea, int? minBath, int? minCar, int? minBed, string? state, string? city)
    {
        IQueryable<Home> homesQuery = _context.Homes.AsQueryable();

        if (minPrice.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Price <= maxPrice);
        }

        if (minArea.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Area >= minArea);
        }

        if (maxArea.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Area <= maxArea);
        }

        if (minBath.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Bathrooms >= minBath);
        }

        if (minCar.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.GarageSpots >= minCar);
        }

        if (minBed.HasValue)
        {
            homesQuery = homesQuery.Where(h => h.Bedrooms >= minBed);
        }

        if (!string.IsNullOrEmpty(state))
        {
            homesQuery = homesQuery.Where(h => h.State.Equals(state));
        }

        if (!string.IsNullOrEmpty(city))
        {
            homesQuery = homesQuery.Where(h => h.City.Equals(city));
        }

        return homesQuery.ToList();
    }

    public Home GetHomeById(int id)
    {
        return _context.Homes.Single(x => x.Id == id);
    }

    public void AddHome(Home home)
    {
        _context.Homes.Add(home);
        _context.SaveChanges();
    }

    public void UpdateHome(Home updatedHome)
    {
        var home = _context.Homes.FirstOrDefault(h => h.Id == updatedHome.Id);

        home.Price = updatedHome.Price;
        home.StreetAddress = updatedHome.StreetAddress;
        home.Area = updatedHome.Area;

        _context.Homes.Update(home);
        _context.SaveChanges();
    }

    public void DeleteHome(int id)
    {
        var home = _context.Homes.FirstOrDefault(h => h.Id == id);

        _context.Homes.Remove(home);
        _context.SaveChanges();
    }
}
