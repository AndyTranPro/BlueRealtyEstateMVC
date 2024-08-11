using Microsoft.AspNetCore.Mvc;
using RealEstateApplication.Models;
using RealEstateApplication.Services;
using System.Diagnostics;

namespace RealEstateApplication.Controllers
{
    public class HomesController : Controller
    {
        private readonly HomeService _homeService;
        private readonly AddressService _addressService;

        public HomesController(HomeService homeService, AddressService addressService)
        {
            _homeService = homeService;
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult GetStates()
        {
            var states = _addressService.GetAmericanStates();
            return Ok(states);
        }

        [HttpPost]
        public IActionResult GetCities([FromBody] CityRequest request)
        {
            var cities = _addressService.GetCitiesInState(request.State);
            return Ok(cities);
        }

        public IActionResult Index(int? minPrice, int? maxPrice, int? minArea, int?maxArea)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var homesViewModel = new HomesViewModel();

            try
            {
                var homes = _homeService.GetHomes();
                // filter homes by MinPrice and MaxPrice if they are provided
                if (minPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price >= minPrice.Value).ToList();
                }
                if (maxPrice.HasValue)
                {
                    homes = homes.Where(h => h.Price <= maxPrice.Value).ToList();
                }
                // filter homes by MinArea and MaxArea if they are provided
                if (minArea.HasValue)
                {
                    homes = homes.Where(h => h.Area >= minArea.Value).ToList();
                }
                if (maxArea.HasValue)
                {
                    homes = homes.Where(h => h.Area <= maxArea.Value).ToList();
                }
                homesViewModel.Homes = homes;
                ViewBag.HomesCount = homes.Count;
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error getting homes from the database: {ex.Message}";
            }
            // pass the MinPrice and MaxPrice to the view so that the user can see the filter values
            homesViewModel.MinPrice = minPrice;
            homesViewModel.MaxPrice = maxPrice;
            // pass the MinArea and MaxArea to the view so that the user can see the filter values
            homesViewModel.MinArea = minArea;
            homesViewModel.MaxArea = maxArea;

            stopwatch.Stop();
            ViewBag.LoadTestTime = stopwatch.Elapsed.TotalSeconds.ToString("F4");

            return View(homesViewModel);
        }

        [HttpGet]
        public IActionResult ClearFilters()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddHomeView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddHome(Home newHome)
        {
            if (!ModelState.IsValid)
            {
                return View("AddHomeView", newHome);
            }

            try
            {
                _homeService.AddHome(newHome);
                TempData["SuccessMessage"] = "Home added successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding home: {ex.Message}";
                return View("AddHomeView", newHome);
            }
        }

        [HttpGet]
        public IActionResult HomeDetailView(int id)
        {
            var home = _homeService.GetHomeById(id);
            return View(home);
        }

        [HttpPost]
        public IActionResult Update(Home updatedHome)
        {
            if (!ModelState.IsValid)
            {
                return View("HomeDetailView", updatedHome);
            }

            try
            {
                _homeService.UpdateHome(updatedHome);
                TempData["SuccessMessage"] = "Home updated successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating home: {ex.Message}";
                return View("HomeDetailView", updatedHome);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _homeService.DeleteHome(id);
                TempData["SuccessMessage"] = "Home deleted successfully!";
                return new OkResult();
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
