using Bogus;
using Microsoft.EntityFrameworkCore;
using RealEstateApplication.Models;
using System.Net;

namespace RealEstateApplication.Services
{
    public class HomeService
    {
        private const string DefaultImageUrl = "https://via.placeholder.com/300x200.png?text=Default+House+Image";
        private List<Home> _homes;
        private readonly HomeContext _context;


        public HomeService(HomeContext context)
        {
            _context = context;
            SeedHomes();
        }

        public void SeedHomes()
        {
            var homes = new List<Home>();

            var imageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
                "https://images.unsplash.com/photo-1570129477492-45c003edd2be",
                "https://plus.unsplash.com/premium_photo-1661964475795-f0cb85767a88",
                "https://images.unsplash.com/photo-1580587771525-78b9dba3b914",
                "https://images.unsplash.com/photo-1613977257363-707ba9348227",
                "https://images.pexels.com/photos/106399/pexels-photo-106399.jpeg",
                "https://images.pexels.com/photos/9951999/pexels-photo-9951999.jpeg",
                "https://images.pexels.com/photos/5178060/pexels-photo-5178060.jpeg",
                "https://images.pexels.com/photos/8583869/pexels-photo-8583869.jpeg",
                "https://images.pexels.com/photos/7710011/pexels-photo-7710011.jpeg",
                "https://images.pexels.com/photos/5524205/pexels-photo-5524205.jpeg"
            };

            if (!_context.Homes.Any())
            {
                var faker = new Faker("en");

                for (int i = 1; i < 500; i++)
                {
                    var home = new Home
                    {
                        Id = i,
                        Price = faker.Finance.Amount(200000, 700000), // Random price between 200,000 and 700,000
                        Address = faker.Address.StreetAddress(), // Generate a realistic street address
                        Area = faker.Random.Int(100, 200), // Random area between 100 and 200 square meters
                        ImageUrl = imageUrls[i % imageUrls.Count]
                    };

                    homes.Add(home);
                }

                _context.Homes.AddRange(homes);
            }

            _context.SaveChanges();
        }
        public List<Home> GetHomes()
        {
            return _context.Homes.ToList();
        }

        public Home GetHomeById(int id)
        {
            return _context.Homes.Single(h => h.Id == id);
        }

        public void AddHome(Home home)
        {
            if (string.IsNullOrEmpty(home.ImageUrl))
            {
                home.ImageUrl = DefaultImageUrl;
            }
            _context.Homes.Add(home);
            _context.SaveChanges();
        }

        public void DeleteHome(int id)
        {
            Console.WriteLine($"Home with ID {id} deleted successfully.");
            var home = _context.Homes.FirstOrDefault(h => h.Id == id);
            Console.WriteLine(home);
            Console.WriteLine("Current list of homes:");
            foreach (var h in _context.Homes)
            {
                Console.WriteLine($"ID: {h.Id}, Price: {h.Price}, Address: {h.Address}, Area: {h.Area}");
            }
            _context.Homes.Remove(home);
            _context.SaveChanges();
        }

        public void UpdateHome(Home updatedHome)
        {
            var home = _context.Homes.FirstOrDefault(h => h.Id == updatedHome.Id);

            home.Price = updatedHome.Price;
            home.Address = updatedHome.Address;
            home.Area = updatedHome.Area;

            _context.Homes.Update(home);
            _context.SaveChanges();
        }
    }
}
